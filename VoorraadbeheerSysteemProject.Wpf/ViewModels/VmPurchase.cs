using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands.ProductsCommands;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Requests;
using VoorraadbeheerSysteemProject.Wpf.Services.Sales;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.Commands.PurchasesCommands;
using VoorraadbeheerSysteemProject.Wpf.Services.Purchases;
using VoorraadbeheerSysteemProject.Wpf.Services.Suppliers;
using VoorraadbeheerSysteemProject.Wpf.Services.Customers;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    // ViewModel for handling product purchases in the application
    class VmPurchase : VmBase
    {
        #region Fields


        // Service for generic API calls
        private readonly ApiService _apiService;
        // Handles navigation between views
        private readonly NavigationStore _navigationStore;

        //Handles Sales Http Requests
        private readonly SalesRequests _salesRequest;
        private readonly PurchasesRequests _purchaseRequest;
        private readonly SupplierRequests _supplierRequests;
        // Stores the number of purchases
        private int countPurchases = 0;

        // All products retrieved from API
        private ObservableCollection<ProductDTO> _allProducts;
        // Filtered/displayed products
        private ObservableCollection<ProductDTO> _products;

        // Selected product from list
        private ProductDTO? _selectedProduct;
        // Selected product in purchase cart
        private ProductSelectedRequest? _selectedProductInCart;

        //Input Fields
        private string _inputText = string.Empty;
        private string _inputSearchNameText = string.Empty;
        private string _inputSearchBarcodeText = string.Empty;

        // Totals
        private decimal _totalAmount;
        private decimal _totalQuantity;
        private int _lineCount;

        #endregion

        #region Constructor
        //Constructor
        public VmPurchase(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _apiService = new ApiService(AppConfig.ApiUrl);
            _purchaseRequest = new PurchasesRequests(AppConfig.ApiUrl);

            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(
                navigationStore, () => new VmDashboard(navigationStore));

            _allProducts = new ObservableCollection<ProductDTO>();
            _supplierRequests = new SupplierRequests(AppConfig.ApiUrl);
            Products = new ObservableCollection<ProductDTO>();

            NumPadViewModel = new VmNumPadDataEntry(this);

            InitialCommands();
            Task.Run(async () => await LoadDataAsync());
        }

        #endregion

        #region Properties - Products & Selection

        public ObservableCollection<ProductDTO> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        public ObservableCollection<ProductSelectedRequest> SelectedProducts { get; set; } = new();

        public ProductDTO? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                (AddSelectedProductCommand as AddSelectedProductCommand)?.RaiseCanExecuteChanged();
                (RemoveSelectedProductCommand as RemoveSelectedProductCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ProductSelectedRequest? SelectedProductInCart
        {
            get => _selectedProductInCart;
            set
            {
                _selectedProductInCart = value;
                OnPropertyChanged(nameof(SelectedProductInCart));
            }
        }

        public VmNumPadDataEntry NumPadViewModel { get; }

        #endregion

        #region Properties - Input Fields

        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged(nameof(InputText));
            }
        }

        public string InputSearchNameText
        {
            get => _inputSearchNameText;
            set
            {
                _inputSearchNameText = value;
                OnPropertyChanged(nameof(InputSearchNameText));
                FilterProducts();
            }
        }

        public string InputSearchBarcodeText
        {
            get => _inputSearchBarcodeText;
            set
            {
                _inputSearchBarcodeText = value;
                OnPropertyChanged(nameof(InputSearchBarcodeText));
                FilterProducts();
            }
        }

        #endregion

        #region Properties - Totals

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
                OnPropertyChanged(nameof(FormattedTotalAmount));
            }
        }
        public decimal TotalQuantity
        {
            get => _totalQuantity;
            set
            {
                _totalQuantity = value;
                OnPropertyChanged(nameof(TotalQuantity));
                OnPropertyChanged(nameof(FormattedTotalQuantity));
            }
        }
        public int LineCount
        {
            get => _lineCount;
            private set
            {
                _lineCount = value;
                OnPropertyChanged(nameof(LineCount));
                OnPropertyChanged(nameof(FormattedCountLine));
            }
        }

        public string FormattedTotalAmount => TotalAmount.ToString("C");
        public string FormattedTotalQuantity => TotalQuantity.ToString("C");
        public string FormattedCountLine => LineCount.ToString("N0");
        public string FormattedPurchaseCount => countPurchases.ToString("N0");

        #endregion

        #region Commands

        public ICommand ValidatePurchaseDataCommand { get; set; }
        public ICommand NavigateDashboardCommand { get; set; }
        public ICommand AddSelectedProductCommand { get; set; }
        public ICommand RemoveSelectedProductCommand { get; set; }
        public ICommand ClearSelectedProductCommand { get; set; }
        public ICommand OpenDialogCommand { get; set; }
        public ICommand SearchProductsCommand { get; set; }

        private void InitialCommands()
        {
            AddSelectedProductCommand = new AddSelectedProductToPurchaseCommand(this, NumPadViewModel);
            RemoveSelectedProductCommand = new RemoveSelectedProductCommand(this);
            ClearSelectedProductCommand = new ClearSelectedProductCommand(this);
            ValidatePurchaseDataCommand = new ValidatePurchaseDataCommand(this);
        }

        #endregion

        #region Supplier properties

        private SupplierDTO _selectedSupplier = new SupplierDTO();

        private ObservableCollection<SupplierDTO> _suppliers;
        private ObservableCollection<SupplierDTO> _filteredSupplier;
        private string _searchTextSupplier;
        public string SearchTextSupplier
        {
            get => _searchTextSupplier;
            set
            {
                _searchTextSupplier = value;
                OnPropertyChanged();
                FilterSupplier();
            }
        }
        public ObservableCollection<SupplierDTO> FilteredSupplier
        {
            get => _filteredSupplier;
            set { _filteredSupplier = value; OnPropertyChanged(); }
        }

        public SupplierDTO? SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));

                //update text to match selected product
                SearchTextSupplier = _selectedSupplier?.Name ?? "";
            }
        }

        public ObservableCollection<SupplierDTO> AllSupplier
        {
            get => _suppliers;
            set { _suppliers = value; OnPropertyChanged(); }
        }
        public void FilterSupplier()
        {
            if (string.IsNullOrWhiteSpace(_searchTextSupplier))
                FilteredSupplier = new ObservableCollection<SupplierDTO>(
                    _suppliers ?? new ObservableCollection<SupplierDTO>());
            else
            {
                FilteredSupplier = new ObservableCollection<SupplierDTO>(
                    _suppliers
                    .Where(
                        items => items.Name.ToLower()
                        .Contains(_searchTextSupplier.ToLower())
                    )
                    .ToList()
                );
            }
        }

        #endregion

        #region Methods - Product Logic

        public void FilterProducts()
        {
            var query = _allProducts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(InputSearchNameText))
                query = query.Where(p => p.Name.Contains(InputSearchNameText, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(InputSearchBarcodeText))
                query = query.Where(p => p.Barcode.Contains(InputSearchBarcodeText, StringComparison.OrdinalIgnoreCase));

            Products.Clear();
            foreach (var product in query)
                Products.Add(product);
        }

        public void CalculateTotalAmount()
        {
            decimal total = 0;
            decimal totalQuantity = 0;
            int countLine = SelectedProducts.Count;

            foreach (var product in SelectedProducts)
            {
                var productDetails = _allProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (productDetails != null)
                {
                    total += productDetails.PurchasePrice * product.Quantity;
                    totalQuantity += product.Quantity;
                }
            }

            TotalAmount = total;
            TotalQuantity = totalQuantity;
            LineCount = countLine;
        }

        private async Task LoadDataAsync()
        {
            var products = await _apiService.GetProductsAsync();
            var suppliers = await _supplierRequests.GetSuppliers();
            countPurchases = await _purchaseRequest.GetPurchasesCountAsync();

            OnPropertyChanged(nameof(FormattedPurchaseCount));

            App.Current.Dispatcher.Invoke(() =>
            {
                _allProducts = new ObservableCollection<ProductDTO>(products);
                Products.Clear();
                foreach (var product in products)
                    Products.Add(product);

                AllSupplier = new ObservableCollection<SupplierDTO>(suppliers);
                FilteredSupplier = AllSupplier;

            });

        }

        #endregion

        #region Methods - Save to API

        public async Task<PurchaseDTO?> SavePurchaseAsync(PurchaseDTO purchaseDto)
        {
            try
            {
                var createdPurchase = await _purchaseRequest.PostPurchaseAsync(purchaseDto);
                return createdPurchase;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PurchaseItemDTO?> SavePurchaseItemAsync(PurchaseItemDTO purchaseItemDto)
        {
            try
            {
                var createdPurchase = await _purchaseRequest.PostPurchaseItemAsync(purchaseItemDto);
                return createdPurchase;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
