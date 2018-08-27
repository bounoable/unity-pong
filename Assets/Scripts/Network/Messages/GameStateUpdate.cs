using UnityEngine;

namespace Pong.Network.Messages
{
    class GameStateUpdate
    {
        public Vector3 Player1Position { get; }
        public Vector3 Player2Position { get; }
        public int Player1Points { get; }
        public int Player2Points { get; }
        public Vector3 BallPosition { get; }

        public GameStateUpdate(Vector3 p1Pos, Vector3 p2Pos, int p1Points, int p2Points, Vector3 ballPos)
        {
            Player1Position = p1Pos;
            Player2Position = p2Pos;
            Player1Points = p1Points;
            Player2Points = p2Points;
            BallPosition = ballPos;
        }
    }
}