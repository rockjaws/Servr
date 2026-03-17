using System.Windows.Input;
using Servr.Domain.Interface;
using Servr.Presentation.Command;

namespace Servr.Presentation.ViewModel
{
    public class BillingViewModel : ObservableObject
    {
        private string _signature = string.Empty;

        public List<IItem> BillItems { get; }

        public decimal Total => BillItems.Sum(i => i.Price);

        public string Signature
        {
            get => _signature;
            set => SetProperty(ref _signature, value);
        }

        public ICommand PayCommand { get; }

        public event Action<string>? PaymentCompleted;

        public BillingViewModel(List<IItem> billItems)
        {
            BillItems = billItems;
            PayCommand = new RelayCommand(_ => Pay(), _ => !string.IsNullOrWhiteSpace(Signature));
        }

        private void Pay()
        {
            PaymentCompleted?.Invoke($"Payment of {Total:N2} kr. confirmed.\nSigned by: {Signature}");
        }
    }
}
