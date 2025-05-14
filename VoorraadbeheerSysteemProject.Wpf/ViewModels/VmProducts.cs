using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmProducts : VmBase
    {
        private readonly ApiService _apiService;

        //productDTO
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<ProductDTO> _filteredProducts;
        private ProductDTO? _selectedProduct;

        //categoryDTO
        private ObservableCollection<string> _categories;
        private ObservableCollection<string> _filteredCategories;

        //TaxRateDTO
        private ObservableCollection<string> _taxRates;
        private ObservableCollection<string> _filteredTaxRate;

        //ShelfDTO
        private ObservableCollection<string> _shelfs;
        private ObservableCollection<string> _filteredShelf;


        //search/filter
        private string _searchTextName = "";
        private string _searchTextBarcode = "";
        private string _searchTextTaxRate;
        private string _searchTextShelf;
        private string _searchTextCategories;


        #region properties
            #region command properties
        public ICommand DisableOrEnableProductCommand { get; }
        public ICommand SaveCommand { get; }
            #endregion

            #region Product properties
        public ObservableCollection<ProductDTO> Products { 
            get => _products; 
            set {
                _products = value;
                OnPropertyChanged(nameof(Products));
                OnPropertyChanged(nameof(ProductCount));
            }
        }

        public ObservableCollection<ProductDTO> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged(nameof(FilteredProducts));

            }
        }

        public ProductDTO? SelectedProduct
        {
            get { return _selectedProduct; }
            set { 
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(ProductIsActive));


                //update text to match selected product
                SearchTextCategories = _selectedProduct?.CategoryName ?? "";
                SearchTextShelf = _selectedProduct?.ShelfName ?? "";
                SearchTextTaxRate = _selectedProduct?.TaxRate.ToString() ?? "";
            }
        }

        public string ProductIsActive
        {
            get
            {
                if (_selectedProduct is null || _selectedProduct.IsActivate) return "disable";

                return "enable";
            }
        }

        public int ProductCount
        {
            get => _products?.Count ?? 0;
        }
        #endregion

            #region category properties
        public ObservableCollection<string> AllCategories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> FilteredCategories
        {
            get => _filteredCategories;
            set { _filteredCategories = value; OnPropertyChanged(); }
        }
        #endregion

            #region tax rate properties
        public ObservableCollection<string> FilteredTaxRate
        {
            get => _filteredTaxRate;
            set { _filteredTaxRate = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> AllTaxRate
        {
            get => _taxRates;
            set { _taxRates = value; OnPropertyChanged(); }
        }
        #endregion

            #region shelf properties
        public ObservableCollection<string> FilteredShelf
        {
            get => _filteredShelf;
            set { _filteredShelf = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> AllShelf
        {
            get => _shelfs;
            set { _shelfs = value; OnPropertyChanged(); }
        }
        #endregion

            #region search/filter text properties
        public string SearchTextName
        {
            get { return _searchTextName; }
            set { 
                _searchTextName = value;
                ////delete text in barcode search box
                _searchTextBarcode = "";
                OnPropertyChanged(nameof(SearchTextBarcode));
                OnPropertyChanged(nameof(SearchTextName));
                FilterProducts();
            }
        }

        public string SearchTextBarcode
        {
            get { return _searchTextBarcode; }
            set
            {
                _searchTextBarcode = value;
                ////delete text in name search box
                _searchTextName = "";
                OnPropertyChanged(nameof(SearchTextName));
                OnPropertyChanged(nameof(SearchTextBarcode));
                FilterProducts();
            }
        }

        public string SearchTextCategories
        {
            get => _searchTextCategories;
            set
            {
                _searchTextCategories = value;
                OnPropertyChanged();
                FilterCategories();
            }
        }

        public string SearchTextTaxRate
        {
            get => _searchTextTaxRate;
            set
            {
                _searchTextTaxRate = value;
                OnPropertyChanged();
                FilterTaxRate();
            }
        }

        public string SearchTextShelf
        {
            get => _searchTextShelf;
            set
            {
                _searchTextShelf = value;
                OnPropertyChanged();
                FilterShelf();
            }
        }
        #endregion
        #endregion

        #region constructors
        public VmProducts(NavigationStore navigationStore)
        {
            //NavigateDataCommand = new NavigationCommand<vmLogin>(navigationStore,
            //    () => new vmLogin(navigationStore));

            //initialize the api service
            _apiService = new ApiService("https://f557-2a02-2c40-270-2029-58ea-66d1-3111-66b1.ngrok-free.app/");

            //get products from api
            Task.Run(LoadDataAsync);

            //initialize the commands
            DisableOrEnableProductCommand = new ButtonCommand(DisableOrEnableProduct);
            SaveCommand = new ButtonCommand(SaveProduct);
        }
        #endregion


        #region filter methods
        private void FilterProducts()
        {
            var filterdProducts = Products.AsEnumerable();

            if (!String.IsNullOrWhiteSpace(SearchTextName))
                filterdProducts = filterdProducts?.Where(p => p.Name.ToLower().Contains(SearchTextName.ToLower()));

            if (!String.IsNullOrWhiteSpace(SearchTextBarcode))
                filterdProducts = filterdProducts?.Where(p => p.Barcode.ToLower().Contains(SearchTextBarcode.ToLower()));

            FilteredProducts = new ObservableCollection<ProductDTO>(filterdProducts);
        }

        private void FilterCategories()
        {
            if (string.IsNullOrWhiteSpace(_searchTextCategories))
                FilteredCategories = new ObservableCollection<string>(
                    _categories ?? new ObservableCollection<string>());
            else
            {
                FilteredCategories = new ObservableCollection<string>(
                    _categories.AsEnumerable()
                    .Where(
                        items => items.ToLower()
                        .Contains(_searchTextCategories.ToLower())
                    )
                    .ToList()
                );
            }
        }

        public void FilterTaxRate()
        {
            if (string.IsNullOrWhiteSpace(_searchTextTaxRate))
                FilteredTaxRate = new ObservableCollection<string>(
                    _taxRates ?? new ObservableCollection<string>());
            else
            {
                FilteredTaxRate = new ObservableCollection<string>(
                    _taxRates.AsEnumerable()
                    .Where(
                        items => items.ToLower()
                        .Contains(_searchTextTaxRate.ToLower())
                    )
                    .ToList()
                );
            }
        }

        public void FilterShelf()
        {
            if (string.IsNullOrWhiteSpace(_searchTextShelf))
                FilteredShelf = new ObservableCollection<string>(
                    _shelfs ?? new ObservableCollection<string>());
            else
            {
                FilteredShelf = new ObservableCollection<string>(
                    _shelfs
                    .Where(
                        items => items.ToLower()
                        .Contains(_searchTextShelf.ToLower())
                    )
                    .ToList()
                );
            }
        }
        #endregion

        #region Methods
            #region command methods
        private void DisableOrEnableProduct(object parameter)
        {
            if (_selectedProduct == null)
                return;

            _selectedProduct.IsActivate = !_selectedProduct.IsActivate;
            _products[_products.IndexOf(_selectedProduct)] = _selectedProduct;
            FilterProducts();
            OnPropertyChanged(nameof(ProductIsActive));
            //OnPropertyChanged(nameof(SelectedProduct));
            //OnPropertyChanged(nameof(FilteredProducts));
        }

        private void SaveProduct(object parameter)
        {
            throw new NotImplementedException();
        }
            #endregion

        private async Task LoadDataAsync()
        {
            Products = new ObservableCollection<ProductDTO>(await _apiService.GetProductsAsync());

            if(Products.Count == 0)
            {
                Products = new ObservableCollection<ProductDTO>()
                {
                    new ProductDTO(){ ProductId = 0, Name = "product 1", PurchasePrice = 13.20m, SalePrice1 = 15.60m, SalePrice2 = 17.60m, TaxRate = 6, CategoryName = "category 1", Barcode = "4534843785"},
                    new ProductDTO(){ ProductId = 1, Name = "special product 1", PurchasePrice = 13.20m, SalePrice1 = 15.60m, SalePrice2 = 17.60m, TaxRate = 6, CategoryName = "category 1", Barcode = "46348644"},
                    new ProductDTO(){ ProductId = 2, Name = "product 2", PurchasePrice = 13.20m, SalePrice1 = 15.60m, SalePrice2 = 17.60m, TaxRate = 6, CategoryName = "category 5", Barcode = "87641864164"},
                    new ProductDTO(){ ProductId = 3, Name = "extremely special product 1", PurchasePrice = 13.20m, SalePrice1 = 15.60m, SalePrice2 = 17.60m, TaxRate = 6, CategoryName = "category 2", Barcode = "5384353843"}
                };

            }
            //sort products by name
            FilteredProducts = new ObservableCollection<ProductDTO>(Products.OrderBy(p => p.Name).ToList());

            //temp filling of categories
            AllCategories = new ObservableCollection<string> {
                "no category selected",
                "category 1",
                "category 2",
                "category 3",
            };
            FilteredCategories = AllCategories;

            //temp filling of tax rates
            AllTaxRate = new ObservableCollection<string> {
                "no tax rate selected",
                "6%",
                "21%"
            };
            FilteredTaxRate = AllTaxRate;

            //temp filling of shelves
            AllShelf = new ObservableCollection<string> {
                "no shelf selected",
                "shelf 1",
                "shelf 2",
                "shelf 3"
            };
            FilteredShelf = AllShelf;


        }
        #endregion

    }
}
