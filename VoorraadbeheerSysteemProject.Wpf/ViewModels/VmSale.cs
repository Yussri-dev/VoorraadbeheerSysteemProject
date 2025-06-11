using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Commands.ProductsCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Requests;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Services.Customers;
using VoorraadbeheerSysteemProject.Wpf.Services.Sales;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.Views;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmSale : VmBase
    {
        #region Fields & Constructors
        private readonly ApiService _apiService;
        private readonly SalesRequests _salesRequest;
        private readonly CustomersRequests _customerRequest;
        private readonly NavigationStore _navigationStore;
        private int countSales = 0;

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

        //adding data to Products Cart
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
        public VmSale(NavigationStore navigationStore)
        {
            //for close button
            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));

            _navigationStore = navigationStore;

            _apiService = new ApiService(AppConfig.ApiUrl);
            _salesRequest = new SalesRequests(AppConfig.ApiUrl);
            _customerRequest = new CustomersRequests(AppConfig.ApiUrl);
            //Using Observable collection to get products
            _allProducts = new ObservableCollection<ProductDTO>();
            Products = new ObservableCollection<ProductDTO>();

            //All Commands initialisation
            InitialCommands();

            //using NumpadDataEntry
            NumPadViewModel = new VmNumPadDataEntry(this);

            //Loading data from Get Product Request
            Task.Run(async () => await LoadDataAsync());

        }


        #endregion

        #region Formatted Numbers
        //formatting Data to show it in UcSale Views
        public string FormattedSalesCount => countSales.ToString("N0");
        public string FormattedTotalAmount => TotalAmount.ToString("C");
        public string FormattedTotalQuantity => TotalQuantity.ToString("C");
        public string FormattedCountLine => LineCount.ToString("N0");
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
        //--------------------------------------------------
        //CustomerDTO

        private ObservableCollection<CustomerDTO> _customers;
        private ObservableCollection<CustomerDTO> _filteredCustomer;
        private string _searchTextCustomer;
        public string SearchTextCustomer
        {
            get => _searchTextCustomer;
            set
            {
                _searchTextCustomer = value;
                OnPropertyChanged();
                FilterCustomer();
            }
        }
        #region shelf properties
        public ObservableCollection<CustomerDTO> FilteredCustomer
        {
            get => _filteredCustomer;
            set { _filteredCustomer = value; OnPropertyChanged(); }
        }

        private CustomerDTO? _selectedCustomer = null;

        public CustomerDTO? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                    OnPropertyChanged(nameof(SelectedCustomer));

                    if (_selectedCustomer?.Name != SearchTextCustomer)
                        _searchTextCustomer = _selectedCustomer?.Name ?? "";

                    OnPropertyChanged(nameof(SearchTextCustomer));
                }
            }
        }



        public ObservableCollection<CustomerDTO> AllCustomer
        {
            get => _customers;
            set { _customers = value; OnPropertyChanged(); }
        }
        #endregion
        public void FilterCustomer()
        {
            if (string.IsNullOrWhiteSpace(_searchTextCustomer))
                FilteredCustomer = new ObservableCollection<CustomerDTO>(
                    _customers ?? new ObservableCollection<CustomerDTO>());
            else
            {
                FilteredCustomer = new ObservableCollection<CustomerDTO>(
                    _customers
                    .Where(
                        items => items.Name.ToLower()
                        .Contains(_searchTextCustomer.ToLower())
                    )
                    .ToList()
                );
            }
        }

        //----------------------------------------------------
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

        //Englobe commands in one Methods
        private void InitialCommands()
        {
            OpenDialogCommand = new OpenDialogCommand(this, NumPadViewModel);
            AddSelectedProductCommand = new AddSelectedProductCommand(this, NumPadViewModel);
            RemoveSelectedProductCommand = new RemoveSelectedProductCommand(this);
            ClearSelectedProductCommand = new ClearSelectedProductCommand(this);

            IncrementQuantityCommand = new ButtonCommand(param => IncrementQuantity(param));
            DecrementQuantityCommand = new ButtonCommand(param => DecrementQuantity(param));

        }

        //Method for Searching and Filtering products using Name or barcode
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

        //getting Products from ApiService
        private async Task LoadDataAsync()
        {
            var products = await _apiService.GetProductsAsync();
            var customers = await _customerRequest.GetCustomers();
            countSales = await _salesRequest.GetSalesCountAsync() + 1;
            OnPropertyChanged(nameof(FormattedSalesCount));

            App.Current.Dispatcher.Invoke(() =>
            {
                _allProducts = new ObservableCollection<ProductDTO>(products);
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
                AllCustomer = new ObservableCollection<CustomerDTO>(customers);
                FilteredCustomer = AllCustomer;

            });
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

                // Ajout automatique si un code-barres complet est scanné
                if (!string.IsNullOrWhiteSpace(_inputSearchBarcodeText) && _inputSearchBarcodeText.Length >= 6)
                {
                    AddProductByBarcode();
                }
            }
        }

        public void AddProductByBarcode()
        {
            if (string.IsNullOrWhiteSpace(InputSearchBarcodeText))
                return;

            var product = _allProducts.FirstOrDefault(p => p.Barcode == InputSearchBarcodeText);
            if (product == null)
                return;

            var existing = SelectedProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existing == null)
            {
                decimal totalPrice = product.SalePrice1;
                decimal taxAmount = Math.Round(product.TaxRate * totalPrice, 2);

                SelectedProducts.Add(new ProductSelectedRequest
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Quantity = 1,
                    SalePrice = product.SalePrice1,
                    AmountPrice = totalPrice,
                    PurchasePrice = product.PurchasePrice,
                    TaxAmount = taxAmount
                });
            }
            else
            {
                existing.Quantity += 1;
                existing.AmountPrice += product.SalePrice1;
            }

            InputSearchBarcodeText = string.Empty;
            CalculateTotalAmount();
        }

        private void IncrementQuantity(object param)
        {
            if (param is ProductSelectedRequest item)
            {
                item.Quantity += 1;
                item.AmountPrice = item.Quantity * item.SalePrice;
                CalculateTotalAmount();
            }
        }

        private void DecrementQuantity(object param)
        {
            if (param is ProductSelectedRequest item && item.Quantity > 1)
            {
                item.Quantity -= 1;
                item.AmountPrice = item.Quantity * item.SalePrice;
                CalculateTotalAmount();
            }
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

        #endregion

        #region commands
        public ICommand NavigateDashboardCommand { get; set; }
        public ICommand AddSelectedProductCommand { get; set; }
        public ICommand RemoveSelectedProductCommand { get; set; }
        public ICommand ClearSelectedProductCommand { get; set; }
        public ICommand OpenDialogCommand { get; set; }
        public ICommand SearchProductsCommand { get; set; }
        public ICommand IncrementQuantityCommand { get; set; }
        public ICommand DecrementQuantityCommand { get; set; }

        #endregion

    }
}
