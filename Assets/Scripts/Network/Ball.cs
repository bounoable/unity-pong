using System;
using UnityEngine;

namespace Pong.Network
{
    class Ball
    {
        static System.Random _rand = new System.Random();

        public event Action<Vector3> PositionChanged = delegate {};

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                PositionChanged(_position);
            }
        }

        public Vector3 Force { get; set; } = Vector3.zero;

        Vector3 _position = new Vector3(0, 0, -0.5f);

        public void InitForce()
        {
            switch (_rand.Next(0, 2)) {
                case 0:
                    Force = new Vector3(500f, _rand.Next(-250, 251)).normalized;
                    break;
                
                case 1:
                    Force = new Vector3(-500f, _rand.Next(-250, 250)).normalized;
                    break;
            }
        }

        public void Reflect(Vector3 normal)
        {
            Force = Vector3.Reflect(Force, normal).normalized;
        }

        public void Respawn()
        {
            Position = new Vector3(0, 0, -0.5f);
        }
    }
}