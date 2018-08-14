using UnityEngine;

namespace Pong.Core
{
    class GameBehaviour: MonoBehaviour
    {
        async virtual protected void OnApplicationQuit()
        {
            await GameManager.Instance.PrepareQuit();
        }
    }
}