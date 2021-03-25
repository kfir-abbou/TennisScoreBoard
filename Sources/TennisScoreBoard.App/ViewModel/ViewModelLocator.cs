using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScoreBoardContext>();
            optionsBuilder.UseSqlServer(@"Server=L-P-KFIRABB-WWN\SQLEXPRESS;Database=TennisScoreDb;Trusted_Connection=True;");
            
            SimpleIoc.Default.Register<IScoreBoardContext>(() => new ScoreBoardContext(optionsBuilder.Options));
            var scoreBoardContext = SimpleIoc.Default.GetInstance<IScoreBoardContext>();
            
            SimpleIoc.Default.Register<IMatchService>(()=> new MatchService(scoreBoardContext));
            var matchService = SimpleIoc.Default.GetInstance<IMatchService>();

            SimpleIoc.Default.Register(()=> new AddPlayerViewModel(matchService));
            var addPlayerViewModel = SimpleIoc.Default.GetInstance<AddPlayerViewModel>();
            
            SimpleIoc.Default.Register(() => new StartMatchViewModel(matchService, addPlayerViewModel));
            SimpleIoc.Default.Register<MainViewModel>();
            
            var startMatchViewModel = SimpleIoc.Default.GetInstance<StartMatchViewModel>();
            SimpleIoc.Default.Register(() => new PlayerScoringViewModel(matchService, startMatchViewModel));
         
            var playerScoringViewModel = SimpleIoc.Default.GetInstance<PlayerScoringViewModel>();
            SimpleIoc.Default.Register<ScoreBoardViewModel>(()=> new ScoreBoardViewModel(matchService, startMatchViewModel, playerScoringViewModel));
        }

        public MainViewModel MainView => SimpleIoc.Default.GetInstance<MainViewModel>();
        public ScoreBoardViewModel ScoreBoard => SimpleIoc.Default.GetInstance<ScoreBoardViewModel>();
        public AddPlayerViewModel AddPlayer => SimpleIoc.Default.GetInstance<AddPlayerViewModel>();
        public PlayerScoringViewModel PlayerScore => SimpleIoc.Default.GetInstance<PlayerScoringViewModel>();
        public StartMatchViewModel StartMatch => SimpleIoc.Default.GetInstance<StartMatchViewModel>();
    }
}
