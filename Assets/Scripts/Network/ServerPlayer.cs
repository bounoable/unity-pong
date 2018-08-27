namespace Pong.Network
{
    class ServerPlayer: Player
    {
        public GameNet.Player NetPlayer { get; }
        public string Secret { get; }

        public ServerPlayer(GameNet.Player netPlayer, int id, string name): base(id, name)
        {
            NetPlayer = netPlayer;
            Secret = netPlayer.Secret;
        }

        public Player ToBasePlayer()
            => new Player(Id, Name);
    }
}