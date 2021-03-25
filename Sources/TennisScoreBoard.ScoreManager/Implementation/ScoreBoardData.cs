using TennisScoreBoard.ScoreManager.Common;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.ScoreManager.Implementation
{
    public class ScoreBoardData : IScoreBoardData
    {
        public POINTS P1Points { get; }
        public POINTS P2Points { get; }
        public int P1SetWins { get; }
        public int P2SetWins { get; }
        public int P1GameWins { get; }
        public int P2GameWins { get; }
        public bool IsMatchOver { get; }

        public ScoreBoardData(int p1GameWins, int p2GameWins, int p1SetWins, int p2SetWins, POINTS p1Points,
            POINTS p2Points, bool isMatchOver)
        {
            P1Points = p1Points;
            P2Points = p2Points;
            P1GameWins = p1GameWins;
            P2GameWins = p2GameWins;
            P1SetWins = p1SetWins;
            P2SetWins = p2SetWins;
            IsMatchOver = isMatchOver;
        }


    }
}
