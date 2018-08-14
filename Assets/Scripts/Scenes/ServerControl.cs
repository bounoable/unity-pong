using Pong.UI;
using Pong.Core;
using UnityEngine;
using Pong.UI.ServerControl;
using System.Threading.Tasks;

namespace Pong.Scenes
{
    class ServerControl: MonoBehaviour
    {
        GameManager Game { get; } = GameManager.Instance;

        [SerializeField] EventButton _stopBtn;
        [SerializeField] ServerLog _log;

        void Awake()
        {
            if (!(_stopBtn && _log)) {
                Destroy(gameObject);
                return;
            }

            _stopBtn.Click += StopServer;

            if (Game.Server == null) {
                Game.LoadMainMenu();
            }
        }

        void Start()
        {
            Game.Server.Start();
        }

        void StopServer()
        {
            Game.StopServer();
            Game.LoadMainMenu().ConfigureAwait(false);
        }
    }
}
