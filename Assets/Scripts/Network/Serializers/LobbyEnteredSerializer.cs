using GameNet.Messages;
using Pong.Network.Messages;

namespace Pong.Network.Serializers
{
    class LobbyEnteredSerializer: ObjectSerializer<LobbyEntered>
    {
        override public byte[] GetBytes(LobbyEntered message)
            => Build().String(message.Secret).String(message.AckToken).Data;
        
        override public LobbyEntered GetObject(byte[] data)
            => new LobbyEntered(PullString(ref data), PullString(ref data));
    }
}