using System.Collections.Generic;


namespace ConsoleLig4.Core.Services
{

    //Class to simulate plays with virtual boards;
    class SimulatedGameService
    {
        public int[,] Board { get; private set; }
        public bool isFinalStatePosition { get; private set; }
        public int Winner { get; private set; }
        public int evaluatedValue { get; private set; }

        private int BoardColumnSize;
        private int BoardRowSize;
        private int CurrentPlayer;
        private const int PLAYER = 1;
        private const int AIPLAYER = 2;

        private const int PLAYERWINSTATEVALUE = -1000;
        private const int AIWINSSTATEVALUE = 1000;
        private const int DRAWSTATEVALUE = 0;


        public SimulatedGameService(int[,] currentBoard, int currentPlayer)
        {

            Board = currentBoard;

            BoardColumnSize = Board.GetLength(0);
            BoardRowSize = Board.GetLength(1);
            CurrentPlayer = currentPlayer;
            TestForVictory(Board);

        }

        public void SetBoard(int[,] board)
        {
            Board = board;
        }

        private void DropPiece(int[,]board, int position, int piece)
        {
            for (int row = 0; row < BoardColumnSize; row++)
            {
                if (board[position, row] == 0)
                {
                    board[position, row] = piece;
                    break;
                }
            };
            TestForVictory(board);
        }

        private int TestForVictory(int[,] board)
        {
            for (int column = 0; column < BoardColumnSize - 3; column++)
            {
                for (int row = 0; row < BoardRowSize - 3; row++)
                {
                    // HORIZONTAL
                    if (board[column, row] != 0 &&
                        board[column, row] == board[column + 1, row] &&
                        board[column + 1, row] == board[column + 2, row] &&
                        board[column + 2, row] == board[column + 3, row])
                    {

                        EvaluateFinaState(board[column, row]);

                        return board[column, row];
                    }

                    // VERTICAL
                    if (board[column, row] != 0 &&
                        board[column, row] == board[column, row + 1] &&
                        board[column, row + 1] == board[column, row + 2] &&
                        board[column, row + 2] == board[column, row + 3])
                    {
                        EvaluateFinaState(board[column, row]);
                        return board[column, row];
                    }

                    // DIAGONAL 1
                    if (board[column, row] != 0 &&
                        board[column, row] == board[column + 1, row + 1] &&
                        board[column + 1, row + 1] == board[column + 2, row + 2] &&
                        board[column + 2, row + 2] == board[column + 3, row + 3])
                    {
                        EvaluateFinaState(board[column, row]);
                        return board[column, row];
                    }

                    // DIAGONAL 2
                    if (board[column + 3, row] != 0 &&
                        board[column + 3, row] == board[column + 2, row + 1] &&
                        board[column + 2, row + 1] == board[column + 1, row + 2] &&
                        board[column + 1, row + 2] == board[column, row + 3])
                    {
                        EvaluateFinaState(board[column, row]);
                        return board[column + 3, row];
                    }
                }
            }
            for (int column = 0; column < BoardColumnSize - 3; column++)
            {
                for (int row = BoardRowSize - 3; row < BoardRowSize; row++)
                {
                    // HORIZONTAL
                    if (board[column, row] != 0 &&
                        board[column, row] == board[column + 1, row] &&
                        board[column + 1, row] == board[column + 2, row] &&
                        board[column + 2, row] == board[column + 3, row])
                    {
                        EvaluateFinaState(board[column, row]);
                        return board[column, row];
                    }
                }
            }
            for (int column = BoardColumnSize - 3; column < BoardColumnSize; column++)
            {
                for (int row = 0; row < BoardRowSize - 3; row++)
                {
                    // VERTICAL
                    if (board[column, row] != 0 &&
                    board[column, row] == board[column, row + 1] &&
                    board[column, row + 1] == board[column, row + 2] &&
                    board[column, row + 2] == board[column, row + 3])
                    {
                        EvaluateFinaState(board[column, row]);
                        return board[column, row];
                    }
                }
            }
            return 0;
        }

        public int EvaluateFinalScore()
        {
            if (isFinalStatePosition)
            {
                if (Winner == AIPLAYER)
                {
                    return AIWINSSTATEVALUE;
                }
                else if (Winner == PLAYER)
                {
                    return PLAYERWINSTATEVALUE;
                }
                else
                {
                    return DRAWSTATEVALUE;
                }
            }
            int aiEvaluatedScore = EvaluatePlayScore(AIPLAYER);
            int playerEvaluatedScore = EvaluatePlayScore(PLAYER);

            return aiEvaluatedScore - playerEvaluatedScore;
        }

        private void EvaluateFinaState(int winner)
        {
            isFinalStatePosition = true;
            if (winner == PLAYER)
            {
                 Winner = PLAYER;
            }
            else
            {
                Winner = AIPLAYER;
            }
            EvaluateFinalScore();
        }

        private int EvaluatePlayScore(int currentPLayer) 
        {
            return EvaluateVerticalScore(currentPLayer) + EvvaluateHorizontalScore(currentPLayer) + EvaluateLeftDiagonalScore(currentPLayer) + EvaluateRightDiagonalScore(currentPLayer);
        }

        private int EvaluateVerticalScore(int currentPlayer)
        {
            int points = 0;

            for (int column = 0; column < BoardColumnSize; column++)
            {
                for (int row = 0; row < BoardRowSize; row++)
                {
                    int cellsLeft = BoardRowSize - row;
                    int pointsLeftToWin = 4 - points;
                    bool isWinPossible = cellsLeft >= pointsLeftToWin;

                    if (!isWinPossible)
                    {
                        return 0;
                    }

                    int currentValue = Board[row, column];

                    if (currentValue == 0)
                    {
                        break;
                    }

                    if (currentValue == currentPlayer)
                    {
                        points++;
                    }
                    else
                    {
                        points = 0;
                    }
                }
            }

            return points;
        }

        private int EvvaluateHorizontalScore(int currentPlayer)
        {
            int points = 0;

            for (int row = 0; row < BoardRowSize; row++)
            {
                for (int column = 0; column < BoardColumnSize - 3; column++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int currentValue = Board[row, column + i];

                        if (currentValue == currentPlayer)
                        {
                            points++;
                        } 
                        else if (currentValue != 0)
                        {
                            points = 0;
                            break;
                        }
                    }
                }
            }

            return points;
        }

        private int EvaluateRightDiagonalScore(int currentPlayer)
        {
            int points = 0;

            for (int row = 0; row < BoardRowSize - 3; row++)
            {
                for (int column = 0; column < BoardColumnSize - 3; column++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int currentValue = Board[row + i, column + i];

                        if (currentValue == currentPlayer)
                        {
                            points++;
                        }
                        else if (currentValue != 0)
                        {
                            points = 0;
                            break;
                        }
                    }
                }
            }

            return points;
        }

        private int EvaluateLeftDiagonalScore(int currentPlayer)
        {
            int points = 0;

            for (int row = 3; row < BoardRowSize; row++)
            {
                for (int column = 0; column <= 5 - 4; column++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int currentValue = Board[column + i,row - i];

                        if (currentValue == currentPlayer)
                        {
                            points++;
                        }
                        else if (currentValue != 0)
                        {
                            points = 0;
                            break;
                        }
                    }
                }
            }

            return points;
        }

        public List<int> GetAvailableColumns()
        {
            List<int> availableColumns = new List<int>();
            int lastRowIndex = BoardRowSize - 1;

            for (int column = 0; column < BoardColumnSize; column++)
            {
                if (Board[column, lastRowIndex] == 0)
                {
                    availableColumns.Add(column);
                }
            }

            return availableColumns;
        }

        public int[,] GetNewState(int col, int player)
        {
            CurrentPlayer = player;
            int[,] board = CloneBoard();
            DropPiece(board, col, CurrentPlayer);
            return board;
        }

        private int[,] CloneBoard()
        {
            int[,] cloneBoard = (int[,]) Board.Clone();
            return cloneBoard;
        }
    }
}
