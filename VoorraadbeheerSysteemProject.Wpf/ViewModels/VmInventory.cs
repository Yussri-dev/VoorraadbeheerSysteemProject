using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmInventory : VmBase
    {
        private readonly ApiService _apiService;

        //PurchaseItemDTO
        private ObservableCollection<PurchaseFlatDTO> _purchases;


        #region Properties
        #region Command properties
        public ICommand PreviousPageButtonCommand { get; }
        public ICommand NextPageButtonCommand { get; }
        public ICommand ResetButtonCommand { get; }
        public ICommand PrintButtonCommand { get; }
        public ICommand NavigateDashboardCommand { get; }
        #endregion

        public ObservableCollection<PurchaseFlatDTO> Purchases { 
            get => _purchases; 
            set {
                _purchases = value;
                OnPropertyChanged(nameof(Purchases));
            } 
        }
        

        #endregion

        #region Constructors
        public VmInventory(NavigationStore navigationStore)
        {
            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));
            _apiService = new ApiService(ConfigurationManager.AppSettings.Get("NGrokApiUri"));

            Task.Run(LoadDataAsync);

            //initialize the commands
            PreviousPageButtonCommand = new ButtonCommand(PreviousPage);
            NextPageButtonCommand = new ButtonCommand(NextPage);
            ResetButtonCommand = new ButtonCommand(Reset);
            PrintButtonCommand = new ButtonCommand(Print);
        }
        #endregion

        #region Methods


        #region Command methods
        private async void PreviousPage(object obj)
        {
            throw new NotImplementedException();
        }

        private void NextPage(object obj)
        {
            throw new NotImplementedException();
        }

        private void Reset(object obj)
        {
            throw new NotImplementedException();
        }

        private void Print(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region LoadData
        private async Task LoadDataAsync()
        {
            Purchases = new ObservableCollection<PurchaseFlatDTO>(await _apiService.GetPurchasesFlatAsync());

            if(Purchases.Count == 0) MakePurchaseItemDummyData();
        }
        private void MakePurchaseItemDummyData()
        {
            //fill purchase list with dummy data
            Purchases = new ObservableCollection<PurchaseFlatDTO>
                {

                    new PurchaseFlatDTO{ PurchaseItemId = 1, ProductName = "Product 1", Price = 10, SalePrice1 = 15, TaxAmount = 21, SupplierName = "Supplier 1", PurchaseDate = DateTime.Now, QuantityStock = 100 },
                    new PurchaseFlatDTO{ PurchaseItemId = 2, ProductName = "Product 2", Price = 25, SalePrice1 = 30, TaxAmount = 6, SupplierName = "Supplier 2", PurchaseDate = DateTime.Now, QuantityStock = 64  },
                    new PurchaseFlatDTO{ PurchaseItemId = 3, ProductName = "Product 3", Price = 5, SalePrice1 = 10, TaxAmount = 12, SupplierName = "supplier 2", PurchaseDate = DateTime.Now, QuantityStock = 256  },
                    new PurchaseFlatDTO{ PurchaseItemId = 4, ProductName = "Product 4", Price = 15, SalePrice1 = 20, TaxAmount = 21, SupplierName = "supplier 3", PurchaseDate = DateTime.MinValue, QuantityStock = 48946  },
                    new PurchaseFlatDTO{ PurchaseItemId = 5, ProductName = "Product 5", Price = 30, SalePrice1 = 40, TaxAmount = 6, SupplierName = "supplier 1", PurchaseDate = DateTime.MaxValue, QuantityStock = 165  },
                };
        }
        #endregion
        #endregion
    }
}


