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
    private readonly Dictionary<MenuCategory, ObservableCollection<IItem>> _menuItems;
    public ObservableCollection<IItem> Receipt { get; } = new();

    private object _selectedItem;
    public object SelectedItem
    {
      get => _selectedItem;
      set
      {
        if (SetProperty(ref _selectedItem, value) && value is IItem item)
        {
          string typeLabel = item is IDrink ? "IDrink" : "IMenuItem";
          _logger.Log(LogLevel.INFO, $"[{typeLabel}] {item.Name} - added to order list.");

          int before = Receipt.Count;
          Receipt.Add(item);

          if (Receipt.Count == before + 1)
            _logger.Log(LogLevel.INFO, "Menu Item correctly added to receipt");
          else
            _logger.Log(LogLevel.WARNING, "Menu Item was not added to receipt");
        }
      }
    }

    private IEnumerable<IItem> _currentItems;
    public IEnumerable<IItem> CurrentItems
    {
      get => _currentItems;
      set => SetProperty(ref _currentItems, value);
    }

    public ObservableCollection<MenuCategory> CategoryOptions { get; }
    public ICommand SelectCategoryCommand { get; }
    public ICommand SendOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }
    public ICommand DiscountCommand { get; }
    public ICommand PayCommand { get; }

    public MainViewModel()
    {
      // Keep logger, as it wont work with passing. DataContext is iffy.
      // Likely caused by it exspecting empty ctor.
      _logger = new DebugLogger();
      CategoryOptions = new ObservableCollection<MenuCategory>(Data.GetCategories());

      _menuItems = new Dictionary<MenuCategory, ObservableCollection<IItem>>
    {
        { MenuCategory.Starters,         new(Data.GetStarters()) },
        { MenuCategory.MainCourses,      new(Data.GetMainCourses()) },
        { MenuCategory.Sides,            new(Data.GetSides()) },
        { MenuCategory.Desserts,         new(Data.GetDesserts()) },
        { MenuCategory.SoftDrinks,       new(Data.GetSoftDrinks()) },
        { MenuCategory.AlcoholicDrinks,  new(Data.GetAlcoholicDrinks()) },
    };

      _currentItems = _menuItems[MenuCategory.Starters];

      SelectCategoryCommand = new RelayCommand(param =>
      {
        if (param is MenuCategory category)
          CurrentItems = _menuItems[category];
      });

      // SendOrderCommand = new RelayCommand(_ => orderService(order)),
      //                                     _ => CanSendOrder());
      CancelOrderCommand = new RelayCommand(_ => CancelOrder(),
                                            _ => CanCancelOrder());
      // PayCommand = new RelayCommand(_ => )
      // DiscountCommand = new RelayCommand(_ => );
    }
    private void CancelOrder()
    {
      _logger.Log(LogLevel.INFO, "Cancel order pressed");
      Receipt.Clear();
      if (Receipt.Count == 0)
      {
        _logger.Log(LogLevel.INFO, "Order cancelled successfully");
      }
      else
      {
        _logger.Log(LogLevel.WARNING, "Order not cleared");
      }
    }

    private bool CanCancelOrder() => Receipt.Any();

    private bool CanSendOrder() => Receipt.Any();
  }
}
