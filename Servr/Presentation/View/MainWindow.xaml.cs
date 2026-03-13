using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Servr.Domain.Interface;
using Servr.Infrastructure.Logger;

namespace Servr
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>

  public partial class MainWindow : Window
  {
    private readonly ILogger _logger;
    public MainWindow(ILogger logger)
    {
      InitializeComponent();
      _logger = logger;
      _logger.Log(LogLevel.INFO, "Application starting");
    }
  }
}
