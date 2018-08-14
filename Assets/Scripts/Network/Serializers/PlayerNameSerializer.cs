using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class PlayerNameSerializer: ObjectSerializer<PlayerName>
    {
        override public byte[] GetBytes(PlayerName message)
            => Build().String(message.Name).String(message.Secret).String(message.AckToken).Data;
        
        override public PlayerName GetObject(byte[] data)
            => new PlayerName(PullString(ref data), PullString(ref data), PullString(ref data));
    }
}