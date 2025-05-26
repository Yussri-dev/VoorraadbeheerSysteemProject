using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmMainWindow : VmBase
    {
        private readonly NavigationStore _navigationStore;

        public VmBase CurrentViewModel => _navigationStore.CurrentViewModel;

        #region Commands
        public ICommand DashboardNavigationCommand { get; }
        public ICommand UserCreateNavigationCommand { get; }
        public ICommand UserLoginNavigationCommand { get; }

        public ICommand SaleNavigationCommand { get; }
        public ICommand PurchaseNavigationCommand { get; }

        public ICommand InventoryNavigationCommand { get; }
        public ICommand ReportNavigationCommand { get; }

        public ICommand SupplierNavigationCommand { get; }
        public ICommand CustomerNavigationCommand { get; }

        public ICommand ProductsNavigationCommand { get; }
        public ICommand CategoryNavigationCommand { get; }
        public ICommand ShelfNavigationCommand { get; }
        public ICommand LineNavigationCommand { get; }
        public ICommand TaxNavigationCommand { get; }

        public ICommand CashRegisterNavigationCommand { get; }
        #endregion


        private string _email;
        public string Email
        {
            get => _email ?? (UserSession.Email ?? "Unknown");
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public VmMainWindow(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;

            LogoutCommand = new ButtonCommand(Logout);
            GetEmailCommand = new ButtonCommand(GetUser);


            ProductsNavigationCommand = new NavigationCommand<VmProducts>(navigationStore,
                () => new VmProducts(navigationStore));

            DashboardNavigationCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));


            UserCreateNavigationCommand = new NavigationCommand<VmUserCreate>(navigationStore,
              () => new VmUserCreate(navigationStore));

            UserLoginNavigationCommand = new NavigationCommand<VmUserLogin>(navigationStore,
                () => new VmUserLogin(navigationStore));

            SaleNavigationCommand = new NavigationCommand<VmSale>(navigationStore,
                () => new VmSale(navigationStore));

            PurchaseNavigationCommand = new NavigationCommand<VmPurchase>(navigationStore,
                () => new VmPurchase(navigationStore));

            InventoryNavigationCommand = new NavigationCommand<VmInventory>(navigationStore,
                () => new VmInventory(navigationStore));

            ReportNavigationCommand = new NavigationCommand<VmReport>(navigationStore,
                () => new VmReport(navigationStore));

            SupplierNavigationCommand = new NavigationCommand<VmSupplier>(navigationStore,
                () => new VmSupplier(navigationStore));

            CustomerNavigationCommand = new NavigationCommand<VmCustomer>(navigationStore,
                () => new VmCustomer(navigationStore));


            CategoryNavigationCommand = new NavigationCommand<VmCategory>(navigationStore,
                () => new VmCategory(navigationStore));

            ShelfNavigationCommand = new NavigationCommand<VmShelf>(navigationStore,
                () => new VmShelf(navigationStore));

            LineNavigationCommand = new NavigationCommand<VmLine>(navigationStore,
                () => new VmLine(navigationStore));

            TaxNavigationCommand = new NavigationCommand<VmTax>(navigationStore,
                () => new VmTax(navigationStore));

            CashRegisterNavigationCommand = new NavigationCommand<VmCashRegister>(navigationStore,
                () => new VmCashRegister(navigationStore));
        }

        private void _navigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ICommand LogoutCommand { get; }
        public ICommand GetEmailCommand { get; }
        private void Logout(object obj)
        {
            UserSession.Clear();
            _navigationStore.CurrentViewModel = new VmUserLogin(_navigationStore);
        }

        private void GetUser(object param)
        {
            Email = UserSession.Email;
        }
    }
}
