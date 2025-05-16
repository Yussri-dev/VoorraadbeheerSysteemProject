using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
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
            _apiService = new ApiService(ConfigurationManager.AppSettings.Get("NGrokApiUri"));

            Task.Run(LoadDataAsync);

            
        }
        #endregion

        #region Methods
        private async Task LoadDataAsync()
        {
            Purchases = new ObservableCollection<PurchaseItemDTO>(await _apiService.GetPurchaseItemsAsync());

            if(Purchases.Count == 0)
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
        }
        #endregion
    }
}


