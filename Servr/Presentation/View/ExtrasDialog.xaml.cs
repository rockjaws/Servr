using System.Windows;
using Servr.Domain.Interface;
using Servr.Domain.Model;
using Servr.Infrastructure.Data;

namespace Servr.Presentation.View
{
    public partial class ExtrasDialog : Window
    {
        public record ExtraOption(string Name, decimal Price);

        public string ItemHeader { get; }
        public List<ExtraOption> Extras { get; }

        public ExtraOption? SelectedExtra { get; private set; }
        public bool IsAdd { get; private set; }

        public ExtrasDialog(IItem item)
        {
            var baseName = GetBaseName(item);
            ItemHeader = item.Name;
            Extras = Data.GetExtrasForItem(baseName)
                .Select(e => new ExtraOption(e.Name, e.Price))
                .ToList();

            InitializeComponent();
            DataContext = this;
        }

        private static string GetBaseName(IItem item)
        {
            var current = item;
            while (current is MenuItemDecorator mid)
                current = mid.WrappedItem;
            while (current is DrinkDecorator dd)
                current = dd.WrappedDrink;
            return current.Name;
        }

        private void OnAdd(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement { Tag: ExtraOption extra })
            {
                SelectedExtra = extra;
                IsAdd = true;
                DialogResult = true;
            }
        }

        private void OnRemove(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement { Tag: ExtraOption extra })
            {
                SelectedExtra = extra;
                IsAdd = false;
                DialogResult = true;
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
