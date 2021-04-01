using TennisScoreBoard.EF.Model;
using TennisScoreBoard.EF.Repository;

namespace TennisScoreBoard.EF.RepositoryImpl
{
    public class GameRepository : BaseRepository<Game>
    {
        private ScoreBoardContext m_context;

        public GameRepository(ScoreBoardContext context) : base(context)
        {
            m_context = context;
        }

    }
}
