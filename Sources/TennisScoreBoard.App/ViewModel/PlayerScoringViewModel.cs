using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using TennisScoreBoard.App.Common;
using TennisScoreBoard.EF;
using TennisScoreBoard.ScoreManager.Common;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    public class PlayerScoringViewModel : ViewModelBase
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMatchService m_matchService;
        private TennisMatch m_currentMatch;
        private string m_matchOverText;
        private bool m_isViewEnabled;
        private readonly ITennisMatchState m_matchState;


        public ICommand PlayerScoredCommand => new RelayCommand<PLAYER>(executePlayerScoredCommand, player => true);
        public string MatchOverText
        {
            get => m_matchOverText;
            set
            {
                m_matchOverText = value;
                RaisePropertyChanged();
            }
        }

        public bool IsViewEnabled
        {
            get => m_isViewEnabled;
            set
            {
                m_isViewEnabled = value;
                RaisePropertyChanged();
            }
        }

        #region CTOR
        public PlayerScoringViewModel(IMatchService matchService, ITennisMatchState matchState)
        {
            s_log.DebugFormat($"[PlayerScoringViewModel]");
            m_matchService = matchService;
            m_matchState = matchState;
            m_matchState.NewMatchStarted.ObserveOnDispatcher().Subscribe(onNewMatchStarted);
        }

        #endregion


        private void onNewMatchStarted(TennisMatch match)
        {
            s_log.DebugFormat($"[onNewMatchStarted] match: {match.Id}");

            m_currentMatch = match;
            IsViewEnabled = true;
            MatchOverText = string.Empty;
        }

        private void executePlayerScoredCommand(PLAYER player)
        {
            s_log.DebugFormat($"[executePlayerScoredCommand] player: {player}");
            var isMatchOver = m_matchService.UpdateGameResult(m_currentMatch, player);

            IsViewEnabled = !isMatchOver;
            m_matchState.NotifyOnPlayerScored();

            if (isMatchOver)
            {
                MessageBox.Show(
                    $"Match is over: '{m_currentMatch.Winner.FirstName} {m_currentMatch.Winner.LastName}' is the winner");
                m_matchState.NotifyOnMatchOver();
                MatchOverText = "This match is over, please start a new match.";
            }
        }

    }
}
