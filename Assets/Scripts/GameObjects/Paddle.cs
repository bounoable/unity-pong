using UnityEngine;
using Pong.Network;

namespace Pong.GameObjects
{
    class Paddle: MonoBehaviour
    {
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
    }
}