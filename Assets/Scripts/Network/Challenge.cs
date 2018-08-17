namespace Pong.Network
{
    class Challenge
    {
        public Player Challenger { get; }
        public Player Challenged { get; }

        public Challenge(Player challenger, Player challenged)
        {
            Challenger = challenger;
            Challenged = challenged;
        }
    }
}