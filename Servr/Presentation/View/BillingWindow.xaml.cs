using System.Windows;
using Servr.Presentation.ViewModel;

namespace Servr.Presentation.View
{
    /// <summary>
    /// Interaction logic for BillingWindow.xaml
    /// </summary>
    public partial class BillingWindow : Window
    {
        public BillingWindow(BillingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.PaymentCompleted += message =>
            {
                MessageBox.Show(message, "Payment Complete");
                Close();
            };
        }
    }
}
