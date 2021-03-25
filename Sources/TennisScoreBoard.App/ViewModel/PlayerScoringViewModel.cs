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
        private readonly StartMatchViewModel m_startMatchVm;
        private readonly ISubject<Unit> m_playerScored = new Subject<Unit>();
        
        public ICommand PlayerScoredCommand => new RelayCommand<PLAYER>(executePlayerScoredCommand, player => true);
        public IObservable<Unit> PlayerScored => m_playerScored;

        public PlayerScoringViewModel(IMatchService matchService, StartMatchViewModel startMatchViewModel)
        {
            s_log.DebugFormat($"[PlayerScoringViewModel]");
            m_matchService = matchService;
            m_startMatchVm = startMatchViewModel;
            m_startMatchVm.NewMatchStarted.ObserveOnDispatcher().Subscribe(onNewMatchStarted);
        }

        private void onNewMatchStarted(TennisMatch match)
        {
            m_currentMatch = match;
        }

        private void executePlayerScoredCommand(PLAYER player)
        {
            s_log.DebugFormat($"[executePlayerScoredCommand] player: {player}");

            var isMatchOver = m_matchService.UpdateGameResult(m_currentMatch, player);

            if (isMatchOver)
            {
                // todo handle match over
                MessageBox.Show(
                    $"Match is over -> {m_currentMatch.Winner.FirstName} {m_currentMatch.Winner.LastName} is the winner");
            }
            else
            {
                m_playerScored.OnNext(Unit.Default);
            }
        }


    }
}
