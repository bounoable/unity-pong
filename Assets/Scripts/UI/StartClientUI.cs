using System;
using GameNet;
using Pong.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Pong.UI
{
    class StartClientUI: InitGameCanvas
    {
        [SerializeField] Toggle _actAsServerToggle;
        [SerializeField] Text _serverUdpPortHeading;
        [SerializeField] InputField _serverUdpPortInput;

        override protected void Awake()
        {
            base.Awake();

            if (!(_actAsServerToggle && _serverUdpPortHeading && _serverUdpPortInput)) {
                Destroy(gameObject);
            }

            _actAsServerToggle.onValueChanged.AddListener(OnActAsServerChanged);
            _startBtn.Click += StartClient;
        }

        void OnActAsServerChanged(bool actAsServer)
        {
            _serverUdpPortHeading.gameObject.SetActive(actAsServer);
        }

        void StartClient()
        {
            GameManager game = GameManager.Instance;

            string ip = _ipInput.text;
            ushort tcpPort = (ushort)int.Parse(_tcpPortInput.text);
            ushort udpPort = (ushort)int.Parse(_udpPortInput.text);

            if (_actAsServerToggle.isOn) {
                ushort serverUdpPort = (ushort)int.Parse(_serverUdpPortInput.text);

                try {
                    game.CreateServer(ip, tcpPort, serverUdpPort);
                } catch (Exception e) {
                    _messageText.text = e.Message;
                    return;
                }
            }

            try {
                game.CreateClient(ip, tcpPort, udpPort);
                game.LoadLobby().ConfigureAwait(false);
            } catch (Exception e) {
                _messageText.text = e.Message;
            }
        }
    }
}