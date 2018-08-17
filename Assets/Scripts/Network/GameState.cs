using UnityEngine;
using Pong.Network.Messages;

namespace Pong.Network
{
    class GameState
    {
        public GamePlayer Player1 { get; }
        public GamePlayer Player2 { get; }
        public GamePlayer Winner => GetWinner();
        public GamePlayer Loser => GetLoser();

        public GameState(GamePlayer player1, GamePlayer player2)
        {
            Player1 = player1;
            Player2 = player2;
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

        public GameStateUpdate GetUpdate()
            => new GameStateUpdate(Player1.PaddlePosition, Player2.PaddlePosition, Player1.Points, Player2.Points);
    }
}