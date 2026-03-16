using System.Collections.ObjectModel;
using System.Windows.Input;
using Servr.Application.Order;
using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Domain.Model;
using Servr.Infrastructure.Data;
using Servr.Infrastructure.Logger;
using Servr.Presentation.Command;

namespace Servr.Presentation.ViewModel
{
  public class MainViewModel : ObservableObject
  {
    private readonly ILogger _logger = new DebugLogger();
    private readonly OrderService _orderService = new();
    private readonly Dictionary<MenuCategory, ObservableCollection<IItem>> _menuItems;

    private int _tableNumber;
    private int _nextOrderId;
    private object _selectedItem;
    private IEnumerable<IItem> _currentItems;

    public ObservableCollection<IItem> OrderView { get; } = new();
    public ObservableCollection<MenuCategory> CategoryOptions { get; }

    public IEnumerable<IItem> CurrentItems
    {
      get => _currentItems;
      set => SetProperty(ref _currentItems, value);
    }

    public object SelectedItem
    {
      get => _selectedItem;
      set
      {
        if (!SetProperty(ref _selectedItem, value) || value is not IItem item) return;

        string typeLabel = item is IDrink ? "IDrink" : "IMenuItem";
        _logger.Log(LogLevel.INFO, $"[{typeLabel}] {item.Name} added to order.");
        AddItemToOrder(item);
      }
    }

    // Commands
    public ICommand SelectCategoryCommand { get; }
    public ICommand SendOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }
    public ICommand DiscountCommand { get; }
    public ICommand PayCommand { get; }

    public MainViewModel()
    {
      CategoryOptions = new ObservableCollection<MenuCategory>(Data.GetCategories());

      _menuItems = new Dictionary<MenuCategory, ObservableCollection<IItem>>
            {
                { MenuCategory.Starters,        new(Data.GetStarters()) },
                { MenuCategory.MainCourses,     new(Data.GetMainCourses()) },
                { MenuCategory.Sides,           new(Data.GetSides()) },
                { MenuCategory.Desserts,        new(Data.GetDesserts()) },
                { MenuCategory.SoftDrinks,      new(Data.GetSoftDrinks()) },
                { MenuCategory.AlcoholicDrinks, new(Data.GetAlcoholicDrinks()) },
            };

      _currentItems = _menuItems[MenuCategory.Starters];

      SelectCategoryCommand = new RelayCommand(
          param => { if (param is MenuCategory cat) CurrentItems = _menuItems[cat]; });

      SendOrderCommand = new RelayCommand(
          _ => SendOrder(),
          _ => OrderView.Any());

      CancelOrderCommand = new RelayCommand(
          _ => CancelOrder(),
          _ => OrderView.Any());
    }

    private void AddItemToOrder(IItem item)
    {
      int before = OrderView.Count;
      OrderView.Add(item);

      if (OrderView.Count == before + 1)
        _logger.Log(LogLevel.INFO, $"{item.Name} correctly added to order.");
      else
        _logger.Log(LogLevel.WARNING, $"{item.Name} was not added to order.");
    }

    private void SendOrder()
    {
      _orderService.NewOrder(new Order(_nextOrderId++, _tableNumber, OrderView));
      _logger.Log(LogLevel.INFO, $"Order {_nextOrderId - 1} sent for table {_tableNumber}.");
      OrderView.Clear();
    }

    private void CancelOrder()
    {
      OrderView.Clear();

      if (OrderView.Count == 0)
        _logger.Log(LogLevel.INFO, "Order cancelled.");
      else
        _logger.Log(LogLevel.WARNING, "Order failed to clear.");
    }
  }
}
