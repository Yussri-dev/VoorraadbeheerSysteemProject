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
            if(_selectedStartDate > _selectedEndDate)
            {
                MessageBox.Show("the start date cannot be on a later day then the end date");
                Reset(new object());
                return;
            }
            if(IsPurchaseActive)//purchase
            {
                Purchases = new ObservableCollection<PurchaseFlatDTO>(await _purchasesRequests.GetPurchaseFlatByPeriodAsync(_selectedStartDate, _selectedEndDate));
                if (Purchases.Count == 0)
                {
                    MessageBox.Show("No purchases found for the selected period.");
                    Reset(new object());
                    return;
                }
                FilteredPurchases = Purchases;
            }
            else if(IsSaleActive)//sale
            {
                Sales = new ObservableCollection<SaleFlatDTO>(await _salesRequests.GetSaleFlatByPeriodAsync(_selectedStartDate, _selectedEndDate));
                if (Sales.Count == 0) 
                { 
                    MessageBox.Show("No sales found for the selected period.");
                    Reset(new object());
                    return;
                }
                FilteredSales = Sales;
            }

        }
        #endregion
        #endregion
    }
}


