using System.Configuration;
using System.Data;
using System.Windows;
using Servr.Infrastructure.Logger;
using Servr.Domain.Interface;


namespace Servr
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : System.Windows.Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      ILogger logger = new DebugLogger();
      MainWindow mainWindow = new MainWindow(logger);
      mainWindow.Show();
    }
  }
}
