using System;

namespace Pong.Network
{
    class Endpoint
    {
        public event Action<Player> PlayerAdded = delegate {};
        public event Action<Player> PlayerRemoved = delegate {};

        public void EmitPlayerAdded(Player player)
            => PlayerAdded(player);
        
        public void EmitPlayerRemoved(Player player)
            => PlayerRemoved(player);
    }
}