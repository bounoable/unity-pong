using GameNet;
using Pong.UI;
using Pong.Core;
using UnityEngine;
using Pong.Network;
using UnityEngine.UI;
using Pong.GameObjects;
using System.Collections;
using Pong.Network.Messages;
using System.Collections.Generic;

namespace Pong.Scenes
{
	class GameSession: MonoBehaviour
	{
		GameManager Game { get; } = GameManager.Instance;
		Network.Client Client => Game.Client;
		GameState Session => Game.Session;

		public GamePlayer Player1 => Game.Session.Player1;
		public GamePlayer Player2 => Game.Session.Player2;

		[SerializeField] GameObject _field;
		[SerializeField] Text _player1Name;
		[SerializeField] Text _player2Name;
		[SerializeField] Text _player1Points;
		[SerializeField] Text _player2Points;
		[SerializeField] Paddle _player1Paddle;
		[SerializeField] Paddle _player2Paddle;
		[SerializeField] GameObjects.Ball _ball;
		[SerializeField] Canvas _gameOverCanvas;
		[SerializeField] Text _youWonText;
		[SerializeField] Text _youLostText;
		[SerializeField] EventButton _enterLobbyBtn;

		bool _end = false;

		void Awake()
		{
			if (!(_player1Name && _player2Name && _player1Points && _player2Points && _player1Paddle && _player2Paddle && _ball && _gameOverCanvas && _youWonText && _youLostText && _enterLobbyBtn)) {
				Destroy(gameObject);
				return;
			}

			if (Game.Session == null) {
				return;
			}

			_enterLobbyBtn.Click += EnterLobby;

			_player1Name.text = Player1.Name;
			_player2Name.text = Player2.Name;

			Player1.PositionChanged += position => Dispatcher.Instance.Enqueue(() => UpdatePaddlePosition(_player1Paddle, position));
			Player2.PositionChanged += position => Dispatcher.Instance.Enqueue(() => UpdatePaddlePosition(_player2Paddle, position));
			
			Player1.PointsChanged += points => Dispatcher.Instance.Enqueue(() => _player1Points.text = points.ToString());
			Player2.PointsChanged += points => Dispatcher.Instance.Enqueue(() => _player2Points.text = points.ToString());

			Session.Ball.PositionChanged += position => Dispatcher.Instance.Enqueue(() => UpdateBallPosition(position));

			if (Screen.fullScreen) {
				Screen.fullScreen = false;
			}

			Screen.SetResolution(1000, 550, FullScreenMode.Windowed);
		}

		void FixedUpdate()
		{
			if (_end)
				return;
			
			if (UnityEngine.Input.GetKey(KeyCode.W)) {
				Client.BaseClient.Send(MovePaddle.Up(Game.Session.Id, Client.BaseClient.Secret), ProtocolType.Udp);
			} else if (UnityEngine.Input.GetKey(KeyCode.S)) {
				Client.BaseClient.Send(MovePaddle.Down(Game.Session.Id, Client.BaseClient.Secret), ProtocolType.Udp);
			}
		}

		void Update()
		{
			if (Session.Winner != null) {
				bool won = Session.Winner.Id == Client.Player.Id;

				_gameOverCanvas.gameObject.SetActive(true);

				if (won) {
					_youWonText.gameObject.SetActive(true);
				} else {
					_youLostText.gameObject.SetActive(true);
				}
			}
		}

		void UpdatePaddlePosition(Paddle paddle, Vector3 position)
		{
			if (paddle == null)
				return;
			
			paddle.Position = position;
		}

		void UpdateBallPosition(Vector3 position)
		{
			_ball.Position = position;
		}

		async void EnterLobby()
		{
			await Game.LoadLobby();
		}
	}
}
