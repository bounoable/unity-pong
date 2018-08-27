using System;
using Pong.Core;
using UnityEngine;
using Pong.Network.Messages;

namespace Pong.Network
{
    class GameState
    {
        const float FieldHeight = 34f;
        const float FieldWidth = 66f;
        const float HalfFieldHeight = 17f;
        const float HalfFieldWidth = 33f;
        const float PaddleHeight = 4f;
        const float HalfPaddleHeight = 2f;

        static Vector3 Player1Spawn = new Vector3(-29, 0, -0.5f);
        static Vector3 Player2Spawn = new Vector3(29, 0, -0.5f);

        public int Id { get; }
        public GamePlayer Player1 { get; }
        public GamePlayer Player2 { get; }
        public GamePlayer Winner => GetWinner();
        public GamePlayer Loser => GetLoser();

        public Ball Ball { get; } = new Ball();

        GameManager Game { get; } = GameManager.Instance;

        public GameState(int id, GamePlayer player1, GamePlayer player2)
        {
            Id = id;
            Player1 = player1;
            Player2 = player2;

            Player1.PaddlePosition = Player1Spawn;
            Player2.PaddlePosition = Player2Spawn;
        }

        public GamePlayer GetWinner()
        {
            if (Player1.Points >= 10) {
                return Player1;
            }

            if (Player2.Points >= 10) {
                return Player2;
            }

            return null;
        }

        public GamePlayer GetLoser()
        {
            var winner = GetWinner();

            return winner != null ? (winner == Player1 ? Player2 : Player1) : null;
        }
        
        public void MovePaddle(Player player, MovePaddle.MoveDirection direction)
        {
            if (Game.Server == null)
                return;
            
            GamePlayer gPlayer = Player1.Id == player.Id ? Player1 : (Player2.Id == player.Id ? Player2 : null);

            if (gPlayer == null)
                return;
            
            Vector3 newPosition = gPlayer.PaddlePosition;
            
            switch (direction) {
                case Messages.MovePaddle.MoveDirection.Up:
                    newPosition = gPlayer.PaddlePosition + Vector3.up * 0.3f;
                    break;
                
                case Messages.MovePaddle.MoveDirection.Down:
                    newPosition = gPlayer.PaddlePosition + Vector3.down * 0.3f;
                    break;
            }

            if (PaddlePositionIsValid(newPosition)) {
                gPlayer.PaddlePosition = newPosition;
            }
        }

        bool PaddlePositionIsValid(Vector3 position)
        {
            return position.y <= (HalfFieldHeight - HalfPaddleHeight) && position.y >= -(HalfFieldHeight - HalfPaddleHeight);
        }

        public void SpawnBall()
        {
            if (Game.Server != null) {
                Ball.InitForce();
            }
        }

        public void UpdateBallPosition()
        {
            if (Game.Server == null)
                return;
            
            Vector3 newPos = Ball.Position + Ball.Force * 0.45f;
            Ball.Position = newPos;

            if (CheckPaddleHit(Player1.PaddlePosition, newPos)) {
                Ball.Reflect(Vector3.right);
            } else if (CheckPaddleHit(Player2.PaddlePosition, newPos)) {
                Ball.Reflect(Vector3.left);
            }

            CheckWallHit();
            CheckGoal();
        }

        bool CheckPaddleHit(Vector3 paddlePos, Vector3 ballPos)
        {
            float paddleTop = paddlePos.y + HalfPaddleHeight;
            float paddleBottom = paddlePos.y - HalfPaddleHeight;

            return Mathf.Abs(paddlePos.x - ballPos.x) <= 0.5f && ballPos.y <= paddleTop && ballPos.y >= paddleBottom;
        }

        void CheckWallHit()
        {
            if (Ball.Position.y <= -HalfFieldHeight) {
                Ball.Reflect(Vector3.up);
            } else if (Ball.Position.y >= HalfFieldHeight) {
                Ball.Reflect(Vector3.down);
            }
        }

        void CheckGoal()
        {
            if (Ball.Position.x <= -HalfFieldWidth) {
                Player2.Points++;
                Ball.Respawn();
                Player1.PaddlePosition = Player1Spawn;
                Player2.PaddlePosition = Player2Spawn;
                Ball.InitForce();
            } else if (Ball.Position.x >= HalfFieldWidth) {
                Player1.Points++;
                Ball.Respawn();
                Player1.PaddlePosition = Player1Spawn;
                Player2.PaddlePosition = Player2Spawn;
                Ball.InitForce();
            }
        }

        public GameStateUpdate GetUpdate()
            => new GameStateUpdate(Player1.PaddlePosition, Player2.PaddlePosition, Player1.Points, Player2.Points, Ball.Position);
        
        public void Update(GameStateUpdate update)
        {
            Player1.PaddlePosition = update.Player1Position;
            Player2.PaddlePosition = update.Player2Position;
            Player1.Points = update.Player1Points;
            Player2.Points = update.Player2Points;
            Ball.Position = update.BallPosition;
        }
    }
}