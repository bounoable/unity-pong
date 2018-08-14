using GameNet;
using System.Net;
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
        }

        void RegisterEvents()
        {}

        public void Start() => BaseServer.Start();
        public void Stop() => BaseServer.Stop();

        async void HandlePlayerNameMessage(PlayerName message)
        {
            GameNet.Player netPlayer = BaseServer.GetPlayerBySecret(message.Secret);

            if (netPlayer == null)
                return;
            
            foreach (ServerPlayer p in _players) {
                if (p.NetPlayer == netPlayer) {
                    return;
                }
            }

            ServerPlayer player = new ServerPlayer(netPlayer, NextPlayerId, message.Name);

            await BaseServer.SendTo(netPlayer, new PlayerId(player.Id));
            await Task.Delay(500);
            
            AddPlayer(player);
            
            BaseServer.Send((Player)player, ProtocolType.Udp);
        }
    }
}