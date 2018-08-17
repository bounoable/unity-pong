using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class AcceptChallengeSerializer: ObjectSerializer<AcceptChallenge>
    {
        override public byte[] GetBytes(AcceptChallenge message)
            => Build().String(message.Secret).String(message.AckToken).Data;

        override public AcceptChallenge GetObject(byte[] data)
            => new AcceptChallenge(PullString(ref data), PullString(ref data));
    }
}