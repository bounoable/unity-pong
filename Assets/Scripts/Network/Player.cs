namespace Pong.Network
{
    class Player
    {
        public int Id { get; }
        public string Name { get; }

        public Player(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public GamePlayer CreateGamePlayer()
            => new GamePlayer(Id, Name);
    }
}