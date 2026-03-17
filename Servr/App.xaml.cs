using System.Windows;

using Servr.Infrastructure.Logger;
using Servr.Domain.Interface;
using Servr.Application.Order;
using Servr.Presentation.ViewModel;
using Servr.Application.Billing;


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

            // composition root
            ILogger logger = new DebugLogger();
            logger.Log(LogLevel.INFO, "Application starting");

            OrderService orderService = new OrderService(logger);
            BillingService billingService = new BillingService(logger);
            var viewModel = new MainViewModel(logger, orderService, billingService); // add bill service
            MainWindow mainWindow = new MainWindow(viewModel);
            mainWindow.Show();
        }
    }
}
