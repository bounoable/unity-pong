using System;
using Pong.Core;
using UnityEngine;
using System.Linq;
using Pong.Network;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Pong.UI.Lobby
{
    class PlayerList: MonoBehaviour
    {
        public event Action<Player> PlayerChallenged = delegate {};

        [SerializeField] Text _headingText;
        [SerializeField] RectTransform _listContent;
        [SerializeField] PlayerButton _playerBtnPrefab;
        [SerializeField] EventButton _challengeBtn;

        readonly HashSet<Player> _players = new HashSet<Player>();
        readonly HashSet<PlayerButton> _playerButtons = new HashSet<PlayerButton>();

        Player _selectedPlayer;
        
        void Awake()
        {
            if (!(_headingText && _listContent && _playerBtnPrefab && _challengeBtn)) {
                Destroy(gameObject);
                return;
            }

            _challengeBtn.Click += () => {
                if (_selectedPlayer != null) {
                    PlayerChallenged(_selectedPlayer);
                }
            };
        }

        void Start()
        {
            _challengeBtn.Interactable = false;
        }

        public void AddPlayer(Player player)
        {
            if (player == null || _players.Contains(player))
                return;

            PlayerButton button = Instantiate(_playerBtnPrefab) as PlayerButton;
            button.transform.SetParent(_listContent);
            button.transform.localScale = Vector3.one;
            button.Player = player;
            button.Click += p => Dispatcher.Instance.Enqueue(() => OnPlayerSelected(p));

            _players.Add(player);
            _playerButtons.Add(button);

            UpdateHeading();
        }

        public void RemovePlayer(Player player)
        {
            if (player == null || !_players.Contains(player))
                return;
            
            _playerButtons.RemoveWhere(button => {
                if (button.Player != player) {
                    return false;
                }

                Destroy(button.gameObject);

                return true;
            });

            _players.Remove(player);

            if (_players.Count == 0) {
                _challengeBtn.Interactable = false;
            }

            UpdateHeading();
        }

        void UpdateHeading() => _headingText.text = $"Players ({_players.Count})";

        void OnPlayerSelected(Player player)
        {
            _selectedPlayer = player;
            _challengeBtn.Interactable = player != null && player != GameManager.Instance.Client.Player;
        }
    }
}