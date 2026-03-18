using System.Collections.ObjectModel;
using System.Windows.Input;

using Servr.Application.Billing;
using Servr.Application.Order;
using Servr.Domain.Enum;
using Servr.Domain.Interface;
using Servr.Domain.Model;
using Servr.Infrastructure.Data;
using Servr.Presentation.Command;

namespace Servr.Presentation.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly ILogger _logger;
        private readonly OrderService _orderService;
        private readonly BillingService _billingService;
        private readonly Dictionary<MenuCategory, ObservableCollection<IItem>> _menuItems;

        private int _tableNumber;
        private int _nextOrderId;
        private DiscountType _discountType;
        private object? _selectedItem;
        private IEnumerable<IItem> _currentItems;

        public ObservableCollection<IItem> OrderView { get; } = new();
        public ObservableCollection<MenuCategory> CategoryOptions { get; }

        public IBill CurrentTableBill => _billingService.GetBillForTable(_tableNumber);

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
                AddItemToOrder(item);
                SelectedItem = null;
            }
        }

        // Events for View interactions
        public event Action<BillingViewModel>? OpenBillingRequested;
        public event Func<int, int?>? SetTableRequested;
        public event Func<DiscountType, DiscountType?>? SetDiscountRequested;
        public event Func<IItem, (string Name, decimal Price, bool IsAdd)?>? AddExtraRequested;
        public event Action<string>? ShowNotificationRequested;

        // Commands
        public ICommand SelectCategoryCommand { get; }
        public ICommand SendOrderCommand { get; }
        public ICommand SetTableCommand { get; }
        public ICommand CancelOrderCommand { get; }
        public ICommand DiscountCommand { get; }
        public ICommand PayCommand { get; }
        public ICommand AddExtraCommand { get; }
        public ICommand AddItemCommand { get; }

        public MainViewModel(
            ILogger logger,
            OrderService orderService,
            BillingService billingService
        )
        {
            _logger = logger;
            _orderService = orderService;
            _billingService = billingService;
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

            SendOrderCommand = new RelayCommand(
                _ => SendOrder(),
                _ => OrderView.Any() && _tableNumber != 0
            );
            CancelOrderCommand = new RelayCommand(_ => CancelOrder(), _ => OrderView.Any());
            SetTableCommand = new RelayCommand(_ => SetTable());
            DiscountCommand = new RelayCommand(_ => SetDiscount());
            PayCommand = new RelayCommand(
                _ => OpenBillingWindow(),
                _ => _tableNumber != 0 && billingService.Bills.ContainsKey(_tableNumber)
            );
            AddExtraCommand = new RelayCommand(
                param => ApplyExtra(param as IItem),
                param => param is IItem
            );
            AddItemCommand = new RelayCommand(
                param => { if (param is IItem item) AddItemToOrder(item); },
                param => param is IItem
            );
        }

        private void OpenBillingWindow()
        {
            var tableAtTimeOfPay = _tableNumber;
            var bill = _billingService.GetBillForTable(tableAtTimeOfPay);

            var vm = new BillingViewModel(bill);
            vm.PaymentCompleted += _ =>
            {
                _logger.Log(LogLevel.INFO, $"Payment completed for table {tableAtTimeOfPay}. Bill total: {bill.BillTotal:N2} kr.");
                _billingService.ClearBill(tableAtTimeOfPay);
                OnPropertyChanged(nameof(CurrentTableBill));
            };
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
            OrderView.Add(item);
            _logger.Log(LogLevel.INFO, $"{item.Name} added to order.");
        }

        private void SendOrder()
        {
            var order = new Order(_nextOrderId++, _tableNumber, OrderView);
            _orderService.NewOrder(order);
            _billingService.AddOrderToTable(_tableNumber, DiscountType, order);

            _logger.Log(LogLevel.INFO, $"Order {_nextOrderId - 1} sent for table {_tableNumber}.");
            OrderView.Clear();
            DiscountType = DiscountType.None;
            OnPropertyChanged(nameof(CurrentTableBill));
        }

        private void ApplyExtra(IItem? item)
        {
            if (item == null)
            {
                _logger.Log(LogLevel.WARNING, "ApplyExtra called with null item.");
                return;
            }
            var result = AddExtraRequested?.Invoke(item);
            if (result == null) return;

            var (extraName, extraPrice, isAdd) = result.Value;
            var index = OrderView.IndexOf(item);
            if (index < 0)
            {
                _logger.Log(LogLevel.WARNING, $"Item '{item.Name}' not found in OrderView when applying extra.");
                return;
            }

            var price = isAdd ? extraPrice : 0m;
            var label = isAdd ? extraName : $"No {extraName}";

            IItem decorated = item switch
            {
                IDrink drink => new DrinkExtra(drink, label, price),
                IMenuItem menuItem => new MenuItemExtra(menuItem, label, price),
                _ => item
            };

            OrderView[index] = decorated;
            _logger.Log(LogLevel.INFO, $"Extra '{label}' applied to {item.Name}.");
        }

        public void NotifyOrderReady(IOrder order)
        {
            ShowNotificationRequested?.Invoke($"Order #{order.OrderId} is ready for table {order.Table}!");
        }

        private void CancelOrder()
        {
            OrderView.Clear();
            _logger.Log(LogLevel.INFO, "Order cancelled.");
        }
    }
}
