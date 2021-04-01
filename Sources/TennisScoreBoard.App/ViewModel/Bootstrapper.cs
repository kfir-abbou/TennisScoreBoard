using System.Reflection;
using GalaSoft.MvvmLight.Ioc;
using log4net;
using TennisScoreBoard.App.Common;
using TennisScoreBoard.EF;
using TennisScoreBoard.EF.Repository;
using TennisScoreBoard.ScoreManager.Implementation;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    public class Bootstrapper
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Bootstrapper()
        {
            s_log.DebugFormat("[NotifyOnMatchStarted]");

            SimpleIoc.Default.Register<ITennisMatchState, TennisMatchState>();
            var matchState = SimpleIoc.Default.GetInstance<ITennisMatchState>();
            
            SimpleIoc.Default.Register<ScoreBoardContext>();
            var scoreBoardContext = SimpleIoc.Default.GetInstance<ScoreBoardContext>();
            
            SimpleIoc.Default.Register<IRepositoryManager>(()=> new RepositoryManager(scoreBoardContext));
            var repoManager = SimpleIoc.Default.GetInstance<IRepositoryManager>();

            SimpleIoc.Default.Register<IMatchService>(()=> new MatchService(repoManager));
            var matchService = SimpleIoc.Default.GetInstance<IMatchService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register(() => new AddPlayerViewModel(matchService, matchState));
            SimpleIoc.Default.Register(() => new StartMatchViewModel(matchService, matchState));
            SimpleIoc.Default.Register(() => new PlayerScoringViewModel(matchService, matchState));
            SimpleIoc.Default.Register(() => new ScoreBoardViewModel(matchService, matchState));
        }

        public MainViewModel MainView => SimpleIoc.Default.GetInstance<MainViewModel>();
        public ScoreBoardViewModel ScoreBoard => SimpleIoc.Default.GetInstance<ScoreBoardViewModel>();
        public AddPlayerViewModel AddPlayer => SimpleIoc.Default.GetInstance<AddPlayerViewModel>();
        public PlayerScoringViewModel PlayerScore => SimpleIoc.Default.GetInstance<PlayerScoringViewModel>();
        public StartMatchViewModel StartMatch => SimpleIoc.Default.GetInstance<StartMatchViewModel>();
    }
}
