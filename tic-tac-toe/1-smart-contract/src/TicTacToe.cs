using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;

namespace AElf.Contracts.TicTacToe
{
    // Contract class must inherit the base class generated from the proto file
    public class TicTacToe : TicTacToeContainer.TicTacToeBase
    {
     private const int BoardSize = 3;

        public override Empty Initialize(Empty input)
        {
            if (State.Initialized.Value)
            {
                return new Empty();
            }
            State.Initialized.Value = true;
            State.Owner.Value = Context.Sender;
            ResetBoard();
            return new Empty();
        }

        public override StringValue StartGame(Empty input)
        {
            if (!State.Initialized.Value)
            {
                return new StringValue { Value = "Contract not initialized." };
            }

            ResetBoard();
            State.CurrentPlayer.Value = "X";
            State.GameStatus.Value = "ongoing";
            State.Winner.Value = "";
            return new StringValue { Value = "Game started. Player X's turn." };
        }

        public override StringValue MakeMove(MoveInput input)
        {
            if (State.GameStatus.Value != "ongoing")
            {
                return new StringValue { Value = "Game is not ongoing." };
            }

            var board = GetBoardArray();
            if (board[input.X, input.Y] != "")
            {
                return new StringValue { Value = "Invalid move. Cell is already occupied." };
            }

            board[input.X, input.Y] = State.CurrentPlayer.Value;
            SaveBoard(board);

            if (CheckWinner())
            {
                State.GameStatus.Value = "finished";
                State.Winner.Value = State.CurrentPlayer.Value;
                return new StringValue { Value = $"Player {State.CurrentPlayer.Value} wins!" };
            }

            if (IsBoardFull(board))
            {
                State.GameStatus.Value = "draw";
                return new StringValue { Value = "It's a draw!" };
            }

            State.CurrentPlayer.Value = State.CurrentPlayer.Value == "X" ? "O" : "X";
            return new StringValue { Value = $"Player {State.CurrentPlayer.Value}'s turn." };
        }

        public override Board GetBoard(Empty input)
        {
              var board = GetBoardArray();
              var boardMessage = new Board();
          
              for (var i = 0; i < 3; i++) // Adjusted to 3 for a 3x3 Tic-Tac-Toe board
              {
                  var row = new List<string>();
                  for (var j = 0; j < 3; j++)
                  {
                      row.Add(board[i, j]);
                  }
                  boardMessage.Rows.Add(string.Join(",", row));
              }
          
              return boardMessage;
        }

        public override GameStatus GetGameStatus(Empty input)
        {
            return new GameStatus
            {
                Status = State.GameStatus.Value,
                Winner = State.Winner.Value
            };
        }

        public override BoolValue GetInitialStatus(Empty input){
              return new BoolValue { Value = State.Initialized.Value };
          }

        private void ResetBoard()
        {
            var emptyBoard = new string[BoardSize, BoardSize];
            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    emptyBoard[i, j] = "";
                }
            }
            SaveBoard(emptyBoard);
        }

        private string[,] GetBoardArray()
        {
            var boardString = State.Board.Value;
            var rows = boardString.Split(';');
            var board = new string[BoardSize, BoardSize];
            for (var i = 0; i < BoardSize; i++)
            {
                var cells = rows[i].Split(',');
                for (var j = 0; j < BoardSize; j++)
                {
                    board[i, j] = cells[j];
                }
            }
            return board;
        }

        private void SaveBoard(string[,] board)
        {
            var rows = new string[BoardSize];
            for (var i = 0; i < BoardSize; i++)
            {
                rows[i] = string.Join(",", board[i, 0], board[i, 1], board[i, 2]);
            }
            State.Board.Value = string.Join(";", rows);
        }

        private bool CheckWinner()
        {
            var board = GetBoardArray();
            var player = State.CurrentPlayer.Value;

            for (var i = 0; i < BoardSize; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) return true;
                if (board[0, i] == player && board[1, i] == player && board[2, i] == player) return true;
            }

            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;

            return false;
        }

        private bool IsBoardFull(string[,] board)
        {
            for (var i = 0; i < BoardSize; i++)
            {
                for (var j = 0; j < BoardSize; j++)
                {
                    if (board[i, j] == "") return false;
                }
            }
            return true;
        }
    }
    
}