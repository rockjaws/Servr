using System.Windows.Input;

using Servr.Domain.Interface;
using Servr.Presentation.Command;

namespace Servr.Presentation.ViewModel
{
    public class BillingViewModel : ObservableObject
    {
        private string _signature = string.Empty;
        private IBill _bill;
        public IBill Bill => _bill;
        public List<IItem> BillItems => _bill
                                        .Orders.SelectMany(o => o.Food.Cast<IItem>().Concat(o.Drinks.Cast<IItem>()))
                                        .ToList();

        public string Signature
        {
            get => _signature;
            set => SetProperty(ref _signature, value);
        }

        public ICommand PayCommand { get; }

        public event Action<string>? PaymentCompleted;

        public BillingViewModel(IBill bill)
        {
            _bill = bill;
            PayCommand = new RelayCommand(_ => Pay(), _ => !string.IsNullOrWhiteSpace(Signature));
        }

        private void Pay()
        {
            PaymentCompleted?.Invoke($"Payment of {_bill.BillTotal:N2} kr. confirmed.\nSigned by: {Signature}");
        }
    }
}
