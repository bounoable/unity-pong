using GameNet;
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
        #endregion

        #region fields
        int _nextPlayerId = 1;
        readonly HashSet<ServerPlayer> _players = new HashSet<ServerPlayer>();
        readonly HashSet<Challenge> _challenges = new HashSet<Challenge>();
        readonly HashSet<GameState> _sessions = new HashSet<GameState>();
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
        }

        void RegisterEvents()
        {}

        public void Start() => BaseServer.Start();
        public void Stop() => BaseServer.Stop();

        ServerPlayer GetPlayerById(int id)
            => _players.FirstOrDefault(p => p.Id == id);

        ServerPlayer GetPlayerBySecret(string secret)
        {
            GameNet.Player netPlayer = BaseServer.GetPlayerBySecret(secret);

            if (netPlayer == null)
                return null;
            
            foreach (ServerPlayer p in _players) {
                if (p.NetPlayer == netPlayer) {
                    return p;
                }
            }

            return null;
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
            
            _players.Add(player);

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

                GameState session = new GameState(challenger.CreateGamePlayer(), player.CreateGamePlayer());
                _sessions.Add(session);

                // BaseServer.SendTo(challenger.NetPlayer, session);
                // BaseServer.SendTo(player.NetPlayer, session);
                accepted = challenge;

                Task.Run(() => SendGameState(session));
                
                break;
            }

            if (accepted != null) {
                _challenges.Remove(accepted);
            }
        }

        async Task SendGameState(GameState session)
        {
            ServerPlayer serverPlayer1 = GetPlayerById(session.Player1.Id);
            ServerPlayer serverPlayer2 = GetPlayerById(session.Player2.Id);

            bool gameOver = false;

            while (!gameOver) {
                await Task.Delay(100);

                GameStateUpdate update = session.GetUpdate();

                await Task.WhenAll(
                    BaseServer.SendTo(serverPlayer1.NetPlayer, update, GameNet.ProtocolType.Udp),
                    BaseServer.SendTo(serverPlayer2.NetPlayer, update, GameNet.ProtocolType.Udp)
                );

                if (session.Winner != null) {
                    gameOver = true;
                }
            }
        }
    }
}