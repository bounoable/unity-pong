using GameNet.Support;
using GameNet.Messages;
using Message = Pong.Network.Messages.Challenge;

namespace Pong.Network.Serializers
{
    class ChallengeSerializer: ObjectSerializer<Message>
    {
        override public byte[] GetBytes(Message message) 
            => Build().Int(message.ChallengerId).Int(message.ChallengedId).String(message.AckToken).Data;
        
        override public Message GetObject(byte[] data)
            => new Message(PullInt(ref data), PullInt(ref data), PullString(ref data));
    }
}