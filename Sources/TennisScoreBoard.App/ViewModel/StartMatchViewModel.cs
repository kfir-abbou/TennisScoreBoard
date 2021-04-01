using System;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using TennisScoreBoard.App.Common;
using TennisScoreBoard.EF;
using TennisScoreBoard.EF.Model;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    public class StartMatchViewModel : ViewModelBase
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMatchService m_matchService;
        private TennisPlayer m_p1SelectedItem;
        private TennisPlayer m_p2SelectedItem;
        private TennisMatch m_currentMatch;
        private readonly ITennisMatchState m_matchState;

        public bool IsViewEnabled => m_currentMatch == null || m_currentMatch.Winner != null;
        public TennisPlayer p1SelectedItem
        {
            get => m_p1SelectedItem;
            set
            {
                m_p1SelectedItem = value;
                RaisePropertyChanged();
            }
        }
        public TennisPlayer p2SelectedItem
        {
            get => m_p2SelectedItem;
            set
            {
                m_p2SelectedItem = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<TennisPlayer> PlayersItemsSource =>
            new ObservableCollection<TennisPlayer>(m_matchService.GetPlayers()); 
        public ICommand StartMatchCommand => new RelayCommand(executeStartMatchCommand, () => true);

        #region CTOR
        public StartMatchViewModel(IMatchService matchService, ITennisMatchState matchState)
        {
            s_log.DebugFormat($"[StartMatchViewModel]");
            m_matchService = matchService;
            m_matchState = matchState;
            m_matchState.NewPlayerAdded.ObserveOnDispatcher().Subscribe(onPlayerAdded);
            m_matchState.MatchOver.ObserveOnDispatcher().Subscribe(onMatchOver);
        }

        private void onMatchOver(Unit obj)
        {
            RaisePropertyChanged(()=> IsViewEnabled);
        }

        #endregion

        private void onPlayerAdded(Unit obj)
        {
            s_log.DebugFormat($"[onPlayerAdded]");
            RaisePropertyChanged(() => PlayersItemsSource);
        }
        private void executeStartMatchCommand()
        {
            s_log.DebugFormat($"[executeStartMatchCommand]");

            if (validateData())
            {
                m_currentMatch = m_matchService.StartMatch(p1SelectedItem, p2SelectedItem);
                if (m_currentMatch != null)
                {
                    m_matchState.NotifyOnMatchStarted(m_currentMatch);
                    s_log.DebugFormat($"[executeStartMatchCommand] matchId: {m_currentMatch.Id}");
                }
                else
                {
                    s_log.ErrorFormat($"[executeStartMatchCommand] Match creation failed.");
                }
            }
            RaisePropertyChanged(() => IsViewEnabled);
        }

        private bool validateData()
        {
            var msg = string.Empty;
            var isValid = false;

            if (p1SelectedItem == null)
            {
                s_log.DebugFormat($"[validateData] 'p1SelectedItem' is null");

                msg = "Please choose first player";
            }
            else if (p2SelectedItem == null)
            {
                s_log.DebugFormat($"[validateData] 'p2SelectedItem' is null");
                msg = "Please choose second player";
            }
            else if (p1SelectedItem == p2SelectedItem)
            {
                s_log.DebugFormat($"[validateData] 'p1SelectedItem' == 'p2SelectedItem'");
                msg = "Cannot choose same player";
            }
            else
            {
                s_log.DebugFormat($"[validateData] data is valid");

                isValid = true;
            }

            if (!isValid)
            {
                MessageBox.Show(msg, "Players data not valid", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return isValid;
        }
    }
}
