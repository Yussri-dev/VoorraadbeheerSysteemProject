using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Printing;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Xps;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.Views.Printing;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmInventory : VmBase
    {
        private readonly ApiService _apiService;

        //PurchaseItemDTO
        private ObservableCollection<PurchaseFlatDTO> _purchases;
        private ObservableCollection<PurchaseFlatDTO> _filteredPurchases;

        //search / filter
        private DateTime _selectedStartDate = DateTime.Now.Date;
        private DateTime _selectedEndDate = DateTime.Now.Date;
        private string _searchTextName;
        private string _searchTextBarcode;


        #region Properties
        #region Command properties
        public ICommand PreviousPageButtonCommand { get; }
        public ICommand NextPageButtonCommand { get; }
        public ICommand ResetButtonCommand { get; }
        public ICommand PrintButtonCommand { get; }
        public ICommand NavigateDashboardCommand { get; }
        #endregion


        #region Purchase properties
        public ObservableCollection<PurchaseFlatDTO> Purchases { 
            get => _purchases; 
            set {
                _purchases = value;
                OnPropertyChanged(nameof(Purchases));
            } 
        }
        public ObservableCollection<PurchaseFlatDTO> FilteredPurchases
        {
            get => _filteredPurchases;
            set
            {
                _filteredPurchases = value;
                OnPropertyChanged(nameof(FilteredPurchases));
            }
        }
        #endregion

        #region Search/filter text properties
        public DateTime SelectedStartDate
        {
            get => _selectedStartDate;
            set {
                _selectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
            }
        }

        public DateTime SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                _selectedEndDate = value;
                OnPropertyChanged(nameof(SelectedEndDate));
            }
        }

        public string SearchTextName
        {
            get { return _searchTextName; }
            set {
                _searchTextName = value;
                OnPropertyChanged(nameof(SearchTextName));
                FilterListView();
            }
        }

        public string SearchTextBarcode
        {
            get { return _searchTextBarcode; }
            set
            {
                _searchTextBarcode = value;
                OnPropertyChanged(nameof(SearchTextBarcode));
                FilterListView();
            }
        }

        #endregion

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
        #region Filter methods
        private void FilterListView()
        {
            var filteredPurchases = Purchases.AsEnumerable();

            //TODO: check what is selected purchase or sale

            if (!String.IsNullOrWhiteSpace(SearchTextName))
                filteredPurchases = filteredPurchases.Where(p => p.ProductName.ToLower().Contains(SearchTextName.ToLower()));

            if (!String.IsNullOrWhiteSpace(SearchTextBarcode))
                filteredPurchases = filteredPurchases.Where(p => p.Barcode.ToLower().Contains(SearchTextBarcode.ToLower()));

            FilteredPurchases = new ObservableCollection<PurchaseFlatDTO>(filteredPurchases);

        }
        #endregion

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
            var printView = new PrintPurchases
            {
                DataContext = this
            };

            PrintDialog printDialog = new PrintDialog();


            if(printDialog.ShowDialog() == true)
            {
                PrintTicket printTicket = printDialog.PrintTicket;
                PageMediaSize pageMediaSize = printTicket.PageMediaSize ?? new PageMediaSize(PageMediaSizeName.ISOA4);


                double pageWidth = pageMediaSize.Width ?? printDialog.PrintableAreaWidth;
                double pageHeight = pageMediaSize.Height ?? printDialog.PrintableAreaHeight;

                if(printTicket.PageOrientation == PageOrientation.Landscape)
                {
                    pageWidth = pageMediaSize.Height ?? printDialog.PrintableAreaHeight;
                    pageHeight = pageMediaSize.Width ?? printDialog.PrintableAreaWidth;
                }

                printView.Measure(new Size(pageWidth, pageHeight));
                printView.Arrange(new Rect(0, 0, pageWidth, pageHeight));
                printView.UpdateLayout();

                PrintQueue printQueue = printDialog.PrintQueue;
                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
                writer.Write(printView, printTicket);
            }
        }
        #endregion


        #region LoadData
        private async Task LoadDataAsync()
        {
            Purchases = new ObservableCollection<PurchaseFlatDTO>(await _apiService.GetPurchasesFlatAsync());

            if(Purchases.Count == 0) MakePurchaseItemDummyData();

            FilteredPurchases = Purchases;
        }
        private void MakePurchaseItemDummyData()
        {
            //fill purchase list with dummy data
            Purchases = new ObservableCollection<PurchaseFlatDTO>
                {
                    new PurchaseFlatDTO{ PurchaseItemId = 1, ProductName = "Product 1", Price = 10, SalePrice1 = 15, TaxAmount = 21, SupplierName = "Supplier 1", PurchaseDate = DateTime.Now, QuantityStock = 100, Barcode = "1234567891234" },
                    new PurchaseFlatDTO{ PurchaseItemId = 2, ProductName = "Product 2", Price = 25, SalePrice1 = 30, TaxAmount = 6, SupplierName = "Supplier 2", PurchaseDate = DateTime.Now, QuantityStock = 64, Barcode = "1234567891234"  },
                    new PurchaseFlatDTO{ PurchaseItemId = 3, ProductName = "Product 3", Price = 5, SalePrice1 = 10, TaxAmount = 12, SupplierName = "supplier 2", PurchaseDate = DateTime.Now, QuantityStock = 256, Barcode = "1234567891234"  },
                    new PurchaseFlatDTO{ PurchaseItemId = 4, ProductName = "Product 4", Price = 15, SalePrice1 = 20, TaxAmount = 21, SupplierName = "supplier 3", PurchaseDate = DateTime.MinValue, QuantityStock = 48946, Barcode = "1234567891234"  },
                    new PurchaseFlatDTO{ PurchaseItemId = 5, ProductName = "Product 5", Price = 30, SalePrice1 = 40, TaxAmount = 6, SupplierName = "supplier 1", PurchaseDate = DateTime.MaxValue, QuantityStock = 165, Barcode = "1234567891234"  },
                };
        }
        #endregion
        #endregion
    }
}


