using System;
using UnityEngine;

namespace Pong.Network
{
    class GamePlayer: Player
    {
        public event Action<int> PointsChanged = delegate {};
        public event Action<Vector3> PositionChanged = delegate {};

        public int Points
        {
            get { return _points; }
            set
            {
                _points = value;
                PointsChanged(_points);
            }
        }

        public Vector3 PaddlePosition
        {
            get { return _paddlePosition; }
            set
            {
                _paddlePosition = value;
                PositionChanged(_paddlePosition);
            }
        }

        int _points;
        Vector3 _paddlePosition = Vector2.zero;

        public GamePlayer(int id, string name): base(id, name)
        {}
    }
}