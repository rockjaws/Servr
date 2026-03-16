using System.Windows;
using Servr.Domain.Enum;

namespace Servr.Presentation.View
{
    public partial class SetDiscountDialog : Window
    {
        public DiscountType SelectedDiscount { get; private set; }

        public SetDiscountDialog(DiscountType current)
        {
            InitializeComponent();
            RadioStudent.IsChecked = current == DiscountType.Student;
            RadioStaff.IsChecked = current == DiscountType.Staff;
            RadioBirthday.IsChecked = current == DiscountType.Birthday;
        }

        private void OnOk(object sender, RoutedEventArgs e)
        {
            if (RadioStudent.IsChecked == true)
                SelectedDiscount = DiscountType.Student;
            else if (RadioStaff.IsChecked == true)
                SelectedDiscount = DiscountType.Staff;
            else if (RadioBirthday.IsChecked == true)
                SelectedDiscount = DiscountType.Birthday;
            else
            {
                MessageBox.Show("Please select a discount type.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
