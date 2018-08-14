using System;
using System.Collections.Generic;

namespace Pong.Network
{
    class Endpoint
    {
        public event EventHandler<Player> PlayerAdded = delegate {};
        public event EventHandler<Player> PlayerRemoved = delegate {};

        readonly protected HashSet<Player> _players = new HashSet<Player>();

        protected bool ContainsPlayer(Player player)
        {
            foreach (Player p in _players) {
                if (p.Id == player.Id) {
                    return true;
                }
            }
            
            return false;
        }

        virtual protected void AddPlayer(Player player)
        {
            if (ContainsPlayer(player))
                return;

            _players.Add(player);

            PlayerAdded(this, player);
        }

        virtual protected void RemovePlayer(Player player)
        {
            if (!ContainsPlayer(player))
                return;
            
            _players.Remove(player);

            PlayerRemoved(this, player);
        }
    }
}