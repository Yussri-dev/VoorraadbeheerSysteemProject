using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Requests
{
    public class ProductSelectedRequest : INotifyPropertyChanged
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal PurchasePrice { get; set; }

        private decimal _amountPrice { get; set; }
        public decimal AmountPrice
        {
            get => _amountPrice;
            set
            {
                if (_amountPrice != value)
                {
                    _amountPrice = value;
                    OnPropertyChanged(nameof(AmountPrice));
                }
            }
        }

        private decimal _quantity;
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
