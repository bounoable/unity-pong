using System;
using GameNet;
using Pong.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Pong.UI
{
    class StartServerUI: InitGameCanvas
    {
        override protected void Awake()
        {
            base.Awake();

            _startBtn.Click += StartServer;
        }

        void StartServer()
        {
            _messageText.text = null;
            
            GameManager game = GameManager.Instance;

            string ip = _ipInput.text;
            ushort tcpPort = (ushort)int.Parse(_tcpPortInput.text);
            ushort udpPort = (ushort)int.Parse(_udpPortInput.text);

            try {
                game.CreateServer(ip, tcpPort, udpPort);
                game.LoadServerControl().ConfigureAwait(false);
            } catch (Exception e) {
                _messageText.text = e.Message;
            }
        }
    }
}