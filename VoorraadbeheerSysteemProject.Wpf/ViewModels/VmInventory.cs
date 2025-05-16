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
        private ObservableCollection<PurchaseItemDTO> _purchases;


        #region Properties
        #region Command properties
        public ICommand PreviousPageButtonCommand { get; }
        public ICommand NextPageButtonCommand { get; }
        public ICommand ResetButtonCommand { get; }
        public ICommand PrintButtonCommand { get; }
        public ICommand NavigateDashboardCommand { get; }
        #endregion

        public ObservableCollection<PurchaseItemDTO> Purchases { 
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
            Purchases = new ObservableCollection<PurchaseItemDTO>(await _apiService.GetPurchaseItemsAsync());

            if(Purchases.Count == 0) MakePurchaseItemDummyData();
        }
        private void MakePurchaseItemDummyData()
        {
            //fill purchase list with dummy data
            Purchases = new ObservableCollection<PurchaseItemDTO>
                {

                    new PurchaseItemDTO
                    {
                        PurchaseItemId = 1,
                        ProductName = "Product 1", //from ProductDTO
                        Price = 10,
                        TaxAmount = 21,

                        //SupplierName = "Supplier 1", //from PurchaseDTO
                        //PurchaseDate = DateTime.Now, //from PurchaseDTO
                        //Stock = 100  //from ProductDTO
                    },
                    new PurchaseItemDTO{ PurchaseId = 2, ProductName = "Product 2", Price = 25, TaxAmount = 6  },
                    new PurchaseItemDTO{ PurchaseId = 3, ProductName = "Product 3", Price = 5, TaxAmount = 12  },
                    new PurchaseItemDTO{ PurchaseId = 4, ProductName = "Product 4", Price = 15, TaxAmount = 21  },
                    new PurchaseItemDTO{ PurchaseId = 5, ProductName = "Product 5", Price = 30, TaxAmount = 6  },
                };
        }
        #endregion
        #endregion
    }
}


