using Pong.Core;
using UnityEngine;
using Pong.Network;
using UnityEngine.UI;

namespace Pong.UI.Lobby
{
    class JoinLobby: MonoBehaviour
    {
        Client Client { get; } = GameManager.Instance.Client;

        [SerializeField] Canvas _canvas;
        [SerializeField] Input _playerNameInput;
        [SerializeField] EventButton _joinBtn;

        void Awake()
        {
            if (!(_canvas && _playerNameInput && _joinBtn)) {
                Destroy(gameObject);
                return;
            }

            _playerNameInput.Changed += input => _joinBtn.Interactable = !string.IsNullOrWhiteSpace(input) && input.Length >= 3 && input.Length <= 20;
            _joinBtn.Click += Join;
        }

        void Start()
        {
            _joinBtn.Interactable = false;
        }

        async void Join()
        {
            _playerNameInput.Interactable = false;
            _joinBtn.Interactable = false;

            await Client.JoinLobby(_playerNameInput.Value);

            _canvas.gameObject.SetActive(false);
        }
    }
}