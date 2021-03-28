using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using TennisScoreBoard.App.Common;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    public class AddPlayerViewModel : ViewModelBase
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMatchService m_matchService;
        private string m_playerFirstName;
        private string m_playerLastName;
        private readonly ITennisMatchState m_matchState;


        public ICommand AddPlayerCommand => new RelayCommand(executeAddPlayerCommand, true);

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

        public AddPlayerViewModel(IMatchService matchService, ITennisMatchState matchState)
        {
            s_log.DebugFormat($"[AddPlayerViewModel]");
            m_matchService = matchService;
            m_matchState = matchState;
        }

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
                m_matchState.NotifyOnPlayerAdded();
            }
            s_log.DebugFormat($"[executeAddPlayerCommand] Add player succeeded: {succeeded}");
        }

        private void clearView()
        {
            PlayerFirstName = string.Empty;
            PlayerLastName = string.Empty;
        }

       
    }
}
