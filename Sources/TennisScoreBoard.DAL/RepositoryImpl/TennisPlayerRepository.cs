using System;
using System.Collections.Generic;
using System.Text;
using TennisScoreBoard.EF.Model;
using TennisScoreBoard.EF.Repository;

namespace TennisScoreBoard.EF.RepositoryImpl
{
    public class TennisPlayerRepository : BaseRepository<TennisPlayer>
    {
        public TennisPlayerRepository(ScoreBoardContext context) : base(context)
        {

        }
    }
}
