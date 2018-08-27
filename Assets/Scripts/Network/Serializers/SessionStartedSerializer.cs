using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class SessionStartedSerializer: ObjectSerializer<SessionStarted>
    {
        override public byte[] GetBytes(SessionStarted message)
            => Build().Int(message.SessionId).Int(message.ChallengerId).Int(message.ChallengedId).String(message.AckToken).Data;
        
        override public SessionStarted GetObject(byte[] data)
            => new SessionStarted(PullInt(ref data), PullInt(ref data), PullInt(ref data), PullString(ref data));
    }
}