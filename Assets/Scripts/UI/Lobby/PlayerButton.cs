using System;
using UnityEngine;
using Pong.Network;
using UnityEngine.UI;

namespace Pong.UI.Lobby
{
    [RequireComponent(typeof(Button))]
    class PlayerButton: MonoBehaviour
    {
        public event EventHandler<Player> Click = delegate {};

        Player _player;
        public Player Player
        {
            get
            {
                return _player;
            }
            set
            {
                _player = value;
                _buttonText.text = _player.Name;
            }
        }

        Button _button;
        Text _buttonText;

        void Awake()
        {
            _button = GetComponent<Button>();
            _buttonText = _button.GetComponentInChildren<Text>();
            _button.onClick.AddListener(() => Click(this, Player));
        }
    }
}