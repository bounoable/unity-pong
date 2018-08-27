using UnityEngine;

namespace Pong.GameObjects
{
    class Ball: MonoBehaviour
    {
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
    }
}