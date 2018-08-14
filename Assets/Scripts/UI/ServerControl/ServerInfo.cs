using Pong.Core;
using UnityEngine;
using Pong.Network;
using UnityEngine.UI;

namespace Pong.UI.ServerControl
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

            _ipText.text = server.BaseServer.IPAddress.ToString();
            _tcpPortText.text = server.BaseServer.Config.Port.ToString();
            _udpPortText.text = server.BaseServer.Config.LocalUdpPort.ToString();
        }
    }
}