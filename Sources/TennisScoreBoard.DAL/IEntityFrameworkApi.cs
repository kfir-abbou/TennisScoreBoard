using System;
using System.Collections.Generic;
using System.Text;

namespace TennisScoreBoard.EF
{
    public interface IEntityFrameworkApi
    {
        void AddPlayer(string first, string last);

        TennisMatch GetMatchById(int matchId);
    }
}
