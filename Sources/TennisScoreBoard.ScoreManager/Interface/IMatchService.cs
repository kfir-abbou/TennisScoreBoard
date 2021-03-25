using System.Collections.Generic;
using TennisScoreBoard.EF;
using TennisScoreBoard.ScoreManager.Common;

namespace TennisScoreBoard.ScoreManager.Interface
{
    public interface IMatchService
    {
        bool AddPlayer(string first, string last);
        IList<TennisPlayer> GetPlayers();
        TennisMatch StartMatch(TennisPlayer player1, TennisPlayer player2);
        bool UpdateGameResult(TennisMatch match, PLAYER player);
        IScoreBoardData GetScoreBoardData(TennisMatch match);
    }
}
