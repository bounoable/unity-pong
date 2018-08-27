using GameNet;
using Pong.Core;
using System.Net;
using System.Linq;
using Pong.Network;
using GameNet.Messages;
using Pong.Network.Messages;
using System.Threading.Tasks;
using Pong.Network.Serializers;
using System.Collections.Generic;

namespace Pong.Network
{
    class Server: Endpoint
    {
        #region properties
        public GameNet.Server BaseServer { get; }

        public int NextPlayerId
        {
            get
            {
                int id = _nextPlayerId;
                _nextPlayerId++;
                return id;
            }
        }

        public int NextSessionId
        {
            get
            {
                int id = _nextSessionId;
                _nextSessionId++;
                return id;
            }
        }
        #endregion

        #region fields
        int _nextPlayerId = 1;
        int _nextSessionId = 1;

        readonly Dictionary<string, ServerPlayer> _players = new Dictionary<string, ServerPlayer>();
        readonly HashSet<Challenge> _challenges = new HashSet<Challenge>();
        readonly Dictionary<int, GameState> _sessions = new Dictionary<int, GameState>();
        #endregion

        public Server(GameNet.Server server)
        {
            BaseServer = server;

            RegisterMessageTypes();
            RegisterEvents();
        }

        void RegisterMessageTypes()
        {
            MessageTypeConfig types = BaseServer.Messenger.TypeConfig;

            types.RegisterMessageType<PlayerId>(new PlayerIdSerializer());
            types.RegisterMessageType<PlayerName>(new PlayerNameSerializer(), HandlePlayerNameMessage);
            types.RegisterMessageType<Player>(new PlayerSerializer());
            types.RegisterMessageType<ChallengePlayer>(new ChallengePlayerSerializer(), HandleChallengePlayerMessage);
            types.RegisterMessageType<Messages.Challenge>(new ChallengeSerializer());
            types.RegisterMessageType<DeclineChallenge>(new DeclineChallengeSerializer(), HandleDeclineChallengeMessage);
            types.RegisterMessageType<ChallengeDeclined>(new ChallengeDeclinedSerializer());
            types.RegisterMessageType<AcceptChallenge>(new AcceptChallengeSerializer(), HandleAcceptChallengeMessage);
            types.RegisterMessageType<GameStateUpdate>(new GameStateUpdateSerializer());
            types.RegisterMessageType<SessionStarted>(new SessionStartedSerializer());
            types.RegisterMessageType<PlayerDisconnected>(new PlayerDisconnectedSerializer());
            types.RegisterMessageType<MovePaddle>(new MovePaddleSerializer(), HandleMovePaddleMessage);
            types.RegisterMessageType<LobbyEntered>(new LobbyEnteredSerializer(), HandleLobbyEnteredMessage);
        }

        void RegisterEvents()
        {
            BaseServer.PlayerDisconnected += (sender, args) => {
                ServerPlayer player = GetPlayerBySecret(args.Player.Secret);

                if (player == null)
                    return;
                
                _players.Remove(player.Secret);
                
                BaseServer.Send(new PlayerDisconnected(player.Id), ProtocolType.Udp);
            };
        }

        public void Start() => BaseServer.Start();
        public void Stop() => BaseServer.Stop();

        ServerPlayer GetPlayerById(int id)
            => _players.Values.FirstOrDefault(p => p.Id == id);

        ServerPlayer GetPlayerBySecret(string secret)
        {
            ServerPlayer player;

            if (_players.TryGetValue(secret, out player)) {
                return player;
            }

            return null;
        }

        void HandleLobbyEnteredMessage(LobbyEntered message)
        {
            GameNet.Player player = BaseServer.GetPlayerBySecret(message.Secret);
            
            foreach (ServerPlayer p in _players.Values) {
                BaseServer.SendTo(player, p.ToBasePlayer(), ProtocolType.Udp);
            }
        }

        async void HandlePlayerNameMessage(PlayerName message)
        {
            if (GetPlayerBySecret(message.Secret) != null)
                return;
            
            GameNet.Player netPlayer = BaseServer.GetPlayerBySecret(message.Secret);

            if (netPlayer == null)
                return;

            ServerPlayer player = new ServerPlayer(netPlayer, NextPlayerId, message.Name);

            await BaseServer.SendTo(netPlayer, new PlayerId(player.Id));
            await Task.Delay(500);
            
            _players[player.Secret] = player;

            BaseServer.Send(player.ToBasePlayer(), ProtocolType.Udp);

            EmitPlayerAdded(player);
        }

        void HandleChallengePlayerMessage(ChallengePlayer message)
        {
            ServerPlayer challenger = GetPlayerBySecret(message.Secret);
            ServerPlayer challenged = GetPlayerById(message.ChallengedId);

            if (challenger == challenged)
                return;

            foreach (Challenge chal in _challenges) {
                if (chal.Challenger == challenger || chal.Challenged == challenged || chal.Challenger == challenged || chal.Challenged == challenger) {
                    return;
                }
            }

            var challenge = new Challenge(challenger, challenged);
            _challenges.Add(challenge);

            BaseServer.SendTo(challenged.NetPlayer, new Messages.Challenge(challenger.Id, challenged.Id));
        }

        void HandleDeclineChallengeMessage(DeclineChallenge message)
        {
            ServerPlayer player = GetPlayerBySecret(message.Secret);

            if (player == null)
                return;
            
            Challenge declined = null;
            
            foreach (Challenge challenge in _challenges) {
                if (challenge.Challenged.Id != player.Id)
                    continue;
                
                ServerPlayer challenger = GetPlayerById(challenge.Challenger.Id);

                BaseServer.SendTo(challenger.NetPlayer, new ChallengeDeclined());
                declined = challenge;
                
                break;
            }

            if (declined != null) {
                _challenges.Remove(declined);
            }
        }

        void HandleAcceptChallengeMessage(AcceptChallenge message)
        {
            ServerPlayer player = GetPlayerBySecret(message.Secret);

            if (player == null)
                return;
            
            Challenge accepted = null;
            
            foreach (Challenge challenge in _challenges) {
                if (challenge.Challenged.Id != player.Id)
                    continue;
                
                ServerPlayer challenger = GetPlayerById(challenge.Challenger.Id);
                ServerPlayer challenged = GetPlayerById(challenge.Challenged.Id);

                GameState session = new GameState(NextSessionId, challenger.CreateGamePlayer(), challenged.CreateGamePlayer());
                _sessions[session.Id] = session;

                var sessionStarted = new SessionStarted(session.Id, challenger.Id, challenged.Id);

                BaseServer.SendTo(challenger.NetPlayer, sessionStarted);
                BaseServer.SendTo(challenged.NetPlayer, sessionStarted);
                accepted = challenge;

                Task.Run(() => RunGame(session).ConfigureAwait(false));
                
                break;
            }

            if (accepted != null) {
                _challenges.Remove(accepted);
            }
        }

        async Task RunGame(GameState session)
        {
            session.SpawnBall();

            bool gameOver = false;

            while (!gameOver) {
                await Task.Delay(20);
                session.UpdateBallPosition();
                await SendGameState(session);

                gameOver = session.Winner != null;
            }
        }

        async Task SendGameState(GameState session)
        {
            ServerPlayer serverPlayer1 = GetPlayerById(session.Player1.Id);
            ServerPlayer serverPlayer2 = GetPlayerById(session.Player2.Id);

            GameStateUpdate update = session.GetUpdate();

            await Task.WhenAll(
                BaseServer.SendTo(serverPlayer1.NetPlayer, update, GameNet.ProtocolType.Udp),
                BaseServer.SendTo(serverPlayer2.NetPlayer, update, GameNet.ProtocolType.Udp)
            );
        }

        void HandleMovePaddleMessage(MovePaddle message)
        {
            ServerPlayer player = GetPlayerBySecret(message.Secret);

            if (player == null)
                return;
            
            GameState session;

            if (!_sessions.TryGetValue(message.SessionId, out session))
                return;
            
            session.MovePaddle(player, message.Direction);
        }
    }
}
