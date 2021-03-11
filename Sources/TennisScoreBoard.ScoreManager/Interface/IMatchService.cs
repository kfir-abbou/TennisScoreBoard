using TennisScoreBoard.ScoreManager.Common;

namespace TennisScoreBoard.ScoreManager.Interface
{
    public interface IMatchService
    {
        bool StartMatch(int player1Id, int player2Id);
        bool UpdateGameResult(int gameId, PLAYER player);
    }
}
