using System;
using UnityEngine;
using Pong.Network;
using UnityEngine.UI;

namespace Pong.UI.Lobby
{
    class PlayerButton: EventButton
    {
        new public event Action<Player> Click = delegate {};

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

        bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                _buttonText.fontStyle = value ? FontStyle.Bold : FontStyle.Normal;
            }
        }

        Text _buttonText;

        override protected void Awake()
        {
            base.Awake();
            
            _buttonText = _button.GetComponentInChildren<Text>();
            base.Click += () => Click(Player);
        }
    }
}