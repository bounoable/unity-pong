using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class MovePaddleSerializer: ObjectSerializer<MovePaddle>
    {
        override public byte[] GetBytes(MovePaddle message)
            => Build().Enum<MovePaddle.MoveDirection>(message.Direction).Int(message.SessionId).String(message.Secret).Data;
        
        override public MovePaddle GetObject(byte[] data)
            => new MovePaddle(PullEnum<MovePaddle.MoveDirection>(ref data), PullInt(ref data), PullString(ref data));
    }
}