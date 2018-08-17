using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class ChallengeDeclinedSerializer: ObjectSerializer<ChallengeDeclined>
    {
        override public byte[] GetBytes(ChallengeDeclined message)
            => Build().String(message.AckToken).Data;

        override public ChallengeDeclined GetObject(byte[] data)
            => new ChallengeDeclined(PullString(ref data));
    }
}