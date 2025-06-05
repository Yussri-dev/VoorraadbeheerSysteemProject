using PdfSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services.CashRegister;
using VoorraadbeheerSysteemProject.Wpf.Services.Drawer;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmCashRegister : VmBase
    {
        //api
        private CashRegisterRequest _cashRegisterRequest;
        private DrawerRequests _drawerRequests;
        private CashShiftDTO? _cashShift = null;
        private CashShiftCloseResultDto? _cashShiftCloseResult = null;


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
        public int Coin1Cent { get => _coin1Cent; set => SetCoin(ref _coin1Cent, value, nameof(Coin1Cent), nameof(Total1CentAmount)); }
        public int Coin2Cent { get => _coin2Cent; set => SetCoin(ref _coin2Cent, value, nameof(Coin2Cent), nameof(Total2CentAmount)); }
        public int Coin5Cent { get => _coin5Cent; set => SetCoin(ref _coin5Cent, value, nameof(Coin5Cent), nameof(Total5CentAmount)); }
        public int Coin10Cent { get => _coin10Cent; set => SetCoin(ref _coin10Cent, value, nameof(Coin10Cent), nameof(Total10CentAmount)); }
        public int Coin20Cent { get => _coin20Cent; set => SetCoin(ref _coin20Cent, value, nameof(Coin20Cent), nameof(Total20CentAmount)); }
        public int Coin50Cent { get => _coin50Cent; set => SetCoin(ref _coin50Cent, value, nameof(Coin50Cent), nameof(Total50CentAmount)); }
        public int Coin1Euro { get => _coin1Euro; set => SetCoin(ref _coin1Euro, value, nameof(Coin1Euro), nameof(Total1EuroAmount)); }
        public int Coin2Euro { get => _coin2Euro; set => SetCoin(ref _coin2Euro, value, nameof(Coin2Euro), nameof(Total2EuroAmount)); }

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
        public int Bill5Euro { get => _bill5Euro; set => SetBill(ref _bill5Euro, value, nameof(Bill5Euro), nameof(Total5EuroAmount)); }
        public int Bill10Euro { get => _bill10Euro; set => SetBill(ref _bill10Euro, value, nameof(Bill10Euro), nameof(Total10EuroAmount)); }
        public int Bill20Euro { get => _bill20Euro; set => SetBill(ref _bill20Euro, value, nameof(Bill20Euro), nameof(Total20EuroAmount)); }
        public int Bill50Euro { get => _bill50Euro; set => SetBill(ref _bill50Euro, value, nameof(Bill50Euro), nameof(Total50EuroAmount)); }
        public int Bill100Euro { get => _bill100Euro; set => SetBill(ref _bill100Euro, value, nameof(Bill100Euro), nameof(Total100EuroAmount)); }
        public int Bill200Euro { get => _bill200Euro; set => SetBill(ref _bill200Euro, value, nameof(Bill200Euro), nameof(Total200EuroAmount)); }
        public int Bill500Euro { get => _bill500Euro; set => SetBill(ref _bill500Euro, value, nameof(Bill500Euro), nameof(Total500EuroAmount)); }
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
        public decimal TotalCashAmount => TotalCoinAmount + TotalBillAmount;
        public decimal DifferenceAmount => TotalCashAmount - CashShift?.DrawerBalance ?? 0.00m;

        public CashShiftDTO? CashShift { 
            get => _cashShift; 
            set {
                _cashShift = value;
                OnPropertyChanged(nameof(DifferenceAmount));
            }
        }

        public CashShiftCloseResultDto? CashShiftCloseResult
        {
            get => _cashShiftCloseResult;
            set { _cashShiftCloseResult = value; }
        }
        #region Command properties
        public ICommand NavigateDashboardCommand { get; }
        public ICommand CompareButtonCommand { get; }
        public ICommand EndShiftButton { get; }
        public ICommand UpArrowButtonCommand { get; }
        public ICommand DownArrowButtonCommand { get; }
        #endregion
        #endregion

        #region Constructor
        public VmCashRegister(NavigationStore navigationStore)
        {
            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));

            _cashRegisterRequest = new CashRegisterRequest(AppConfig.ApiUrl);
            _drawerRequests = new DrawerRequests(AppConfig.ApiUrl);

            //CompareButtonCommand = new ButtonCommand(async _ => await CompareCash());
            GetCashShift();
            EndShiftButton = new ButtonCommand(async _ => await EndShift());
            UpArrowButtonCommand = new ButtonCommand(IncreaseValue);
            DownArrowButtonCommand = new ButtonCommand(DecreaseValue);

        }
        #endregion

        #region Methods
        private void IncreaseValue(object obj)
        {
            if(obj is string propertyName)
            {
                //get the property info for the given property name
                PropertyInfo propertyInfo = GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
                if(propertyInfo != null && propertyInfo.CanWrite && propertyInfo.PropertyType == typeof(int))
                {
                    //get current property value
                    int currentValue = (int)propertyInfo.GetValue(this);

                    //set new value by increasing current value by 1
                    propertyInfo.SetValue(this, currentValue + 1);
                }
            }
        }
        private void DecreaseValue(object obj)
        {
            if(obj is string propertyName)
            {
                PropertyInfo propertyInfo = GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
                if(propertyInfo != null && propertyInfo.CanWrite && propertyInfo.PropertyType == typeof(int))
                {
                    int currentValue = (int)propertyInfo.GetValue(this);

                    if(currentValue > 0)
                        propertyInfo.SetValue(this, currentValue - 1);
                }
            }
        }

        private async Task GetCashShift()
        {
            if (CashShift is null || CashShift.CashShiftId == 0)
                CashShift = await _drawerRequests.GetCashShiftTodayByEmployeeId(UserSession.IdUSer);

            OnPropertyChanged(nameof(DifferenceAmount));
        }
        private async Task EndShift()
        {
            await GetCashShift();
            if(CashShift is null || CashShift.ShiftEnd != null)
            {
                MessageBox.Show("No cash shift found, please start a shift first");
                return;
            }
            if (DifferenceAmount != 0.00m)
            {
                if (MessageBox.Show($"Are you shure you want to end your shift with a difference of {DifferenceAmount} €",
                    "End Shift Confirmation",
                    MessageBoxButton.YesNo)
                    == MessageBoxResult.No
                ) return;
            }
            CashShiftCloseResult = await _cashRegisterRequest.PostEndShiftAsync(TotalCashAmount, CashShift.CashShiftId);
            if (CashShiftCloseResult.Difference == 0)
                MessageBox.Show("Your shift has ended with no difference");
            else
                MessageBox.Show($"your shift has ended with a difference of {CashShiftCloseResult.Difference} Euro");
        }

        private void SetCoin(ref int field, int value, string propertyName, string totalAmountProperty)
        {
            if(field != value)
            {
                field = value;
                OnPropertyChanged(propertyName);
                OnPropertyChanged(totalAmountProperty);
                OnPropertyChanged(nameof(TotalCoinAmount));
                OnPropertyChanged(nameof(TotalCashAmount));
                OnPropertyChanged(nameof(DifferenceAmount));
            }
        }

        private void SetBill(ref int field, int value, string propertyName, string totalAmountProperty)
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(propertyName);
                OnPropertyChanged(totalAmountProperty);
                OnPropertyChanged(nameof(TotalBillAmount));
                OnPropertyChanged(nameof(TotalCashAmount));
                OnPropertyChanged(nameof(DifferenceAmount));
            }
        }
        #endregion
    }
}
