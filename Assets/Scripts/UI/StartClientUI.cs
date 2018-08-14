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
        [SerializeField] UnityEngine.UI.InputField _serverUdpPortInput;

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
            => _serverUdpPortHeading.gameObject.SetActive(actAsServer);

        void StartClient()
        {
            _messageText.text = null;

            GameManager game = GameManager.Instance;

            string ip = _ipInput.text;
            ushort tcpPort = (ushort)int.Parse(_tcpPortInput.text);
            ushort udpPort = (ushort)int.Parse(_udpPortInput.text);

            if (_actAsServerToggle.isOn) {
                ushort serverUdpPort = (ushort)int.Parse(_serverUdpPortInput.text);

                try {
                    game.CreateServer(ip, tcpPort, serverUdpPort);
                    game.Server.Start();
                } catch (Exception e) {
                    game.StopServer();
                    _messageText.text = e.Message;
                    return;
                }
            }

            try {
                game.CreateClient(udpPort);
                game.Client.Connect(ip, tcpPort);
                game.LoadLobby();
            } catch (Exception e) {
                game.StopServer();
                game.StopClient();
                
                _messageText.text = e.Message;
            }
        }
    }
}