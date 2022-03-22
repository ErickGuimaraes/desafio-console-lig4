using ConsoleLig4.Core.Interfaces;

namespace ConsoleLig4.Core.Models
{
    class MiniMaxResult : IMiniMaxResult
    {
        public int EvaluatedColumn { get; set; }
        public int EvaluatedScore { get; set; }

        public MiniMaxResult(int evaluatedColumn, int evaluatedScore)
        {
            EvaluatedColumn = evaluatedColumn;
            EvaluatedScore = evaluatedScore;
        }
    }
}
