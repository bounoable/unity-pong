using UnityEngine;

namespace Pong.Network.Messages
{
    class GameStateUpdate
    {
        public Vector2 Player1Position { get; }
        public Vector2 Player2Position { get; }
        public int Player1Points { get; }
        public int Player2Points { get; }

        public GameStateUpdate(Vector2 p1Pos, Vector2 p2Pos, int p1Points, int p2Points)
        {
            Player1Position = p1Pos;
            Player2Position = p2Pos;
            Player1Points = p1Points;
            Player2Points = p2Points;
        }
    }
}