using Pong.UI;
using Pong.Core;
using UnityEngine;
using Pong.Network;
using Pong.UI.Lobby;
using UnityEngine.UI;
using System.Collections;
using Pong.Network.Messages;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pong.Scenes
{
	class Lobby: MonoBehaviour
	{
		GameManager Game { get; } = GameManager.Instance;
		Client Client { get; } = GameManager.Instance.Client;

		[SerializeField] PlayerList _playerList;
		[SerializeField] EventButton _leaveBtn;
		[SerializeField] Canvas _challengePlayerCanvas;
		[SerializeField] EventButton _abortChallengeBtn;
		[SerializeField] Canvas _respondToChallengeCanvas;
		[SerializeField] Text _respondToChallengeHeading;
		[SerializeField] EventButton _acceptChallengeBtn;
		[SerializeField] EventButton _declineChallengeBtn;

		bool _waitingForResponse = false;
		bool WaitingForResponse
		{
			get { return _waitingForResponse; }
			set
			{
				_waitingForResponse = value;
				_challengePlayerCanvas.gameObject.SetActive(value);
			}
		}

		void Awake()
		{
			if (!(_playerList && _leaveBtn && _challengePlayerCanvas && _abortChallengeBtn && _respondToChallengeCanvas && _acceptChallengeBtn && _declineChallengeBtn)) {
				Destroy(gameObject);
				return;
			}

			if (Game.Client == null) {
				Game.LoadMainMenu();
				return;
			}

			_leaveBtn.Click += Leave;
			_playerList.PlayerChallenged += player => Dispatcher.Instance.Enqueue(() => ChallengePlayer(player));
			_abortChallengeBtn.Click += () => Dispatcher.Instance.Enqueue(AbortChallenge);
			_acceptChallengeBtn.Click += () => Dispatcher.Instance.Enqueue(AcceptChallenge);
			_declineChallengeBtn.Click += () => Dispatcher.Instance.Enqueue(DeclineChallenge);

			Game.Client.PlayerAdded += player => Dispatcher.Instance.Enqueue(() => _playerList.AddPlayer(player));
			Game.Client.PlayerRemoved += player => Dispatcher.Instance.Enqueue(() => _playerList.RemovePlayer(player));
			Game.Client.PlayerChallenged += challenge => Dispatcher.Instance.Enqueue(() => OnPlayerChallenged(challenge));
		}

		void Start()
		{
			Game.Client.BaseClient.Send(new LobbyEntered(Game.Client.BaseClient.Secret), GameNet.ProtocolType.Udp);
		}

		async void Leave()
		{
			await Game.StopClient();
			await Game.LoadMainMenu();
		}

		void ChallengePlayer(Player player)
		{
			Client.BaseClient.Send(new ChallengePlayer(player.Id, Client.BaseClient.Secret), GameNet.ProtocolType.Udp);
			WaitingForResponse = true;
		}

		void AbortChallenge()
		{
			WaitingForResponse = false;
		}

		void OnPlayerChallenged(Network.Challenge challenge)
		{
			WaitingForResponse = false;
			_respondToChallengeHeading.text = $"You got challenged by {challenge.Challenger.Name}";
			_respondToChallengeCanvas.gameObject.SetActive(true);
		}

		void AcceptChallenge()
		{
			_acceptChallengeBtn.gameObject.SetActive(false);
			_declineChallengeBtn.gameObject.SetActive(false);
			Client.BaseClient.Send(new AcceptChallenge(Client.BaseClient.Secret), GameNet.ProtocolType.Udp);
		}

		void DeclineChallenge()
		{
			Client.BaseClient.Send(new DeclineChallenge(Client.BaseClient.Secret), GameNet.ProtocolType.Udp);
			_respondToChallengeCanvas.gameObject.SetActive(false);
		}
	}
}
