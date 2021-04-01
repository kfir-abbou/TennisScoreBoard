using System;
using System.Collections.Generic;
using System.Text;
using TennisScoreBoard.EF.Model;
using TennisScoreBoard.EF.Repository;

namespace TennisScoreBoard.EF.RepositoryImpl
{
    public class TennisSetRepository : BaseRepository<TennisSet>
    {
        public TennisSetRepository(ScoreBoardContext context) : base(context)
        {

        }
    }
}
