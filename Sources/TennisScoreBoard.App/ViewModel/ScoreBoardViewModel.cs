using System;
using System.Reactive;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using TennisScoreBoard.App.Common;
using TennisScoreBoard.EF;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    public class ScoreBoardViewModel : ViewModelBase
    {
        private IScoreBoardData m_currentData;
        private readonly IMatchService m_matchService;
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


        public ScoreBoardViewModel(IMatchService matchService, ITennisMatchState matchState)
        {
            matchState.NewMatchStarted.ObserveOnDispatcher().Subscribe(onNewMatchStarted);
            matchState.PlayerScored.ObserveOnDispatcher().Subscribe(onPlayerScored);
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
                throw new NullReferenceException("[refreshScoreData] GetScoreBoardData returns null");
            }
        }

    }
}
