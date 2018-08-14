using GameNet;
using Pong.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Pong.UI
{
    class ServerInfo: MonoBehaviour
    {
        [SerializeField] Text _ipText;
        [SerializeField] Text _tcpPortText;
        [SerializeField] Text _udpPortText;

        void Awake()
        {
            if (!(_ipText && _tcpPortText && _udpPortText)) {
                Destroy(gameObject);
                return;
            }

            Server server = GameManager.Instance.Server;

            if (server == null)
                return;

            _ipText.text = server.IPAddress.ToString();
            _tcpPortText.text = server.Config.Port.ToString();
            _udpPortText.text = server.Config.LocalUdpPort.ToString();
        }
    }
}