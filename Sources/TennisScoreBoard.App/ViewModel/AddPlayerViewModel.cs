using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    public class AddPlayerViewModel : ViewModelBase
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMatchService m_matchService;
        private string m_playerFirstName;
        private string m_playerLastName;
        private readonly ISubject<Unit> m_newPlayerAdded = new Subject<Unit>();

        public string PlayerFirstName
        {
            get => m_playerFirstName;
            set
            {
                m_playerFirstName = value;
                RaisePropertyChanged();
            }
        }

        public string PlayerLastName
        {
            get => m_playerLastName;
            set
            {
                m_playerLastName = value;
                RaisePropertyChanged();
            }
        }

        public IObservable<Unit> NewPlayerAdded => m_newPlayerAdded;
        public AddPlayerViewModel(IMatchService matchService)
        {
            s_log.DebugFormat($"[AddPlayerViewModel]");
            m_matchService = matchService;
        }

        public ICommand AddPlayerCommand => new RelayCommand(executeAddPlayerCommand, true);

        private void executeAddPlayerCommand()
        {
            s_log.DebugFormat($"[executeAddPlayerCommand]");
            if (PlayerFirstName == null || PlayerLastName == null)
            {
                return;
            }
            var succeeded = m_matchService.AddPlayer(PlayerFirstName, PlayerLastName);
            if (succeeded)
            {
                clearView();
                m_newPlayerAdded.OnNext(Unit.Default);
            }
            s_log.DebugFormat($"[executeAddPlayerCommand] Add player succeed: {succeeded}");
        }

        private void clearView()
        {
            PlayerFirstName = string.Empty;
            PlayerLastName = string.Empty;
            notifyData();
        }

        private void notifyData()
        {
            
        }
    }
}
