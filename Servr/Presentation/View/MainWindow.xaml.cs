using System.Windows;
using Servr.Domain.Interface;
using Servr.Infrastructure.Logger;
using Servr.Presentation.View;
using Servr.Presentation.ViewModel;

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

            if (DataContext is MainViewModel vm)
            {
                vm.OpenBillingRequested += billingVm =>
                {
                    new BillingWindow(billingVm) { Owner = this }.Show();
                };

                vm.SetTableRequested += current =>
                {
                    var dialog = new SetTableDialog(current) { Owner = this };
                    return dialog.ShowDialog() == true ? dialog.TableNumber : null;
                };

                vm.SetDiscountRequested += current =>
                {
                    var dialog = new SetDiscountDialog(current) { Owner = this };
                    return dialog.ShowDialog() == true ? dialog.SelectedDiscount : null;
                };
            }
        }
    }
}
