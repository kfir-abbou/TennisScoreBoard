using System;
using System.Collections.Generic;
using System.Text;
using TennisScoreBoard.ScoreManager.Common;

namespace TennisScoreBoard.ScoreManager.Interface
{
    public interface IScoreBoardData
    {
        POINTS P1Points { get; }
        POINTS P2Points { get; }
        int P1SetWins { get; }
        int P2SetWins { get; }
        int P1GameWins { get; }
        int P2GameWins { get; }
        bool IsMatchOver { get; }
    }
}
