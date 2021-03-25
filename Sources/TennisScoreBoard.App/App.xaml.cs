using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using log4net;
using log4net.Config;

namespace TennisScoreBoard.App
{
    
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
