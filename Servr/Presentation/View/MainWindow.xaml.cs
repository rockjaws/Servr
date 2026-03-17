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
using Servr.Presentation.View;
using Servr.Presentation.ViewModel;

namespace Servr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
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
