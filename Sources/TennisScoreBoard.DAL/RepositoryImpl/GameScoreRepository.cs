using System;
using System.Collections.Generic;
using System.Text;
using TennisScoreBoard.EF.Model;
using TennisScoreBoard.EF.Repository;

namespace TennisScoreBoard.EF.RepositoryImpl
{
    public class GameScoreRepository : BaseRepository<GameScores>
    {
        public GameScoreRepository(ScoreBoardContext context) : base(context)
        {
            
        }
    }
}
