using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Infrastructure.Data;
using Servr.Infrastructure.Logger;
using Servr.Presentation.Command;

namespace Servr.Presentation.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private ILogger _logger;
        private readonly Dictionary<MenuCategory, IEnumerable> _menuItems;

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value) && value != null)
                {
                    if (value is IDrink drink)
                    {
                        _logger.Log(LogLevel.INFO, $"[IDrink] {drink.Name} - added to order list.");
                        // Add to drink orderList
                    }
                    else if (value is IMenuItem menuItem)
                    {
                        _logger.Log(
                            LogLevel.INFO,
                            $"[IMenuItem] {menuItem.Name} - added to order list."
                        );
                        // Add to food orderList
                    }
                }
            }
        }

        private IEnumerable _currentItems;
        public IEnumerable CurrentItems
        {
            get => _currentItems;
            set => SetProperty(ref _currentItems, value);
        }

        public ObservableCollection<MenuCategory> CategoryOptions { get; }
        public ICommand SelectCategoryCommand { get; }

        public MainViewModel()
        {
            // Keep logger, as it wont work with passing. DataContext is iffy.
            // Likely caused by it exspecting empty ctor.
            _logger = new DebugLogger();
            CategoryOptions = new ObservableCollection<MenuCategory>(Data.GetCategories());

            _menuItems = new Dictionary<MenuCategory, IEnumerable>
            {
                { MenuCategory.Starters, new ObservableCollection<IMenuItem>(Data.GetStarters()) },
                {
                    MenuCategory.MainCourses,
                    new ObservableCollection<IMenuItem>(Data.GetMainCourses())
                },
                { MenuCategory.Sides, new ObservableCollection<IMenuItem>(Data.GetSides()) },
                { MenuCategory.Desserts, new ObservableCollection<IMenuItem>(Data.GetDesserts()) },
                { MenuCategory.SoftDrinks, new ObservableCollection<IDrink>(Data.GetSoftDrinks()) },
                {
                    MenuCategory.AlcoholicDrinks,
                    new ObservableCollection<IDrink>(Data.GetAlcoholicDrinks())
                },
            };

            _currentItems = _menuItems[MenuCategory.Starters];

            SelectCategoryCommand = new RelayCommand(param =>
            {
                if (param is MenuCategory category)
                    CurrentItems = _menuItems[category];
            });
        }
    }
}
