using System;
using GameNet;
using System.Linq;
using GameNet.Messages;
using Pong.Network.Messages;
using System.Threading.Tasks;
using Pong.Network.Serializers;
using System.Collections.Generic;

namespace Pong.Network
{
    class Client: Endpoint
    {
        public event Action<Challenge> PlayerChallenged = delegate {};
        public event Action ChallengeDeclined = delegate {};
        public event Action<GameStateUpdate> GameStateUpdated = delegate {};

        public GameNet.Client BaseClient { get; }
        public int? PlayerId { get; private set; }
        public Player Player { get; private set; }

        readonly HashSet<Player> _players = new HashSet<Player>();

        public Client(GameNet.Client client)
        {
            BaseClient = client;

            RegisterMessageTypes();
            RegisterEvents();
        }

        void RegisterMessageTypes()
        {
            MessageTypeConfig types = BaseClient.Messenger.TypeConfig;

            types.RegisterMessageType<PlayerId>(new PlayerIdSerializer(), message => PlayerId = message.Id);
            types.RegisterMessageType<PlayerName>(new PlayerNameSerializer());
            types.RegisterMessageType<Player>(new PlayerSerializer(), AddPlayer);
            types.RegisterMessageType<ChallengePlayer>(new ChallengePlayerSerializer());
            types.RegisterMessageType<Messages.Challenge>(new ChallengeSerializer(), HandleChallengeMessage);
            types.RegisterMessageType<DeclineChallenge>(new DeclineChallengeSerializer());
            types.RegisterMessageType<ChallengeDeclined>(new ChallengeDeclinedSerializer(), message => ChallengeDeclined());
            types.RegisterMessageType<AcceptChallenge>(new AcceptChallengeSerializer());
            types.RegisterMessageType<GameStateUpdate>(new GameStateUpdateSerializer(), HandleGameStateUpdateMessage);
        }

        void RegisterEvents()
        {}

        public void Connect(string ip, ushort tcpPort) => BaseClient.Connect(ip, tcpPort);
        public Task Disconnect() => BaseClient.Disconnect();

        Player GetPlayerById(int id)
            => _players.FirstOrDefault(p => p.Id == id);

        async public Task JoinLobby(string playerName)
        {
            if (PlayerId != null)
                return;
            
            await BaseClient.Send(new PlayerName(playerName, BaseClient.Secret), ProtocolType.Udp);
        }

        void AddPlayer(Player player)
        {
            _players.Add(player);

            if (player.Id == PlayerId) {
                Player = player;
            }

            EmitPlayerAdded(player);
        }

        void HandleChallengeMessage(Messages.Challenge message)
        {
            Player challenger = GetPlayerById(message.ChallengerId);
            Player challenged = GetPlayerById(message.ChallengedId);

            if (challenger == null || challenged == null || challenged.Id != Player.Id)
                return;

            PlayerChallenged(new Challenge(challenger, challenged));
        }

        void HandleGameStateUpdateMessage(GameStateUpdate message)
        {
            UnityEngine.Debug.Log("Handle game state update");
        }
    }
}
