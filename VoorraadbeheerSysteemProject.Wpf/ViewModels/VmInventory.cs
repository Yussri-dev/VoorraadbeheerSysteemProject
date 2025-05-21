using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
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
using VoorraadbeheerSysteemProject.Wpf.Services.Purchases;
using VoorraadbeheerSysteemProject.Wpf.Services.Sales;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.Views.Printing;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmInventory : VmBase
    {
        private readonly ApiService _apiService;

        //Purchase
        private PurchasesRequests _purchasesRequests;
        private ObservableCollection<PurchaseFlatDTO> _purchases;
        private ObservableCollection<PurchaseFlatDTO> _filteredPurchases;

        //Sale
        private SalesRequests _salesRequests;
        private ObservableCollection<SaleFlatDTO> _sales;
        private ObservableCollection<SaleFlatDTO> _filteredSales;

        //search / filter
        private int _selectedTypeIndex = 0; //0: purchase, 1: sale
        private DateTime _selectedStartDate = DateTime.Now.Date.AddDays(-1);
        private DateTime _selectedEndDate = DateTime.Now.Date;
        private string _searchTextName;
        private string _searchTextBarcode;


        #region Properties
        #region Command properties
        public ICommand SearchButtonCommand { get; }
        public ICommand ResetButtonCommand { get; }
        public ICommand PrintButtonCommand { get; }
        public ICommand NavigateDashboardCommand { get; }
        #endregion


        #region Purchase properties
        public ObservableCollection<PurchaseFlatDTO> Purchases
        { 
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
        public decimal PurchaseTotal => FilteredPurchases.Sum(p => p.TotalAmount);

        #endregion

        #region Sale properties
        public ObservableCollection<SaleFlatDTO> Sales
        {
            get => _sales;
            set
            {
                _sales = value;
                OnPropertyChanged(nameof(Sales));
            }
        }

        public ObservableCollection<SaleFlatDTO> FilteredSales
        {
            get => _filteredSales;
            set
            {
                _filteredSales = value;
                OnPropertyChanged(nameof(FilteredSales));
            }
        }
        #endregion

        #region Search/filter text properties
        public int SelectedTypeIndex
        {
            get => _selectedTypeIndex;
            set {
                _selectedTypeIndex = value;
                OnPropertyChanged(nameof(SelectedTypeIndex));
                OnPropertyChanged(nameof(IsSaleActive));
                OnPropertyChanged(nameof(IsPurchaseActive));
                LoadDataAsync();
            }
        }
        public bool IsSaleActive => SelectedTypeIndex == 1;
        public bool IsPurchaseActive => SelectedTypeIndex == 0;
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

            _apiService = new ApiService(AppConfig.ApiUrl);
            _salesRequests = new SalesRequests(AppConfig.ApiUrl);
            _purchasesRequests = new PurchasesRequests(AppConfig.ApiUrl);

            Task.Run(LoadDataAsync);

            //initialize the commands
            SearchButtonCommand = new ButtonCommand(async _ => await LoadDataAsync());
            ResetButtonCommand = new ButtonCommand(Reset);
            PrintButtonCommand = new ButtonCommand(Print);
        }
        #endregion

        #region Methods
        #region Filter methods
        private void FilterListView()
        {
            if (IsPurchaseActive)
            {
                var filteredPurchases = Purchases.AsEnumerable();

                if (!String.IsNullOrWhiteSpace(SearchTextName))
                    filteredPurchases = filteredPurchases.Where(p => 
                    p.ProductName.ToLower().Contains(SearchTextName.ToLower()));

                if (!String.IsNullOrWhiteSpace(SearchTextBarcode))
                    filteredPurchases = filteredPurchases.Where(p => 
                    p.Barcode.ToLower().Contains(SearchTextBarcode.ToLower()));

                FilteredPurchases = new ObservableCollection<PurchaseFlatDTO>(filteredPurchases);
            }

            if (IsSaleActive)
            {
                var filteredSales = Sales.AsEnumerable();

                if (!String.IsNullOrWhiteSpace(SearchTextName))
                    filteredSales = filteredSales.Where(s => 
                    s.ProductName.ToLower().Contains(SearchTextName.ToLower()));

                if (!String.IsNullOrWhiteSpace(SearchTextBarcode))
                    filteredSales = filteredSales.Where(s => 
                    s.Barcode.ToLower().Contains(SearchTextBarcode.ToLower()));

                FilteredSales = new ObservableCollection<SaleFlatDTO>(filteredSales);
            }



        }
        #endregion

        #region Command methods

        private void Reset(object obj)
        {
            SelectedStartDate = DateTime.Now.Date.AddDays(-1);
            SelectedEndDate = DateTime.Now.Date;
        }

        private void Print(object obj)
        {
            UserControl? printView = null;

            if(IsPurchaseActive)
            {
                printView = new PrintPurchases
                {
                    DataContext = this
                };
            }
            else if (IsSaleActive)
            {
                printView = new PrintSales
                {
                    DataContext = this
                };
            }

            if (printView is null) return;

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
            if(IsPurchaseActive)//purchase
            {
                Purchases = new ObservableCollection<PurchaseFlatDTO>(await _purchasesRequests.GetPurchaseFlatByPeriodAsync(_selectedStartDate, _selectedEndDate));
                if(Purchases.Count == 0) MakePurchaseItemDummyData();
                FilteredPurchases = Purchases;
            }
            else if(IsSaleActive)//sale
            {
                Sales = new ObservableCollection<SaleFlatDTO>(await _salesRequests.GetSalesFlatAsync());
                if (Sales.Count == 0) MakeSaleItemDummyData();
                FilteredSales = Sales;
            }

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

        private void MakeSaleItemDummyData()
        {
            Sales = new ObservableCollection<SaleFlatDTO>
            {
                new SaleFlatDTO { SaleItemId = 1, ProductName = "Product 1", SalePrice1 = 10, TaxAmount = 21, SaleDate = DateTime.Now, QuantityStock = 100, Barcode = "1234567891234" },
                new SaleFlatDTO { SaleItemId = 2, ProductName = "Product 2", SalePrice1 = 25, TaxAmount = 6, SaleDate = DateTime.Now, QuantityStock = 64, Barcode = "1234567891234" },
                new SaleFlatDTO { SaleItemId = 3, ProductName = "Product 3", SalePrice1 = 5, TaxAmount = 12, SaleDate = DateTime.Now, QuantityStock = 256, Barcode = "1234567891234" },
                new SaleFlatDTO { SaleItemId = 4, ProductName = "Product 4", SalePrice1 = 15, TaxAmount = 21, SaleDate = DateTime.Now, QuantityStock = 48946, Barcode = "1234567891234" },
                new SaleFlatDTO { SaleItemId = 5, ProductName = "Product 5", SalePrice1 = 30, TaxAmount = 6, SaleDate = DateTime.Now, QuantityStock = 165, Barcode = "1234567891234" },
            };
        }
        #endregion
        #endregion
    }
}


