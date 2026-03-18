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
using System.Windows.Threading;

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

                vm.AddExtraRequested += item =>
                {
                    var dialog = new ExtrasDialog(item) { Owner = this };
                    if (dialog.ShowDialog() == true && dialog.SelectedExtra != null)
                        return (dialog.SelectedExtra.Name, dialog.SelectedExtra.Price, dialog.IsAdd);
                    return null;
                };

                vm.ShowNotificationRequested += message =>
                {
                    Dispatcher.Invoke(() => ShowToast(message));
                };
            }
        }

        private DispatcherTimer? _toastTimer;

        private void ShowToast(string message)
        {
            ToastText.Text = message;
            ToastNotification.Visibility = Visibility.Visible;

            _toastTimer?.Stop();
            _toastTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(8) };
            _toastTimer.Tick += (_, _) => DismissToast();
            _toastTimer.Start();
        }

        private void DismissToast()
        {
            ToastNotification.Visibility = Visibility.Collapsed;
            _toastTimer?.Stop();
        }

        private void ToastNotification_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DismissToast();
        }
    }
}
