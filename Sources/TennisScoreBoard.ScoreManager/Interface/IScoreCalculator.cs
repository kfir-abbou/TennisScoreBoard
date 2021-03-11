using System.Buffers.Binary;

namespace TennisScoreBoard.ScoreManager.Interface
{
    public interface IScoreCalculator
    {
        bool CalculateScore(int match);
        bool GetScoreBoardData(int match);
    }
}
