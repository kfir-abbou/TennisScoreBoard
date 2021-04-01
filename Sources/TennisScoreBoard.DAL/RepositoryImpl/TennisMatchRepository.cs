using System;
using System.Collections.Generic;
using System.Text;
using TennisScoreBoard.EF.Model;
using TennisScoreBoard.EF.Repository;

namespace TennisScoreBoard.EF.RepositoryImpl
{
    public class TennisMatchRepository : BaseRepository<TennisMatch>
    {
        public TennisMatchRepository(ScoreBoardContext context) : base(context)
        {

        }
    }
}
