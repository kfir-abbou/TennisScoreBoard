using System.Reflection;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using log4net;
using Microsoft.EntityFrameworkCore;
using TennisScoreBoard.App.Common;
using TennisScoreBoard.App.Config;
using TennisScoreBoard.EF;
using TennisScoreBoard.ScoreManager.Implementation;
using TennisScoreBoard.ScoreManager.Interface;

namespace TennisScoreBoard.App.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static readonly ILog s_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            s_log.DebugFormat("[NotifyOnMatchStarted]");

            SimpleIoc.Default.Register<ConfigSerializer>();
            var configSerializer = SimpleIoc.Default.GetInstance<ConfigSerializer>();
            var config = configSerializer.LoadScoreboardConfig(@"Config\ScoreboardConfig.xml");

            var optionsBuilder = new DbContextOptionsBuilder<ScoreBoardContext>();
            optionsBuilder.UseSqlServer($@"Server={config.ConnectionString};Database=TennisScoreDb;Trusted_Connection=True;");

            SimpleIoc.Default.Register<ITennisMatchState, TennisMatchState>();
            var matchState = SimpleIoc.Default.GetInstance<ITennisMatchState>();
            
            SimpleIoc.Default.Register<IScoreBoardContext>(() => new ScoreBoardContext(optionsBuilder.Options));
            var scoreBoardContext = SimpleIoc.Default.GetInstance<IScoreBoardContext>();
            
            SimpleIoc.Default.Register<IMatchService>(()=> new MatchService(scoreBoardContext));
            var matchService = SimpleIoc.Default.GetInstance<IMatchService>();

            SimpleIoc.Default.Register(()=> new AddPlayerViewModel(matchService, matchState));
            SimpleIoc.Default.Register(() => new StartMatchViewModel(matchService, matchState));
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register(() => new PlayerScoringViewModel(matchService, matchState));
            SimpleIoc.Default.Register<ScoreBoardViewModel>(()=> new ScoreBoardViewModel(matchService, matchState));
        }

        public MainViewModel MainView => SimpleIoc.Default.GetInstance<MainViewModel>();
        public ScoreBoardViewModel ScoreBoard => SimpleIoc.Default.GetInstance<ScoreBoardViewModel>();
        public AddPlayerViewModel AddPlayer => SimpleIoc.Default.GetInstance<AddPlayerViewModel>();
        public PlayerScoringViewModel PlayerScore => SimpleIoc.Default.GetInstance<PlayerScoringViewModel>();
        public StartMatchViewModel StartMatch => SimpleIoc.Default.GetInstance<StartMatchViewModel>();
    }
}
