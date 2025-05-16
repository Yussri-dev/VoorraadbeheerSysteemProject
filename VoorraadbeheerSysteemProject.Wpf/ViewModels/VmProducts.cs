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
using System.ComponentModel;
using System.Windows;
using System.Diagnostics.CodeAnalysis;
using System.Configuration;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmProducts : VmBase
    {
        private readonly ApiService _apiService;
        private int _pageNumber = 1;
        private int _pageSize = 10;

        //productDTO
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<ProductDTO> _filteredProducts;
        private ProductDTO _selectedProduct = new ProductDTO();
        private int _productCount = 0;

        //categoryDTO
        private ObservableCollection<CategoryDTO> _categories;
        private ObservableCollection<CategoryDTO> _filteredCategories;

        //TaxRateDTO
        private ObservableCollection<TaxDTO> _taxRates;
        private ObservableCollection<TaxDTO> _filteredTaxRate;

        //ShelfDTO
        private ObservableCollection<ShelfDTO> _shelfs;
        private ObservableCollection<ShelfDTO> _filteredShelf;


        //search/filter
        private string _searchTextName = "";
        private string _searchTextBarcode = "";
        private string _searchTextTaxRate;
        private string _searchTextShelf;
        private string _searchTextCategories;


        #region properties
        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                _pageNumber = value;
                OnPropertyChanged(nameof(PageNumber));
            }
        }

        #region command properties
        public ICommand DisableOrEnableProductCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ResetButtonCommand { get; }
        public ICommand AddButtonCommand { get; }
        public ICommand NavigateDashboardCommand { get; }
        public ICommand PreviousPageButtonCommand { get; }
        public ICommand NextPageButtonCommand { get; }
        #endregion

        #region Product properties
        public ObservableCollection<ProductDTO> Products 
        { 
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
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                OnPropertyChanged(nameof(ProductIsActive));

                //update text to match selected product
                SearchTextCategories = _selectedProduct?.CategoryName ?? "";
                SearchTextShelf = _selectedProduct?.ShelfName ?? "";
                SearchTextTaxRate = _selectedProduct?.TaxRate.ToString() ?? "";
            }
        }

        public int ProductCount
        {
            get => _productCount;
            set
            {
                _productCount = value;
                OnPropertyChanged(nameof(ProductCount));
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
        #endregion

            #region category properties
        public ObservableCollection<CategoryDTO> AllCategories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CategoryDTO> FilteredCategories
        {
            get => _filteredCategories;
            set { _filteredCategories = value; OnPropertyChanged(); }
        }
        #endregion

        #region tax rate properties
        public ObservableCollection<TaxDTO> FilteredTaxRate
        {
            get => _filteredTaxRate;
            set { _filteredTaxRate = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TaxDTO> AllTaxRate
        {
            get => _taxRates;
            set { _taxRates = value; OnPropertyChanged(); }
        }
        #endregion

    #region shelf properties
        public ObservableCollection<ShelfDTO> FilteredShelf
        {
            get => _filteredShelf;
            set { _filteredShelf = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ShelfDTO> AllShelf
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
            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));

            //initialize the api service
            _apiService = new ApiService(ConfigurationManager.AppSettings.Get("NGrokApiUri"));

            //get products from api
            Task.Run(LoadDataAsync);

            //initialize the commands
            DisableOrEnableProductCommand = new ButtonCommand(DisableOrEnableProduct);
            SaveCommand = new ButtonCommand(SaveProduct);
            ResetButtonCommand = new ButtonCommand(Reset);
            AddButtonCommand = new ButtonCommand(AddProduct);
            PreviousPageButtonCommand = new ButtonCommand(PreviousPage);
            NextPageButtonCommand = new ButtonCommand(NextPage);
        }
        #endregion

        #region Methods
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
                FilteredCategories = new ObservableCollection<CategoryDTO>(
                    _categories ?? new ObservableCollection<CategoryDTO>());
            else
            {
                FilteredCategories = new ObservableCollection<CategoryDTO>(
                    _categories.AsEnumerable()
                    .Where(
                        items => items.Name.ToLower()
                        .Contains(_searchTextCategories.ToLower())
                    )
                    .ToList()
                );
            }
        }

        public void FilterTaxRate()
        {
            if (string.IsNullOrWhiteSpace(_searchTextTaxRate))
                FilteredTaxRate = new ObservableCollection<TaxDTO>(
                    _taxRates ?? new ObservableCollection<TaxDTO>());
            else
            {
                FilteredTaxRate = new ObservableCollection<TaxDTO>(
                    _taxRates.AsEnumerable()
                    .Where(
                        items => items.TaxRate.ToString()
                        .Contains(_searchTextTaxRate.ToLower())
                    )
                    .ToList()
                );
            }
        }

        public void FilterShelf()
        {
            if (string.IsNullOrWhiteSpace(_searchTextShelf))
                FilteredShelf = new ObservableCollection<ShelfDTO>(
                    _shelfs ?? new ObservableCollection<ShelfDTO>());
            else
            {
                FilteredShelf = new ObservableCollection<ShelfDTO>(
                    _shelfs
                    .Where(
                        items => items.Name.ToLower()
                        .Contains(_searchTextShelf.ToLower())
                    )
                    .ToList()
                );
            }
        }
        #endregion

            #region command methods
        private async void PreviousPage(object parameter)
        {
            if (_pageNumber <= 1) return;

            PageNumber--;

            //get products from api
            Products = new ObservableCollection<ProductDTO>(await _apiService.GetProductsAsync(_pageNumber));
            FilterProducts();
        }

        private async void NextPage(object parameter)
        {
            if (_pageNumber >= Math.Ceiling(ProductCount / (double)_pageSize)) return;

            PageNumber++;

            //get products from api
            Products = new ObservableCollection<ProductDTO>(await _apiService.GetProductsAsync(_pageNumber));
            FilterProducts();
        }


        private void DisableOrEnableProduct(object parameter)
        {
            if ( String.IsNullOrEmpty(_selectedProduct.Barcode))
                return;

            _selectedProduct.IsActivate = !_selectedProduct.IsActivate;
            _products[_products.IndexOf(_selectedProduct)] = _selectedProduct;
            FilterProducts();
            OnPropertyChanged(nameof(ProductIsActive));
        }

        private async void SaveProduct(object parameter)
        {
            if(_selectedProduct == null)
                return;

            _selectedProduct.DateModified = DateTime.Now;

            //TODO check if there are changes
            await _apiService.PutProductAsync(_selectedProduct);
        }

        private async void AddProduct(object parameter)
        {
            var newProduct = SelectedProduct;

            //get from comboboxes
            if(SelectedProduct.Category == null)
            {
                MessageBox.Show("Please select a category");
                return;
            }
            newProduct.CategoryId = SelectedProduct.Category.CategoryId;

            if(SelectedProduct.Tax == null)
            {
                MessageBox.Show("Please select a tax rate");
                return;
            }
            newProduct.TaxId = SelectedProduct.Tax.TaxId;


            if(SelectedProduct.Shelf == null)
            {
                MessageBox.Show("Please select a shelf");
                return;
            }
            newProduct.ShelfId = SelectedProduct.Shelf.ShelfId;



            //check textboxes
            if(String.IsNullOrWhiteSpace(newProduct.Name))
            { 
                MessageBox.Show("Please fill in a name");
                return;
            }
            if(newProduct.MinStock <= 0)
            {
                MessageBox.Show("Minimal Stock must be greater than 0");
                return;
            }
            if(newProduct.PurchasePrice <= 0)
            {
                MessageBox.Show("Purchase price must be greater than 0");
                return;
            }
            if(newProduct.SalePrice1 <= 0)
            {
                MessageBox.Show("Sale price 1 must be greater than 0");
                return;
            }
            if(newProduct.SalePrice2 <= 0)
            {
                MessageBox.Show("Sale price 2 must be greater than 0");
                return;
            }



            //generate barcode
            //13 lang begin met 1 in begin count products
            newProduct.Barcode = newProduct.Barcode.Trim();

            if (String.IsNullOrWhiteSpace(newProduct.Barcode))
            {
                var barcode = ProductCount.ToString();
                var barcodeLenght = barcode.Length;
                    if (barcodeLenght < 13)
                    {
                        for (int i = 0; i < 12 - barcodeLenght; i++)
                        {
                            barcode = "0" + barcode;
                        }
                        barcode = "1" + barcode;
                    }
                newProduct.Barcode = barcode;
            }
            else if(!Int32.TryParse(newProduct.Barcode, out int barcodeInt))
            {
                MessageBox.Show("Barcode must be a number (leave empty to auto-generate a barcode");
                return;
            }
            else if(newProduct.Barcode.Length != 13)
            {
                MessageBox.Show("Barcode must be 13 characters long (leave empty to auto-generate a barcode)");
                return;
            }

            newProduct.DateCreated = DateTime.Now;

            //temp fill in data
            newProduct.LineId = 1;
            newProduct.UnitId = 1;
            newProduct.PackUnitType = "Box";



            //get from session
            newProduct.CreatedBy = "admin";


            await _apiService.PostProductAsync(newProduct);
            //refresh the product list
            Products = new ObservableCollection<ProductDTO>(await _apiService.GetProductsAsync());
            FilterProducts();
        }

        private void Reset(object parameter)
        {
            SelectedProduct = new ProductDTO();
        }
            #endregion

        private async Task LoadDataAsync()
        {
            //get product count
            ProductCount = await _apiService.GetProductCountAsync();


            //get products from api
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
            FilteredProducts = new ObservableCollection<ProductDTO>(Products.OrderBy(p => p.Name).ToList());


            //get categories from api
            AllCategories = new ObservableCollection<CategoryDTO>(await _apiService.GetCategoriesAsync());

            if(AllCategories.Count == 0)
            {
                AllCategories = new ObservableCollection<CategoryDTO> {
                    new CategoryDTO(){ CategoryId = 0, Name = "no category selected"},
                    new CategoryDTO(){ CategoryId = 1, Name = "category 1"},
                    new CategoryDTO(){ CategoryId = 2, Name = "category 2"},
                    new CategoryDTO(){ CategoryId = 3, Name = "category 3"},
                };
            }
            FilteredCategories = AllCategories;



            AllTaxRate = new ObservableCollection<TaxDTO>(await _apiService.GetTaxRatesAsync());
            
            //temp filling of tax rates
            if(AllTaxRate.Count == 0)
            {
                AllTaxRate = new ObservableCollection<TaxDTO> {
                    new TaxDTO(){ TaxId = 0, TaxRate = 0},
                    new TaxDTO(){ TaxId = 1, TaxRate = 6},
                    new TaxDTO(){ TaxId = 2, TaxRate = 21},
                };
            }
            FilteredTaxRate = new ObservableCollection<TaxDTO>(AllTaxRate.OrderBy(t => t.TaxRate));

            AllShelf = new ObservableCollection<ShelfDTO>(await _apiService.GetShelfsAsync());

            //temp filling of shelves
            if(AllShelf.Count == 0)
            {
                AllShelf = new ObservableCollection<ShelfDTO> {
                    new ShelfDTO(){ ShelfId = 1, Name = "shelf 1"},
                    new ShelfDTO(){ ShelfId = 2, Name = "shelf 2"},
                    new ShelfDTO(){ ShelfId = 3, Name = "shelf 3"},
                };
            }
            FilteredShelf = AllShelf;


        }
        #endregion

    }
}
