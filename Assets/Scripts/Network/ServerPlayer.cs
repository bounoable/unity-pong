namespace Pong.Network
{
    class ServerPlayer: Player
    {
        public GameNet.Player NetPlayer { get; }

        public ServerPlayer(GameNet.Player netPlayer, int id, string name): base(id, name)
        {
            NetPlayer = netPlayer;
        }
    }
}