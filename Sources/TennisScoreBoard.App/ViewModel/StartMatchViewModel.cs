using System;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using TennisScoreBoard.EF;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    public class StartMatchViewModel :ViewModelBase
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMatchService m_matchService;
        // private TennisMatch m_currentMatch;
        private TennisPlayer m_p1SelectedItem;
        private TennisPlayer m_p2SelectedItem;
        private readonly AddPlayerViewModel m_addPlayerVm;
        private readonly ISubject<TennisMatch> m_newMatchStarted = new Subject<TennisMatch>();


        public IObservable<TennisMatch> NewMatchStarted => m_newMatchStarted;
        public StartMatchViewModel(IMatchService matchService, AddPlayerViewModel addPlayerViewModel)
        {
            s_log.DebugFormat($"[StartMatchViewModel]");
            m_matchService = matchService;
            m_addPlayerVm = addPlayerViewModel;
            m_addPlayerVm.NewPlayerAdded.ObserveOnDispatcher().Subscribe(onPlayerAdded);
        }

        private void onPlayerAdded(Unit obj)
        {
            RaisePropertyChanged(()=> PlayersItemsSource);
        }

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
            new ObservableCollection<TennisPlayer>(m_matchService.GetPlayers()); // todo: consider remove new statement

        public ICommand StartMatchCommand => new RelayCommand(executeStartMatchCommand, () => true);

        private void executeStartMatchCommand()
        {
            s_log.DebugFormat($"[executeStartMatchCommand]");

            if (validateData())
            {
                var Match = m_matchService.StartMatch(p1SelectedItem, p2SelectedItem);
                if (Match != null)
                {
                    m_newMatchStarted.OnNext(Match);
                    s_log.DebugFormat($"[executeStartMatchCommand] matchId: {Match.Id}");
                }
                else
                {
                    s_log.ErrorFormat($"[executeStartMatchCommand] Match creation failed.");
                }
            }
            
           
        }

        private bool validateData()
        {
            var msg = string.Empty;
            var isValid = false;
            
            if (p1SelectedItem == null)
            {
                msg = "Please choose first player";
            }
            else if (p2SelectedItem == null)
            {
                msg = "Please choose second player";
            }
            else if (p1SelectedItem == p2SelectedItem)
            {
                msg = "Cannot choose same player";
            }
            else
            {
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
