using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Xps;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.IO.Packaging;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
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
        public DateTime FilteredStartDate { get; set; } = DateTime.Now.Date.AddDays(-1);

        public DateTime SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                _selectedEndDate = value;
                OnPropertyChanged(nameof(SelectedEndDate));
            }
        }
        public DateTime FilteredEndDate { get; set; } = DateTime.Now.Date;

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
            SelectedStartDate = FilteredStartDate;
            SelectedEndDate = FilteredEndDate;
            //Task.Run(LoadDataAsync);
        }

        private void Print(object obj)
        {
            if(SelectedStartDate != FilteredStartDate || SelectedEndDate != FilteredEndDate)
            {
                MessageBox.Show("Warning: The data shown is not equal to the dates!\nDid you forget to click the search button?");
                return;
            }


            UserControl? printView = null;
            string fileName = string.Empty;

            if (IsPurchaseActive)
            {
                printView = new PrintPurchases
                {
                    DataContext = this
                };
                fileName = "Purchases";
            }
            else if (IsSaleActive)
            {
                printView = new PrintSales
                {
                    DataContext = this
                };
                fileName = "Sales";
            }

            fileName = $"{fileName}_{SelectedStartDate.ToString("dd-MM-yyyy")}_{SelectedEndDate.ToString("dd-MM-yyyy")}";


            printView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            printView.Arrange(new Rect(0, 0, printView.DesiredSize.Width, printView.DesiredSize.Height));
            printView.UpdateLayout();

            double A4Width = 1123; //(in pixels at 96 DPI) landscape
            double A4Height = 794; //(in pixels at 96 DPI) landscape
            int totalPages = (int)Math.Ceiling(printView.DesiredSize.Height / A4Height);

            FixedDocument fixedDoc = new FixedDocument();
            fixedDoc.DocumentPaginator.PageSize = new Size(A4Width, A4Height);

            for (int i = 0; i < totalPages; i++)
            {
                FixedPage page = new FixedPage
                {
                    Width = A4Width,
                    Height = A4Height
                };
                
                var visualBrush = new VisualBrush(printView)
                {
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left,
                    AlignmentY = AlignmentY.Top,
                    ViewboxUnits = BrushMappingMode.Absolute,
                    Viewbox = new Rect(0, i * A4Height, A4Width, A4Height),
                };

                var pageContent = new Canvas
                {
                    Width = A4Width,
                    Height = A4Height,
                    Background = visualBrush
                };

                FixedPage.SetLeft(pageContent, 0);
                FixedPage.SetTop(pageContent, 0);
                page.Children.Add(pageContent);

                PageContent pageWrapper = new PageContent();
                ((IAddChild)pageWrapper).AddChild(page);
                fixedDoc.Pages.Add(pageWrapper);
            }


            MemoryStream stream = new MemoryStream();

            Package package = Package.Open(stream, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);


            writer.Write(fixedDoc.DocumentPaginator);

            doc.Close();
            package.Close();

            //convert
            MemoryStream outstream = new MemoryStream();
            XpsConverter.Convert(stream, outstream, false);

            //write pdf file
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Save PDF",
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = $"{fileName}.pdf",
                DefaultExt = ".pdf"
            };

            if (dialog.ShowDialog() == false) return;


            FileStream fileStream = new FileStream(dialog.FileName, FileMode.Create);
            outstream.WriteTo(fileStream);

            //cleanup
            outstream.Flush();
            outstream.Close();
            fileStream.Flush();
            fileStream.Close();
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
            FilteredStartDate = _selectedStartDate;
            FilteredEndDate = _selectedEndDate;
        }
        #endregion
        #endregion
    }
}


