using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TennisScoreBoard.EF;
using TennisScoreBoard.EF.Model;
using TennisScoreBoard.EF.Repository;
using TennisScoreBoard.ScoreManager.Common;
using TennisScoreBoard.ScoreManager.Implementation;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.Test
{
    [TestClass]
    public class MatchServiceTest
    {
        private readonly IMatchService m_matchService;
        private TennisMatch m_match;
        private Mock<TennisPlayer> m_p1;
        private Mock<TennisPlayer> m_p2;
        private readonly RepositoryManager m_repoMgr;

        public MatchServiceTest()
        {
            // var serializer = new ConfigLoader();
            // var config = serializer.LoadScoreboardConfig(Constants.CONFIG_FILE);
            // var optionsBuilder = new DbContextOptionsBuilder<ScoreBoardContext>();
            // optionsBuilder.UseSqlServer(@$"Server={config.ConnectionString};Database=TennisScoreDb_ForTest;User Id={config.UserId};Password={config.Password};");
            // optionsBuilder.UseSqlServer($@"Server={config.ConnectionString};Database=TennisScoreDb;Integrated Security=true;Trusted_Connection=True;");

            var context = new ScoreBoardContext();
            m_repoMgr = new RepositoryManager(context);
            m_matchService = new MatchService(m_repoMgr);

        }

        [TestInitialize]
        public void InitData()
        {
            m_p1 = new Mock<TennisPlayer>();
            m_p2 = new Mock<TennisPlayer>();

            m_match = m_matchService.StartMatch(m_p1.Object, m_p2.Object);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
        }

        [TestMethod]
        public void StartMatchTest()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            // m_match = m_matchService.StartMatch(m_p1.Object, p2.Object);
            Assert.AreNotEqual(null, m_match);

        }

        [TestMethod]
        public void StartMatchTest_CreateSingleTennisSet()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            Assert.AreEqual(1, m_match.Sets.Count);
        }

        [TestMethod]
        public void StartMatchTest_CreateSingleTennisGame()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            Assert.AreEqual(1, m_match.Sets.Last().Games.Count);
        }

        [TestMethod]
        public void UpdateGameResultTest_OneScoreMatchNotOver()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var isMatchOver = m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);

            Assert.AreEqual(false, isMatchOver);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "No tennis set initialize for this match")]
        public void UpdateGameResultTest_NoMatchSupposedToBeFound()
        {
            var match = new Mock<TennisMatch>();

            m_matchService.UpdateGameResult(match.Object, PLAYER.SECOND);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Match is null exception should have been thrown")]
        public void UpdateGameResultTest_matchIsNull()
        {
            m_matchService.UpdateGameResult(null, PLAYER.SECOND);
        }


        [TestMethod]
        public void UpdateGameResultTest_120ScoresBySinglePlayerMatchIsOver()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var isMatchOver = false;

            var totalServes = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                              Constants.NUMBER_OF_GAMES_TO_WIN_SET *
                              Constants.NUMBER_OF_SETS_TO_WIN_MATCH;


            for (int i = 0; i < totalServes; i++)
            {
                isMatchOver = m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            Assert.AreEqual(true, isMatchOver);
        }

        [TestMethod]
        public void UpdateGameResultTest_BothPlayersWinsSets_SecondPlayerWinsMatch_MatchIsOver()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var isMatchOver = false;

            var numberOfSets = 2;
            var numberOfScoreToWinSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                              Constants.NUMBER_OF_GAMES_TO_WIN_SET;


            for (int i = 0; i < numberOfScoreToWinSet * numberOfSets; i++)
            {
                isMatchOver = m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            numberOfSets = Constants.NUMBER_OF_SETS_TO_WIN_MATCH;

            for (int i = 0; i < numberOfScoreToWinSet * numberOfSets; i++)
            {
                isMatchOver = m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            Assert.AreEqual(true, isMatchOver);
        }

        [TestMethod]
        public void UpdateGameResultTest_IsMatchOver_bothPlayerScoresPoints()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var isMatchOver = false;

            for (var i = 0; i < Constants.NUMBER_OF_SCORES_TO_WIN_GAME - 2; i++)
            {
                isMatchOver = m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            for (var i = 0; i < Constants.NUMBER_OF_SCORES_TO_WIN_GAME; i++)
            {
                isMatchOver = m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            Assert.AreEqual(false, isMatchOver);
        }

        [TestMethod]
        public void UpdateGameResultTest_IsGameOver_bothPlayerScores_3_2_gameShouldNotOver()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);
            // var gameId = match.Sets.Last().Games.Last().Id;

            var numberOfScores = 3;

            for (var i = 0; i < numberOfScores; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            numberOfScores = 2;
            for (var i = 0; i < numberOfScores; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            var isGameOver = m_match.Sets.Last().Games.Last().Winner != null;
            Assert.AreEqual(false, isGameOver);
        }


        [TestMethod]
        public void UpdateGameResultTest_IsGameOver_bothPlayerScores_1_4_gameShouldBeOver()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var numberOfScores = 1;

            for (var i = 0; i < numberOfScores; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            numberOfScores = 4;
            for (var i = 0; i < numberOfScores; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            var games = m_match.Sets.Last().Games.ToArray();

            var isGameOver = games[^2].Winner != null;

            Assert.AreEqual(true, isGameOver);
        }

        [TestMethod]
        public void UpdateGameResultTest_IsSetOver_bothPlayerWinsGames_2_4_SetShouldNotBeOver()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var numberOfGames = 2;

            for (var i = 0; i < numberOfGames * Constants.NUMBER_OF_SCORES_TO_WIN_GAME; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            numberOfGames = 4;
            for (var i = 0; i < numberOfGames * Constants.NUMBER_OF_SCORES_TO_WIN_GAME; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            var sets = m_match.Sets.ToArray();

            var isSetOver = sets[^1].Winner != null;

            Assert.AreEqual(false, isSetOver);
        }

        [TestMethod]
        public void UpdateGameResultTest_OnceSetIsOver_NewSingleGameIsCreated()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            // var isSetOver = false;
            

            for (var i = 0; i < Constants.NUMBER_OF_GAMES_TO_WIN_SET * Constants.NUMBER_OF_SCORES_TO_WIN_GAME; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            var newSet = m_match.Sets.Last();
            var gamesCount = newSet.Games.Count;


            Assert.AreEqual(1, gamesCount);
        }

        [TestMethod]
        public void UpdateGameResultTest_OnceSetIsOver_NewSingleSetIsCreated()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            // var isSetOver = false;
            

            for (var i = 0; i < Constants.NUMBER_OF_GAMES_TO_WIN_SET * Constants.NUMBER_OF_SCORES_TO_WIN_GAME; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            var setsCount = m_match.Sets.Count;

            Assert.AreEqual(2, setsCount);
        }

        [TestMethod]
        public void UpdateGameResultTest_firsPlayerWinsSet()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var scoresToWinSingleSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                                       Constants.NUMBER_OF_GAMES_TO_WIN_SET;

            for (var i = 0; i < scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            var sets = m_match.Sets.ToArray();

            Assert.AreEqual(m_p1.Object, sets[^2].Winner);
        }


        [TestMethod]
        public void UpdateGameResultTest_SecondPlayerWinsSet()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var scoresToWinSingleSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                                       Constants.NUMBER_OF_GAMES_TO_WIN_SET;

            for (var i = 0; i < scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            var sets = m_match.Sets.ToArray();

            Assert.AreEqual(m_p2.Object, sets[^2].Winner);
        }

        [TestMethod]
        public void UpdateGameResultTest_SecondPlayerWinsSet_bothPlayersWinsGames()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var scoresToWinSingleSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                                       Constants.NUMBER_OF_GAMES_TO_WIN_SET;

            for (var i = 0; i < Constants.NUMBER_OF_SCORES_TO_WIN_GAME; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            for (var i = 0; i < scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            var sets = m_match.Sets.ToArray();

            Assert.AreEqual(m_p2.Object, sets[^2].Winner);
        }

        [TestMethod]
        public void UpdateGameResultTest_firsPlayerWinsSetAfterGetAndLooseAdvantage()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var scoresToWinSingleSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                                       Constants.NUMBER_OF_GAMES_TO_WIN_SET;
            // Both player score 40
            for (var i = 0; i < 3; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }
            for (var i = 0; i < 3; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            // First player wins Advantage
            for (var i = 0; i < scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
                }

                for (var k = 0; k < 2; k++)
                {
                    m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
                }
            }

            for (var i = 0; i < scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            var sets = m_match.Sets.ToArray();

            Assert.AreEqual(m_p1.Object, sets[^2].Winner);
        }

        [TestMethod]
        public void UpdateGameResultTest_IsMatchOver_Player2Wins_6_4_MatchIsOver()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var isMatchOver = false;

            var numberOfSets = 4;

            var scoresToWinSingleSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                                       Constants.NUMBER_OF_GAMES_TO_WIN_SET;


            for (var i = 0; i < numberOfSets * scoresToWinSingleSet; i++)
            {
                isMatchOver = m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            numberOfSets = 6;
            for (var i = 0; i < numberOfSets * scoresToWinSingleSet; i++)
            {
                isMatchOver = m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            Assert.AreEqual(true, isMatchOver);
        }

        [TestMethod]
        public void UpdateGameResultTest_getCurrentScores_scoresByPlayerKeyIs_0()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);

            var P1Points = m_matchService.GetScoreBoardData(m_match).P1Points;

            Assert.AreEqual(POINTS.Fifteen, P1Points);
        }

        [TestMethod]
        public void UpdateGameResultTest_getCurrentScores_TestPlayer1AdvantageCase()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            for (var i = 0; i < 3; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }
            for (var i = 0; i < 4; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            var P1Points = m_matchService.GetScoreBoardData(m_match).P1Points;

            Assert.AreEqual(POINTS.Advantage, P1Points);
        }

        [TestMethod]
        public void UpdateGameResultTest_getCurrentScores_TestPlayer2AdvantageCase()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            for (var i = 0; i < 3; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }
            for (var i = 0; i < 4; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }

            var P2Points = m_matchService.GetScoreBoardData(m_match).P2Points;

            Assert.AreEqual(POINTS.Advantage, P2Points);
        }


        [TestMethod]
        public void UpdateGameResultTest_getCurrentScores_TestPlayerDeuceCase()
        {
            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            for (var i = 0; i < 3; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }
            for (var i = 0; i < 4; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }
            m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);


            var P1Points = m_matchService.GetScoreBoardData(m_match).P1Points;

            Assert.AreEqual(POINTS.Fourty, P1Points);
        }


        [TestMethod]
        public void AddPlayerTest_ValidInput()
        {
            var playersCount = m_matchService.GetPlayers().Count;
            var res = m_matchService.AddPlayer(playersCount.ToString(), playersCount.ToString());

            Assert.AreEqual(true, res);
        }

        [TestMethod]
        public void AddPlayerTest_FirstNameEmpty()
        {
            var res = m_matchService.AddPlayer(string.Empty, "last");

            Assert.AreEqual(false, res);
        }

        [TestMethod]
        public void AddPlayerTest_LastNameEmpty()
        {
            var res = m_matchService.AddPlayer("First", string.Empty);

            Assert.AreEqual(false, res);
        }

        [TestMethod]
        public void GetPlayersTest()
        {
            var players = m_matchService.GetPlayers();

            Assert.IsInstanceOfType(players, typeof(IList<TennisPlayer>));
        }

        [TestMethod]
        public void GetScoreBoardDataTest_ValidMatch()
        {

            // var p1 = new Mock<TennisPlayer>();
            // var p2 = new Mock<TennisPlayer>();
            //
            // var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var numberOfSets = 4;

            var scoresToWinSingleSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                                       Constants.NUMBER_OF_GAMES_TO_WIN_SET;


            for (var i = 0; i < numberOfSets * scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.FIRST);
            }

            for (var i = 0; i < scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(m_match, PLAYER.SECOND);
            }


            var res = m_matchService.GetScoreBoardData(m_match);

            Assert.IsTrue(res != null);
        }

        [TestMethod]
        public void GetScoreBoardDataTest_MatchIsNull()
        {
            var res = m_matchService.GetScoreBoardData(null);
            if (res == null)
            {
                //
            }
            Assert.AreEqual(null, res);
        }

        [TestMethod]
        public void AddPlayerTest()
        {
            // m_matchService.GetPlayers().Count;

            var res = m_matchService.AddPlayer("1", "2");

            Assert.AreEqual(true, res);

        }
    }
}
