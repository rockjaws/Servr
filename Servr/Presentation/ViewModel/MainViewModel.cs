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
        private readonly Dictionary<int, IBill> _bills = new();

        private int _tableNumber;
        private int _nextOrderId;
        private DiscountType _discountType;
        private object? _selectedItem;
        private IEnumerable<IItem> _currentItems;

        public ObservableCollection<IItem> OrderView { get; } = new();
        public ObservableCollection<MenuCategory> CategoryOptions { get; }

        public IBill CurrentTableBill
        {
            get
            {
                if (_bills.TryGetValue(_tableNumber, out var bill))
                {
                    _logger.Log(LogLevel.INFO, $"Subtotal: {bill.Subtotal}");
                    return bill;
                }
                return null;
            }
        }

        public DiscountType DiscountType
        {
            get => _discountType;
            private set => SetProperty(ref _discountType, value);
        }

        public int TableNumber
        {
            get => _tableNumber;
            private set
            {
                if (!SetProperty(ref _tableNumber, value))
                    return;
                OrderView.Clear();
                _logger.Log(LogLevel.INFO, $"Table set to {_tableNumber}.");
                OnPropertyChanged(nameof(CurrentTableBill));
            }
        }

        public IEnumerable<IItem> CurrentItems
        {
            get => _currentItems;
            set => SetProperty(ref _currentItems, value);
        }

        public object? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!SetProperty(ref _selectedItem, value) || value is not IItem item)
                    return;

                string typeLabel = item is IDrink ? "IDrink" : "IMenuItem";
                _logger.Log(LogLevel.INFO, $"[{typeLabel}] {item.Name} added to order.");
                AddItemToOrder(item);
            }
        }

        // Events for View interactions
        public event Action<BillingViewModel>? OpenBillingRequested;
        public event Func<int, int?>? SetTableRequested;
        public event Func<DiscountType, DiscountType?>? SetDiscountRequested;

        // Commands
        public ICommand SelectCategoryCommand { get; }
        public ICommand SendOrderCommand { get; }
        public ICommand SetTableCommand { get; }
        public ICommand CancelOrderCommand { get; }
        public ICommand DiscountCommand { get; }
        public ICommand PayCommand { get; }

        public MainViewModel()
        {
            CategoryOptions = new ObservableCollection<MenuCategory>(Data.GetCategories());

            _menuItems = new Dictionary<MenuCategory, ObservableCollection<IItem>>
            {
                { MenuCategory.Starters, new(Data.GetStarters()) },
                { MenuCategory.MainCourses, new(Data.GetMainCourses()) },
                { MenuCategory.Sides, new(Data.GetSides()) },
                { MenuCategory.Desserts, new(Data.GetDesserts()) },
                { MenuCategory.SoftDrinks, new(Data.GetSoftDrinks()) },
                { MenuCategory.AlcoholicDrinks, new(Data.GetAlcoholicDrinks()) },
            };

            _currentItems = _menuItems[MenuCategory.Starters];

            SelectCategoryCommand = new RelayCommand(param =>
            {
                if (param is MenuCategory cat)
                    CurrentItems = _menuItems[cat];
            });

            SendOrderCommand = new RelayCommand(_ => SendOrder(), _ => OrderView.Any());
            CancelOrderCommand = new RelayCommand(_ => CancelOrder(), _ => OrderView.Any());
            SetTableCommand = new RelayCommand(_ => SetTable());
            DiscountCommand = new RelayCommand(_ => SetDiscount());
            PayCommand = new RelayCommand(_ => OpenBillingWindow(), _ => _bills.Any());
        }

        private void OpenBillingWindow()
        {
            var items = _bills.Values
                .SelectMany(b => b.Orders)
                .SelectMany(o => o.Food.Cast<IItem>().Concat(o.Drinks.Cast<IItem>()))
                .ToList();

            var vm = new BillingViewModel(items);
            OpenBillingRequested?.Invoke(vm);
        }

        private void SetDiscount()
        {
            var result = SetDiscountRequested?.Invoke(_discountType);
            if (result.HasValue)
            {
                DiscountType = result.Value;
                _logger.Log(LogLevel.INFO, $"Discount set to {_discountType}.");
            }
        }

        private void SetTable()
        {
            var result = SetTableRequested?.Invoke(_tableNumber);
            if (result.HasValue)
            {
                TableNumber = result.Value;
            }
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
            var order = new Order(_nextOrderId++, _tableNumber, OrderView);
            _orderService.NewOrder(order);

            if (_bills.TryGetValue(_tableNumber, out var bill))
                bill.Orders.Add(order);
            else
                _bills[_tableNumber] = new Bill(1, DiscountType, "Bo", new List<IOrder> { order });

            _logger.Log(LogLevel.INFO, $"Order {_nextOrderId - 1} sent for table {_tableNumber}.");
            OrderView.Clear();
            DiscountType = DiscountType.None;
            TableNumber = 0;
            OnPropertyChanged(nameof(CurrentTableBill));
        }

        private void CancelOrder()
        {
            OrderView.Clear();
            _logger.Log(LogLevel.INFO, "Order cancelled.");
        }
    }
}
