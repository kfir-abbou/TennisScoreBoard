using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using log4net;
using TennisScoreBoard.EF;
using TennisScoreBoard.EF.Model;
using TennisScoreBoard.EF.Repository;
using TennisScoreBoard.ScoreManager.Common;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.ScoreManager.Implementation
{
    public class MatchService : IMatchService
    {
        // private readonly IScoreBoardContext m_context;
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IRepositoryManager m_repositoryManager;

        // public MatchService(IScoreBoardContext context)
        public MatchService(IRepositoryManager repositoryManager)
        {
            s_log.DebugFormat($"[MatchService]");
            // m_context = context;
            m_repositoryManager = repositoryManager;
        }

        public bool AddPlayer(string first, string last)
        {
            // validate
            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last))
            {
                return false;
            }

            if (m_repositoryManager.TennisPlayerRepository.GetAll().Any(p => p.FirstName == first && p.LastName == last))
            {
                return true;
            }

            m_repositoryManager.TennisPlayerRepository.Add(new TennisPlayer
            {
                FirstName = first,
                LastName = last
            });


            m_repositoryManager.SaveData();
            return true;
        }

        /// <summary>
        /// Set new match with players data
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>matchData</returns>
        public TennisMatch StartMatch(TennisPlayer p1, TennisPlayer p2)
        {

            var match = new TennisMatch
            {
                FirstPlayer = p1,
                SecondPlayer = p2,
                Sets = new List<TennisSet>
                {
                    new TennisSet()
                }
            };

            var set = match.Sets.OrderBy(s => s.Id).Last();
            var game = set.Games.Last();

            m_repositoryManager.GameRepository.Add(game);
            m_repositoryManager.TennisSetRepository.Add(set);
            m_repositoryManager.TennisMatchRepository.Add(match);
            m_repositoryManager.SaveData();

            var matchData = m_repositoryManager.TennisMatchRepository.GetAll().OrderBy(m => m.Id).Last();

            return matchData;
        }

        public bool UpdateGameResult(TennisMatch match, PLAYER player)
        {
            if (match == null)
            {
                throw new ArgumentNullException("Match is null");
            }
            var currentGame = getCurrentGame(match);
            
            m_repositoryManager.GameScoreRepository.Add(new GameScores
            {
                Game = (Game)currentGame,
                ScoringPlayer = (int)player
            });

            m_repositoryManager.SaveData();
            var isMatchOver = CheckMatchStatus(match);
            return isMatchOver;
        }

        private Game getCurrentGame(TennisMatch match)
        {
            if (match.Sets == null)
            {
                throw new NullReferenceException("No tennis set initialize for this match");
            }
            return match.Sets.OrderBy(s => s.Id)
                .LastOrDefault().Games.OrderBy(g => g.Id)
                .LastOrDefault();
        }

        public IScoreBoardData GetScoreBoardData(TennisMatch match)
        {
            if (match == null)
            {
                return null;
            }

            var P1SetWins = match.Sets.Count(s => s.Winner == match.FirstPlayer);
            var P2SetWins = match.Sets.Count(s => s.Winner == match.SecondPlayer);
            var P1GameWins = match.Sets.Last().Games.Count(g => g.Winner == match.FirstPlayer);
            var P2GameWins = match.Sets.Last().Games.Count(g => g.Winner == match.SecondPlayer);

            var gameScore = getGamescoresDataByGanmeId(match.Sets.Last().Games.Last().Id);

            getCurrentScores(gameScore, out var p1Scores, out var p2Scores);

            var scoresData = new ScoreBoardData(P1GameWins, P2GameWins, P1SetWins, P2SetWins,
                (POINTS)p1Scores, (POINTS)p2Scores,
                match.Winner != null);

            return scoresData;
        }

        private void getCurrentScores(IEnumerable<GameScores> gameScore, out int p1Scores, out int p2Scores)
        {
            p1Scores = 0;
            p2Scores = 0;
            var scoresByPlayer = gameScore.GroupBy(gs => gs.ScoringPlayer);
            switch (scoresByPlayer.Count())
            {
                case 0:
                    break; // no changes  all 0
                case 1:
                    if (scoresByPlayer.First().Key == 0) // First player
                    {
                        p1Scores = gameScore.Count();
                    }
                    else
                    {
                        p2Scores = gameScore.Count();
                    }
                    break;
                case 2:
                    p1Scores = scoresByPlayer.First().Count();
                    p2Scores = scoresByPlayer.Last().Count();

                    if (p1Scores + p2Scores > 6) // Ad
                    {
                        switch (Math.Abs(p1Scores - p2Scores))
                        {
                            case 1 when p1Scores > p2Scores:
                                p1Scores = (int)POINTS.Advantage;
                                p2Scores = (int)POINTS.Fourty;
                                break;
                            case 1:
                                p2Scores = (int)POINTS.Advantage;
                                p1Scores = (int)POINTS.Fourty;
                                break;
                            case 0:
                                p1Scores = (int)POINTS.Fourty;
                                p2Scores = (int)POINTS.Fourty;
                                break;
                        }
                    }
                    break;
            }
        }

        public IList<TennisPlayer> GetPlayers()
        {
            return m_repositoryManager.TennisPlayerRepository.GetAll().ToList();
        }

        private bool CheckMatchStatus(TennisMatch match)
        {

            var currentGameId = match
                .Sets.OrderBy(s => s.Id).Last()
                .Games.OrderBy(g => g.Id).Last().Id;

            var gameOver = isGameOver(currentGameId);


            if (gameOver)
            {
                setGameWinner(currentGameId, match);
                var tennisSet = match.Sets.ToList().Last();
                var setOver = isSetOver(tennisSet);
                if (setOver)
                {
                    setTennisSetWinner(tennisSet, match);

                    match.IsOver = isMatchOver(match);

                    if (match.IsOver)
                    {
                        setMatchWinner(match);
                    }
                    else
                    {
                        var set = new TennisSet();
                        var game = set.Games.Last();
                        match.Sets.Add(set);

                        m_repositoryManager.TennisSetRepository.Add(set);
                        m_repositoryManager.GameRepository.Add(game);
                    }
                }
                else
                {
                    // Start new game
                    var game = createNewGame();
                    tennisSet.Games.Add(game);
                    m_repositoryManager.GameRepository.Add(game);
                }
            }
            m_repositoryManager.SaveData();
            return match.IsOver;
        }

        private void setMatchWinner(TennisMatch match)
        {
            var setWins = match.Sets.Where(s => s.Winner != null).GroupBy(s => s.Winner);

            switch (setWins.Count())
            {
                case 1:
                    match.Winner = setWins.First().Key; // KEY IS TennisPlayer
                    break;
                case 2:
                    match.Winner = setWins.First().Count() > setWins.Last().Count() ? match.FirstPlayer : match.SecondPlayer;
                    break;
            }
        }

        private Game createNewGame()
        {
            return new Game();
        }

        private void setTennisSetWinner(TennisSet tennisSet, TennisMatch match)
        {
            var gameWinsByPlayer = tennisSet.Games.Where(g => g.Winner != null).GroupBy(g => g.Winner);

            switch (gameWinsByPlayer.Count())
            {
                case 1:
                    tennisSet.Winner = gameWinsByPlayer.First().Key;
                    break;
                case 2:
                    tennisSet.Winner = gameWinsByPlayer.First().Count() > gameWinsByPlayer.Last().Count()
                        ? match.FirstPlayer
                        : match.SecondPlayer;
                    break;
            }

        }

        private void setGameWinner(int currentGameId, TennisMatch match)
        {
            var game = m_repositoryManager.GameRepository.Get(currentGameId);

            var gameScores = getGamescoresDataByGanmeId(currentGameId);

            var p1Wins = gameScores.Count(gs => gs.ScoringPlayer == (int)PLAYER.FIRST);
            var p2Wins = gameScores.Count(gs => gs.ScoringPlayer == (int)PLAYER.SECOND);

            var winner = p1Wins > p2Wins ?
                match.FirstPlayer :
                match.SecondPlayer;
            // game.SetGameWinner(winner);
            game.Winner = winner;
        }

        private IEnumerable<GameScores> getGamescoresDataByGanmeId(int currentGameId)
        {
            var gameScores = m_repositoryManager.GameScoreRepository.Find(gs => gs.Game.Id == currentGameId)
                .OrderBy(gs => gs.ScoringPlayer);

            return gameScores;
        }

        private bool isGameOver(int gameId)
        {
            var retVal = false;

            var gameScores = m_repositoryManager.GameScoreRepository.Find(gs => gs.Game.Id == gameId)
                .AsEnumerable()
                .GroupBy(gs => gs.ScoringPlayer);

            int p1Scores;
            int p2Scores;
            var gamesCount = gameScores.Count();
            switch (gamesCount)
            {
                case 1:
                    retVal = gameScores.First().Count() == 4;
                    break;
                case 2:
                    p1Scores = gameScores.First().Count();
                    p2Scores = gameScores.Last().Count();
                    retVal = Math.Abs(p1Scores - p2Scores) > 1 &&
                             (p1Scores >= Constants.NUMBER_OF_SCORES_TO_WIN_GAME ||
                              p2Scores >= Constants.NUMBER_OF_SCORES_TO_WIN_GAME);
                    break;
            }

            return retVal;
        }

        private bool isSetOver(TennisSet set)
        {
            var gameWins = set.Games.Where(g => g.Winner != null).GroupBy(g => g.Winner).ToList();
            int p1Wins;
            int p2Wins;

            switch (gameWins.Count)
            {
                case 1:
                    return gameWins.First().Count() == Constants.NUMBER_OF_GAMES_TO_WIN_SET;
                case 2:
                default:
                    p1Wins = gameWins.First().Count();
                    p2Wins = gameWins.Last().Count();

                    return Math.Abs(p1Wins - p2Wins) > 1 &&
                           (p1Wins >= Constants.NUMBER_OF_GAMES_TO_WIN_SET || p2Wins >= Constants.NUMBER_OF_GAMES_TO_WIN_SET);
            }
        }

        private bool isMatchOver(TennisMatch match)
        {
            var setWinsByPlayer = match.Sets.Where(s => s.Winner != null).GroupBy(s => s.Winner);

            int p1Wins;
            int p2Wins;

            switch (setWinsByPlayer.Count())
            {
                case 1:
                    return setWinsByPlayer.First().Count() == Constants.NUMBER_OF_SETS_TO_WIN_MATCH;
                case 2:
                default:
                    p1Wins = setWinsByPlayer.First().Count();
                    p2Wins = setWinsByPlayer.Last().Count();
                    return Math.Abs(p1Wins - p2Wins) > 1 &&
                           (p1Wins >= Constants.NUMBER_OF_SETS_TO_WIN_MATCH ||
                            p2Wins >= Constants.NUMBER_OF_SETS_TO_WIN_MATCH);
            }
        }

    }
}
