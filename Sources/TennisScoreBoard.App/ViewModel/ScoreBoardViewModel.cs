using System;
using System.Reactive;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using TennisScoreBoard.EF;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    public class ScoreBoardViewModel : ViewModelBase
    {
        private IScoreBoardData m_currentData;
        private IMatchService m_matchService;
        private bool m_showScoreBoard;
        private TennisMatch m_currentMatch;
        private string m_firstPlayer;
        private string m_SecondPlayer;

        public bool ShowScoreBoard
        {
            get => m_showScoreBoard;
            set
            {
                m_showScoreBoard = value;
                RaisePropertyChanged();
            }
        }
        public IScoreBoardData CurrentData => m_currentData;
        public string FirstPlayer => m_firstPlayer;
        public string SecondPlayer => m_SecondPlayer;


        public ScoreBoardViewModel(IMatchService matchService, StartMatchViewModel startMatchViewModel, PlayerScoringViewModel playerScoringViewModel)
        {
            startMatchViewModel.NewMatchStarted.ObserveOnDispatcher().Subscribe(onNewMatchStarted);
            playerScoringViewModel.PlayerScored.ObserveOnDispatcher().Subscribe(onPlayerScored);
            m_matchService = matchService;

            ShowScoreBoard = false;
        }

        private void onPlayerScored(Unit obj)
        {
            refreshScoreData();
        }

        private void onNewMatchStarted(TennisMatch match)
        {
            ShowScoreBoard = true;
            m_currentMatch = match;
            setPlayersNames(match);
            refreshScoreData();
            
        }

        private void setPlayersNames(TennisMatch match)
        {
            m_firstPlayer = $"{match.FirstPlayer.FirstName} {match.FirstPlayer.LastName}";
            m_SecondPlayer = $"{match.SecondPlayer.FirstName} {match.SecondPlayer.LastName}";
            
            RaisePropertyChanged(()=> FirstPlayer);
            RaisePropertyChanged(()=> SecondPlayer);
        }

        private void refreshScoreData()
        {
            var data = m_matchService.GetScoreBoardData(m_currentMatch);
            if (data != null)
            {
                m_currentData = data;
                RaisePropertyChanged(() => CurrentData);
            }
            else
            {
                // todo: handle null
            }
        }

    }
}
