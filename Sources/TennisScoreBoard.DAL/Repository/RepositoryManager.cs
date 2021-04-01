using System;
using System.Collections.Generic;
using System.Text;
using TennisScoreBoard.EF.Model;
using TennisScoreBoard.EF.RepositoryImpl;

namespace TennisScoreBoard.EF.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ScoreBoardContext m_context;

        public RepositoryManager(ScoreBoardContext context)
        {
            GameRepository = new GameRepository(context);
            GameScoreRepository = new GameScoreRepository(context);
            TennisMatchRepository = new TennisMatchRepository(context);
            TennisPlayerRepository = new TennisPlayerRepository(context);
            TennisSetRepository = new TennisSetRepository(context);
            m_context = context;
        }

        public IRepository<Game> GameRepository { get; }
        public IRepository<GameScores> GameScoreRepository { get; }
        public IRepository<TennisMatch> TennisMatchRepository { get; }
        public IRepository<TennisPlayer> TennisPlayerRepository { get; }
        public IRepository<TennisSet> TennisSetRepository { get; }

        public void SaveData()
        {
            m_context.SaveData();
        }
    }

    public interface IRepositoryManager
    {
        IRepository<Game> GameRepository { get; }
        IRepository<GameScores> GameScoreRepository { get; }
        IRepository<TennisMatch> TennisMatchRepository { get; }
        IRepository<TennisPlayer> TennisPlayerRepository { get; }
        IRepository<TennisSet> TennisSetRepository { get; }
        void SaveData();
    }
}
