using System;

namespace Pong.Network.Messages
{
    class MovePaddle
    {
        public enum MoveDirection
        {
            Up,
            Down
        }

        public MoveDirection Direction { get; }
        public int SessionId { get; }
        public string Secret { get; }

        public MovePaddle(MoveDirection direction, int sessionId, string secret)
        {
            Direction = direction;
            SessionId = sessionId;
            Secret = secret;
        }


        public static MovePaddle Up(int sessionId, string secret)
            => new MovePaddle(MoveDirection.Up, sessionId, secret);
        
        public static MovePaddle Down(int sessionId, string secret)
            => new MovePaddle(MoveDirection.Down, sessionId, secret);
    }
}