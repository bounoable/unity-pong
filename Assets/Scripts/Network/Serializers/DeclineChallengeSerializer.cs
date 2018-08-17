using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class DeclineChallengeSerializer: ObjectSerializer<DeclineChallenge>
    {
        override public byte[] GetBytes(DeclineChallenge message)
            => Build().String(message.Secret).String(message.AckToken).Data;

        override public DeclineChallenge GetObject(byte[] data)
            => new DeclineChallenge(PullString(ref data), PullString(ref data));
    }
}