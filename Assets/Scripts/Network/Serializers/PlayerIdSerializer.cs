using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class PlayerIdSerializer: ObjectSerializer<PlayerId>
    {
        override public byte[] GetBytes(PlayerId message)
            => Build().Int(message.Id).String(message.AckToken).Data;
        
        override public PlayerId GetObject(byte[] data)
            => new PlayerId(PullInt(ref data), PullString(ref data));
    }
}