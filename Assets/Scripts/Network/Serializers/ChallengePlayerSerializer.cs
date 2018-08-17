using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class ChallengePlayerSerializer: ObjectSerializer<ChallengePlayer>
    {
        override public byte[] GetBytes(ChallengePlayer message)
            => Build().Int(message.ChallengedId).String(message.Secret).String(message.AckToken).Data;
        
        override public ChallengePlayer GetObject(byte[] data)
            => new ChallengePlayer(PullInt(ref data), PullString(ref data), PullString(ref data));
    }
}