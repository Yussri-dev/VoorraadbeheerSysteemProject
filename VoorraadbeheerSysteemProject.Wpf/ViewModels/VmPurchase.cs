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

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmPurchase : VmBase
    {
        #region Fields & Constructors
        private readonly ApiService _apiService;
        private readonly SalesRequests _salesRequest;
        private readonly PurchasesRequests _purchaseRequest;
        private readonly NavigationStore _navigationStore;
        private int countPurchases = 0;
        public VmNumPadDataEntry NumPadViewModel { get; }

        private ProductDTO? _selectedProduct;
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

        private ProductSelectedRequest? _selectedProductInCart;
        public ProductSelectedRequest? SelectedProductInCart
        {
            get => _selectedProductInCart;
            set
            {
                _selectedProductInCart = value;
                OnPropertyChanged(nameof(SelectedProductInCart));
            }
        }
        //Constructor
        public VmPurchase(NavigationStore navigationStore)
        {

            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));

            _navigationStore = navigationStore;
            _apiService = new ApiService("https://localhost:5001/");
            _purchaseRequest = new PurchasesRequests("https://localhost:5001/");

            _allProducts = new ObservableCollection<ProductDTO>();
            Products = new ObservableCollection<ProductDTO>();

            InitialCommands();

            NumPadViewModel = new VmNumPadDataEntry(this);
            Task.Run(async () => await LoadDataAsync());
        }
        public string FormattedPurchaseCount => countPurchases.ToString("N0");

        #endregion

        #region ObservableCollections

        private ObservableCollection<ProductDTO> _allProducts;

        public ObservableCollection<ProductSelectedRequest> SelectedProducts { get; set; } = new();

        private ObservableCollection<ProductDTO> _products;

        public ObservableCollection<ProductDTO> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        #endregion

        #region Methods


        //Calculate The Total of Quantities,Lines,Amount
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
                    total += productDetails.SalePrice1 * product.Quantity;
                    totalQuantity += product.Quantity;
                }
            }

            TotalAmount = total;
            TotalQuantity = totalQuantity;
            LineCount = countLine;
        }

        private void InitialCommands()
        {
            AddSelectedProductCommand = new AddSelectedProductToPurchaseCommand(this, NumPadViewModel);
            RemoveSelectedProductCommand = new RemoveSelectedProductCommand(this);
            ClearSelectedProductCommand = new ClearSelectedProductCommand(this);
            ValidatePurchaseDataCommand = new ValidatePurchaseDataCommand(this);
        }

        public void FilterProducts()
        {
            var query = _allProducts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(InputSearchNameText))
            {
                query = query.Where(p =>
                    p.Name.Contains(InputSearchNameText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(InputSearchBarcodeText))
            {
                query = query.Where(p =>
                    p.Barcode.Contains(InputSearchBarcodeText, StringComparison.OrdinalIgnoreCase));
            }

            Products.Clear();
            foreach (var product in query)
            {
                Products.Add(product);
            }
        }

        private async Task LoadDataAsync()
        {
            var products = await _apiService.GetProductsAsync();

            countPurchases = await _purchaseRequest.GetPurchasesCountAsync();

            OnPropertyChanged(nameof(FormattedPurchaseCount));

            App.Current.Dispatcher.Invoke(() =>
            {
                _allProducts = new ObservableCollection<ProductDTO>(products);
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            });
        }
        #endregion

        #region InputText and Calcul
        private string _inputText = string.Empty;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged(nameof(InputText));
            }
        }

        private string _inputSearchNameText = string.Empty;
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

        private string _inputSearchBarcodeText = string.Empty;
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

        //Total Amount
        private decimal _totalAmount;
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

        public string FormattedTotalAmount => TotalAmount.ToString("C");

        // Total Quantity
        private decimal _totalQuantity;
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
        public string FormattedTotalQuantity => TotalQuantity.ToString("C");

        //Total Lines
        private int _lineCount;
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
        public string FormattedCountLine => LineCount.ToString("N0");

        #endregion

        #region commands
        public ICommand ValidatePurchaseDataCommand { get; set; }
        public ICommand NavigateDashboardCommand { get; set; }
        public ICommand AddSelectedProductCommand { get; set; }
        public ICommand RemoveSelectedProductCommand { get; set; }
        public ICommand ClearSelectedProductCommand { get; set; }
        public ICommand OpenDialogCommand { get; set; }
        public ICommand SearchProductsCommand { get; set; }
        #endregion

        #region Post Purchases

        public async Task<PurchaseDTO?> SavePurchaseAsync(PurchaseDTO purchaseDto)
        {
            try
            {
                var createdPurchase = await _purchaseRequest.PostPurchaseAsync(purchaseDto);

                if (createdPurchase != null)
                {
                    return createdPurchase;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<PurchaseItemDTO?> SavePurchaseItemAsync(PurchaseItemDTO purchaseItemDto)
        {
            try
            {
                var createdPurchase = await _purchaseRequest.PostPurchaseItemAsync(purchaseItemDto);

                if (createdPurchase != null)
                {
                    return createdPurchase;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
