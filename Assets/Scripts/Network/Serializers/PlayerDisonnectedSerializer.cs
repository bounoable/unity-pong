using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class PlayerDisconnectedSerializer: ObjectSerializer<PlayerDisconnected>
    {
        override public byte[] GetBytes(PlayerDisconnected message)
            => Build().Int(message.Id).String(message.AckToken).Data;
        
        override public PlayerDisconnected GetObject(byte[] data)
            => new PlayerDisconnected(PullInt(ref data), PullString(ref data));
    }
}