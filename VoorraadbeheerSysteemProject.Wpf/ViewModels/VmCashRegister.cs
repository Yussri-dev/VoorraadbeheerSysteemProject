using PdfSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmCashRegister : VmBase
    {
        //coins
        private int _coin1Cent = 0;
        private int _coin2Cent = 0;
        private int _coin5Cent = 0;
        private int _coin10Cent = 0;
        private int _coin20Cent = 0;
        private int _coin50Cent = 0;
        private int _coin1Euro = 0;
        private int _coin2Euro = 0;

        //bills
        private int _bill5Euro = 0;
        private int _bill10Euro = 0;
        private int _bill20Euro = 0;
        private int _bill50Euro = 0;
        private int _bill100Euro = 0;
        private int _bill200Euro = 0;
        private int _bill500Euro = 0;

        #region properties
        #region coin properties
        public int Coin1Cent { get => _coin1Cent; set { _coin1Cent = value; OnPropertyChanged(nameof(Coin1Cent)); OnPropertyChanged(nameof(Total1CentAmount)); OnPropertyChanged(nameof(TotalCoinAmount)); } }
        public int Coin2Cent { get => _coin2Cent; set { _coin2Cent = value; OnPropertyChanged(nameof(Coin2Cent)); OnPropertyChanged(nameof(Total2CentAmount)); OnPropertyChanged(nameof(TotalCoinAmount)); } }
        public int Coin5Cent { get => _coin5Cent; set { _coin5Cent = value; OnPropertyChanged(nameof(Coin5Cent)); OnPropertyChanged(nameof(Total5CentAmount)); OnPropertyChanged(nameof(TotalCoinAmount)); } }
        public int Coin10Cent { get => _coin10Cent; set { _coin10Cent = value; OnPropertyChanged(nameof(Coin10Cent)); OnPropertyChanged(nameof(Total10CentAmount)); OnPropertyChanged(nameof(TotalCoinAmount)); } }
        public int Coin20Cent { get => _coin20Cent; set { _coin20Cent = value; OnPropertyChanged(nameof(Coin20Cent)); OnPropertyChanged(nameof(Total20CentAmount)); OnPropertyChanged(nameof(TotalCoinAmount)); } }
        public int Coin50Cent { get => _coin50Cent; set { _coin50Cent = value; OnPropertyChanged(nameof(Coin50Cent)); OnPropertyChanged(nameof(Total50CentAmount)); OnPropertyChanged(nameof(TotalCoinAmount)); } }
        public int Coin1Euro { get => _coin1Euro; set { _coin1Euro = value; OnPropertyChanged(nameof(Coin1Euro)); OnPropertyChanged(nameof(Total1EuroAmount)); OnPropertyChanged(nameof(TotalCoinAmount)); } }
        public int Coin2Euro { get => _coin2Euro; set { _coin2Euro = value; OnPropertyChanged(nameof(Coin2Euro)); OnPropertyChanged(nameof(Total2EuroAmount)); OnPropertyChanged(nameof(TotalCoinAmount)); } }

        public decimal Total1CentAmount => Coin1Cent * 0.01m;
        public decimal Total2CentAmount => Coin2Cent * 0.02m;
        public decimal Total5CentAmount => Coin5Cent * 0.05m;
        public decimal Total10CentAmount => Coin10Cent * 0.10m;
        public decimal Total20CentAmount => Coin20Cent * 0.20m;
        public decimal Total50CentAmount => Coin50Cent * 0.50m;
        public decimal Total1EuroAmount => Coin1Euro * 1.00m;
        public decimal Total2EuroAmount => Coin2Euro * 2.00m;
        public decimal TotalCoinAmount => Total1CentAmount + Total2CentAmount + Total5CentAmount + 
            Total10CentAmount + Total20CentAmount + Total50CentAmount + Total1EuroAmount + Total2EuroAmount;
        #endregion
        #region bill properties
        public int Bill5Euro { get => _bill5Euro; set {_bill5Euro = value; OnPropertyChanged(nameof(Bill5Euro)); OnPropertyChanged(nameof(Total5EuroAmount)); OnPropertyChanged(nameof(TotalBillAmount)); } }
        public int Bill10Euro { get => _bill10Euro; set {_bill10Euro = value; OnPropertyChanged(nameof(Bill10Euro)); OnPropertyChanged(nameof(Total10EuroAmount)); OnPropertyChanged(nameof(TotalBillAmount)); } }
        public int Bill20Euro { get => _bill20Euro; set {_bill20Euro = value; OnPropertyChanged(nameof(Bill20Euro)); OnPropertyChanged(nameof(Total20EuroAmount)); OnPropertyChanged(nameof(TotalBillAmount)); } }
        public int Bill50Euro { get => _bill50Euro; set {_bill50Euro = value; OnPropertyChanged(nameof(Bill50Euro)); OnPropertyChanged(nameof(Total50EuroAmount)); OnPropertyChanged(nameof(TotalBillAmount)); } }
        public int Bill100Euro { get => _bill100Euro; set {_bill100Euro = value; OnPropertyChanged(nameof(Bill100Euro)); OnPropertyChanged(nameof(Total100EuroAmount)); OnPropertyChanged(nameof(TotalBillAmount)); } }
        public int Bill200Euro { get => _bill200Euro; set {_bill200Euro = value; OnPropertyChanged(nameof(Bill200Euro)); OnPropertyChanged(nameof(Total200EuroAmount)); OnPropertyChanged(nameof(TotalBillAmount)); } }
        public int Bill500Euro { get => _bill500Euro; set {_bill500Euro = value; OnPropertyChanged(nameof(Bill500Euro)); OnPropertyChanged(nameof(Total500EuroAmount)); OnPropertyChanged(nameof(TotalBillAmount)); } }
        public decimal Total5EuroAmount => Bill5Euro * 5.00m;
        public decimal Total10EuroAmount => Bill10Euro * 10.00m;
        public decimal Total20EuroAmount => Bill20Euro * 20.00m;
        public decimal Total50EuroAmount => Bill50Euro * 50.00m;
        public decimal Total100EuroAmount => Bill100Euro * 100.00m;
        public decimal Total200EuroAmount => Bill200Euro * 200.00m;
        public decimal Total500EuroAmount => Bill500Euro * 500.00m;
        public decimal TotalBillAmount => Total5EuroAmount + Total10EuroAmount + Total20EuroAmount +
            Total50EuroAmount + Total100EuroAmount + Total200EuroAmount + Total500EuroAmount;
        #endregion
        #endregion


        public ICommand NavigateDashboardCommand { get; }

        #region Constructor
        public VmCashRegister(NavigationStore navigationStore)
        {
            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));
        }
        #endregion
    }
}
