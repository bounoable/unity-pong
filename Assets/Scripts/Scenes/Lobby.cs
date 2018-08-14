using Pong.UI;
using Pong.Core;
using UnityEngine;
using Pong.UI.Lobby;
using System.Collections;
using System.Collections.Generic;

namespace Pong.Scenes
{
	class Lobby: MonoBehaviour
	{
		GameManager Game { get; } = GameManager.Instance;

		[SerializeField] PlayerList _playerList;
		[SerializeField] EventButton _leaveBtn;

		void Awake()
		{
			if (!(_playerList && _leaveBtn)) {
				Destroy(gameObject);
				return;
			}

			if (Game.Client == null) {
				Game.LoadMainMenu();
				return;
			}

			_leaveBtn.Click += Leave;

			Game.Client.PlayerAdded += (sender, player) => _playerList.AddPlayer(player);
			Game.Client.PlayerRemoved += (sender, player) => _playerList.RemovePlayer(player);
		}

		async void Leave()
		{
			await Game.StopClient();
			await Game.LoadMainMenu();
		}
	}
}
