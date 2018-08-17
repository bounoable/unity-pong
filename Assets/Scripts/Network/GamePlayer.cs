using UnityEngine;

namespace Pong.Network
{
    class GamePlayer: Player
    {
        public int Points { get; set; }
        public Vector2 PaddlePosition { get; set; } = Vector2.zero;

        public GamePlayer(int id, string name): base(id, name)
        {}
    }
}