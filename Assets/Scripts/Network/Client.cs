using GameNet;
using GameNet.Messages;
using Pong.Network.Messages;
using System.Threading.Tasks;
using Pong.Network.Serializers;
using System.Collections.Generic;

namespace Pong.Network
{
    class Client: Endpoint
    {
        public GameNet.Client BaseClient { get; }
        public int? PlayerId { get; private set; }
        public Player Player { get; private set; }

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
            types.RegisterMessageType<Player>(new PlayerSerializer(), player => {
                UnityEngine.Debug.Log("klappto...");
                AddPlayer(player);
            });
        }

        void RegisterEvents()
        {}

        public void Connect(string ip, ushort tcpPort) => BaseClient.Connect(ip, tcpPort);
        public Task Disconnect() => BaseClient.Disconnect();

        async public Task JoinLobby(string playerName)
        {
            if (PlayerId != null)
                return;
            
            await BaseClient.Send(new PlayerName(playerName, BaseClient.Secret), ProtocolType.Udp);
        }

        override protected void AddPlayer(Player player)
        {
            UnityEngine.Debug.Log("commmmmmm");

            base.AddPlayer(player);

            if (player.Id == PlayerId) {
                Player = player;
            }
        }
    }
}
