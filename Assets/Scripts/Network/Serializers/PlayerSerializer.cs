using GameNet.Messages;

namespace Pong.Network.Serializers
{
    class PlayerSerializer: ObjectSerializer<Player>
    {
        override public byte[] GetBytes(Player player)
            => Build().Int(player.Id).String(player.Name).Data;
        
        override public Player GetObject(byte[] data)
            => new Player(PullInt(ref data), PullString(ref data));
    }
}