using System;
using GameNet;
using Pong.Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Pong.UI
{
    class ServerLog: MonoBehaviour
    {
        Server Server { get; } = GameManager.Instance.Server;

        [SerializeField] Text _logText;

        readonly List<string> _logs = new List<string>();

        void Awake()
        {
            if (!_logText) {
                Destroy(gameObject);
                return;
            }

            if (Server == null)
                return;
        
            Server.ServerStopped += () => Log("Server stopped");
        }

        public void Log(string message)
        {
            _logs.Add(message);

            if (_logs.Count > 100) {
                _logs.RemoveAt(0);
            }

            UpdateLogText();
        }

        void UpdateLogText()
        {
            foreach (string log in _logs) {
                string text = string.IsNullOrEmpty(_logText.text) ? string.Empty : $"{_logText.text}\n";
                _logText.text = $"{text}[{DateTime.Now.ToString("HH:mm:ss")}]: {log}";
            }
        }
    }
}