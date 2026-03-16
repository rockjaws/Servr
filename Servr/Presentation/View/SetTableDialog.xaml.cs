using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Servr.Presentation.View
{
    public partial class SetTableDialog : Window
    {
        public int TableNumber { get; private set; }

        public SetTableDialog(int current)
        {
            InitializeComponent();
            if (current > 0)
                TableNumberInput.Text = current.ToString();
            TableNumberInput.Focus();
            TableNumberInput.SelectAll();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        private void OnOk(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TableNumberInput.Text, out int number) && number > 0)
            {
                TableNumber = number;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid table number.", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
