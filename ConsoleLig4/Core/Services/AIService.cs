using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleLig4.Core.Interfaces;
using ConsoleLig4.Core.Models;

namespace ConsoleLig4.Core.Services
{
    public class AIService : IAIService
    {
        public int NextMove { get; private set; }

        private int[,] currentBoard;

        private SimulatedGameService simulatedGameService;

        private const int TREEDEPTH = 5;                     // Depth of Tree to find bast path ;
        private const int BALANCEFACTORERRORVALUE = 10;      // value to mess up min max search ;
        private const int BALANCEFACTORERRORVPERCENTUAL = 7; // percentual chances to reduce max search;

        private const int PLAYER = 1;
        private const int AIPLAYER = 2;

        private TaskCompletionSource AIProcessing { get; set; }


        public async Task IsProcessing()
        {
            if (AIProcessing == null)
            {
                await Task.CompletedTask;
            }
            await AIProcessing.Task;
        }

        public void SetBoard(int[,] board)
        {
            AIProcessing = new TaskCompletionSource();
            // SET BOARD
            currentBoard = board;

            AIProcessing.SetResult();
        }

        public void SetPlayerMove(int position)
        {
            AIProcessing = new TaskCompletionSource();
            // SET PLAYER MOVE

            NextMove = MaximizeResult(currentBoard, 1).EvaluatedColumn+1;

            //NextMove = new Random().Next(1, 6);
            AIProcessing.SetResult();
        }

       //The maximizing Function of Min Max search (best for current player)
        MiniMaxResult MaximizeResult(int[,] currentBoard, int depth, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            simulatedGameService = new SimulatedGameService(currentBoard, AIPLAYER);
            if (depth > TREEDEPTH || simulatedGameService.isFinalStatePosition)
            {
                return new MiniMaxResult(-1, simulatedGameService.EvaluateFinalScore());
            }

            MiniMaxResult maximunResult = new MiniMaxResult(-1, int.MinValue );
            List<int> avaiableColumns = simulatedGameService.GetAvailableColumns();

            for (int i = 0; i < avaiableColumns.Count; i++)
            {

                int currentColumn = avaiableColumns[i];
                int[,] simulatedBoard = simulatedGameService.GetNewState(currentColumn, AIPLAYER);

                MiniMaxResult nextTurnMinimunResult = MinimizeResult(simulatedBoard, depth + 1, alpha, beta);
                int balanceFactor = new Random().Next(0,10);
                if (balanceFactor < BALANCEFACTORERRORVPERCENTUAL)
                {
                    nextTurnMinimunResult.EvaluatedScore += BALANCEFACTORERRORVALUE;
                }

                if (maximunResult.EvaluatedColumn == -1 || nextTurnMinimunResult.EvaluatedScore > maximunResult.EvaluatedScore)
                {
                    maximunResult.EvaluatedColumn = currentColumn;
                    maximunResult.EvaluatedScore = nextTurnMinimunResult.EvaluatedScore;
                    alpha = nextTurnMinimunResult.EvaluatedScore;
                }

                if (alpha >= beta)
                {
                    //Pruning
                    return maximunResult;
                }

                simulatedGameService.SetBoard(currentBoard);
            }

            return maximunResult;
        }

        //The minimazing Function of Min Max search (best for other player)
        MiniMaxResult MinimizeResult(int[,] currentBoard, int depth, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            simulatedGameService = new SimulatedGameService(currentBoard, PLAYER);

            if (depth > TREEDEPTH || simulatedGameService.isFinalStatePosition)
            {
                return new MiniMaxResult(-1, simulatedGameService.EvaluateFinalScore());
            }

            MiniMaxResult minimumResult = new MiniMaxResult(-1, int.MaxValue );
            List<int> avaiableColumns = simulatedGameService.GetAvailableColumns();

            for (var i = 0; i < avaiableColumns.Count; i++)
            {

                int currentColumn = avaiableColumns[i];
                int[,] newBoard = simulatedGameService.GetNewState(currentColumn, PLAYER);

                MiniMaxResult nextTurnMaximumResult = MaximizeResult(newBoard, depth + 1, alpha, beta);

                int balanceFactor = new Random().Next(0, 10);
                if (balanceFactor < BALANCEFACTORERRORVPERCENTUAL)
                {
                    nextTurnMaximumResult.EvaluatedScore -= BALANCEFACTORERRORVALUE;
                }

                if (minimumResult.EvaluatedColumn == -1 || nextTurnMaximumResult.EvaluatedScore < minimumResult.EvaluatedScore)
                {
                    minimumResult.EvaluatedColumn = currentColumn;
                    minimumResult.EvaluatedScore = nextTurnMaximumResult.EvaluatedScore;
                    beta = nextTurnMaximumResult.EvaluatedScore;
                }

                if (alpha >= beta)
                {
                    //Pruning
                    return minimumResult;
                }

                simulatedGameService.SetBoard(currentBoard);
            }

            return minimumResult;
        }
    }
}
