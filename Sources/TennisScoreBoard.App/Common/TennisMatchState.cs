using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Reflection;
using log4net;
using TennisScoreBoard.EF;

namespace TennisScoreBoard.App.Common
{
    public class TennisMatchState : ITennisMatchState
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISubject<TennisMatch> m_newMatchStarted = new Subject<TennisMatch>();
        private readonly ISubject<Unit> m_newPlayerAdded = new Subject<Unit>();
        private readonly ISubject<Unit> m_playerScored = new Subject<Unit>();
        private readonly ISubject<Unit> m_matchOver = new Subject<Unit>();

        public IObservable<TennisMatch> NewMatchStarted => m_newMatchStarted;
        public IObservable<Unit> NewPlayerAdded => m_newPlayerAdded;
        public IObservable<Unit> PlayerScored => m_playerScored;
        public IObservable<Unit> MatchOver => m_matchOver;
        public void NotifyOnMatchStarted(TennisMatch match)
        {
            s_log.DebugFormat("[NotifyOnMatchStarted]");
            m_newMatchStarted.OnNext(match);
        }

        public void NotifyOnPlayerAdded()
        {
            s_log.DebugFormat("[NotifyOnPlayerAdded]");
           m_newPlayerAdded.OnNext(Unit.Default);
        }

        public void NotifyOnPlayerScored()
        {
            s_log.DebugFormat("[NotifyOnPlayerScored]");
           m_playerScored.OnNext(Unit.Default);
        }

        public void NotifyOnMatchOver()
        {
            s_log.DebugFormat("[NotifyOnMatchOver]");
            m_matchOver.OnNext(Unit.Default);
        }
    }

    public interface ITennisMatchState
    {
        IObservable<TennisMatch> NewMatchStarted { get; }
        IObservable<Unit> NewPlayerAdded { get; }
        IObservable<Unit> PlayerScored { get; }
        IObservable<Unit> MatchOver { get; }

        void NotifyOnMatchStarted(TennisMatch match);
        void NotifyOnPlayerAdded();
        void NotifyOnPlayerScored();
        void NotifyOnMatchOver();
    }
}
