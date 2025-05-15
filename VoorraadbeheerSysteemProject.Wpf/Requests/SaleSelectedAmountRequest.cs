using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Requests
{
    class SaleSelectedAmountRequest : INotifyPropertyChanged
    {

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


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
