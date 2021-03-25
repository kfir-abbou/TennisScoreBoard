﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TennisScoreBoard.EF;
using TennisScoreBoard.ScoreManager.Common;
using TennisScoreBoard.ScoreManager.Implementation;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.Test
{
    [TestClass]
    public class ScoreBoardDataTest
    {

        private Mock<TennisMatch> m_matchMock;
        private Mock<TennisPlayer> m_p1;
        private Mock<TennisPlayer> m_p2;
        private IMatchService m_matchService;

        public ScoreBoardDataTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScoreBoardContext>();
            // TODO: kfir -> extract connection string to config
            optionsBuilder.UseSqlServer(@"Server=L-P-KFIRABB-WWN\SQLEXPRESS;Database=TennisScoreDb_ForTest;Trusted_Connection=True;");
            var context = new ScoreBoardContext(optionsBuilder.Options);

            m_matchService = new MatchService(context);
        }
        [TestInitialize]
        public void InitScoreBoardTest()
        {
            m_matchMock = new Mock<TennisMatch>();
            m_p1 = new Mock<TennisPlayer>();
            m_p2 = new Mock<TennisPlayer>();
        }

        [TestMethod]
        public void UpdateGameResultTest_TestP1GameWins()
        {
            var match = m_matchService.StartMatch(m_p1.Object, m_p2.Object);

            var numberOfGames = 3;
            
            for (int i = 0; i < numberOfGames * Constants.NUMBER_OF_SCORES_TO_WIN_GAME; i++)
            {
                m_matchService.UpdateGameResult(match, PLAYER.FIRST);
            }

            var p1GameWins = m_matchService.GetScoreBoardData(match).P1GameWins;
            Assert.AreEqual(numberOfGames, p1GameWins);
        }

        [TestMethod]
        public void UpdateGameResultTest_TestP2GameWins()
        {
            var match = m_matchService.StartMatch(m_p1.Object, m_p2.Object);

            var numberOfGames = 3;

            for (int i = 0; i < numberOfGames * Constants.NUMBER_OF_SCORES_TO_WIN_GAME; i++)
            {
                m_matchService.UpdateGameResult(match, PLAYER.SECOND);
            }

            var p2GameWins = m_matchService.GetScoreBoardData(match).P2GameWins;
            Assert.AreEqual(numberOfGames, p2GameWins);
        }

        [TestMethod]
        public void UpdateGameResultTest_TestP1SetWins()
        {
            var match = m_matchService.StartMatch(m_p1.Object, m_p2.Object);

            var numberOfSets = 4;

            var scoresToWinSingleSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                                       Constants.NUMBER_OF_GAMES_TO_WIN_SET;


            for (var i = 0; i < numberOfSets * scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(match, PLAYER.FIRST);
            }

            var p1SetWins = m_matchService.GetScoreBoardData(match).P1SetWins;

            Assert.AreEqual(numberOfSets, p1SetWins);
        }

        [TestMethod]
        public void UpdateGameResultTest_TestP2SetWins()
        {
            var match = m_matchService.StartMatch(m_p1.Object, m_p2.Object);

            var numberOfSets = 4;

            var scoresToWinSingleSet = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                                       Constants.NUMBER_OF_GAMES_TO_WIN_SET;


            for (var i = 0; i < numberOfSets * scoresToWinSingleSet; i++)
            {
                m_matchService.UpdateGameResult(match, PLAYER.SECOND);
            }

            var p2SetWins = m_matchService.GetScoreBoardData(match).P2SetWins;

            Assert.AreEqual(numberOfSets, p2SetWins);
        }

        [TestMethod]
        public void UpdateGameResultTest_120ScoresBySinglePlayerMatchIsOver()
        {
            var p1 = new Mock<TennisPlayer>();
            var p2 = new Mock<TennisPlayer>();

            var match = m_matchService.StartMatch(p1.Object, p2.Object);

            var isMatchOver = false;

            var totalServes = Constants.NUMBER_OF_SCORES_TO_WIN_GAME *
                              Constants.NUMBER_OF_GAMES_TO_WIN_SET *
                              Constants.NUMBER_OF_SETS_TO_WIN_MATCH;


            for (int i = 0; i < totalServes; i++)
            {
                m_matchService.UpdateGameResult(match, PLAYER.SECOND);
            }

            isMatchOver = m_matchService.GetScoreBoardData(match).IsMatchOver;

            Assert.AreEqual(true, isMatchOver);
        }
    }
    
   
}