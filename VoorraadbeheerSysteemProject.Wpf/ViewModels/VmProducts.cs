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

        private IList<ProductDTO> _products;
        private IList<ProductDTO> _filteredProducts;
        private ProductDTO? _selectedProduct;

        private string _searchTextName = "";
        private string _searchTextBarcode = "";


        public IList<ProductDTO> Products { 
            get => _products; 
            set {
                _products = value;
                OnPropertyChanged(nameof(Products));
                OnPropertyChanged(nameof(ProductCount));
            }
        }

        public IList<ProductDTO> FilteredProducts
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

                //update text to match selected product
                SearchTextCategories = _selectedProduct?.CategoryName ?? "";
                SearchTextShelf = _selectedProduct?.ShelfName ?? "";
                SearchTextTaxRate = _selectedProduct?.TaxRate.ToString() ?? "";
            }
        }

        public int ProductCount
        {
            get => _products?.Count ?? 0;
        }

        

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


        public VmProducts(NavigationStore navigationStore)
        {

            //NavigateDataCommand = new NavigationCommand<vmLogin>(navigationStore,
            //    () => new vmLogin(navigationStore));

            //initialize the api service
            _apiService = new ApiService("https://inventoryapi-dtavbdhhgdama7cr.switzerlandnorth-01.azurewebsites.net/");

            //get products from api
            Task.Run(LoadDataAsync);

        }

        #region category filter
        private string _searchTextCategories;
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

        private IList<string> _allCategories;
        public IList<string> AllCategories
        {
            get => _allCategories;
            set { _allCategories = value; OnPropertyChanged(); }
        }

        private IList<string> _filteredCategories;
        public IList<string> FilteredCategories
        {
            get => _filteredCategories;
            set { _filteredCategories = value; OnPropertyChanged(); }
        }

        private void FilterCategories()
        {
            if (string.IsNullOrWhiteSpace(SearchTextCategories))
                FilteredCategories = new List<string>(AllCategories ?? new List<string>());
            else
            {
                FilteredCategories = AllCategories
                    .Where(items => items.ToLower()
                        .Contains(SearchTextCategories.ToLower()))
                    .ToList();
            }
        }
        #endregion

        #region tax rate filter
        private string _searchTextTaxRate;
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
        private IList<string> _filteredTaxRate;
        public IList<string> FilteredTaxRate
        {
            get => _filteredTaxRate;
            set { _filteredTaxRate = value; OnPropertyChanged(); }
        }

        private IList<string> _allTaxRate;
        public IList<string> AllTaxRate
        {
            get => _allTaxRate;
            set { _allTaxRate = value; OnPropertyChanged(); }
        }
        public void FilterTaxRate()
        {
            if (string.IsNullOrWhiteSpace(SearchTextTaxRate))
                FilteredTaxRate = new List<string>(AllTaxRate ?? new List<string>());
            else
            {
                FilteredTaxRate = AllTaxRate
                    .Where(items => items.ToLower()
                        .Contains(SearchTextTaxRate.ToLower()))
                    .ToList();
            }
        }
        #endregion

        #region Shelf filter
        private string _searchTextShelf;
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
        private IList<string> _filteredShelf;
        public IList<string> FilteredShelf
        {
            get => _filteredShelf;
            set { _filteredShelf = value; OnPropertyChanged(); }
        }

        private IList<string> _allShelf;
        public IList<string> AllShelf
        {
            get => _allShelf;
            set { _allShelf = value; OnPropertyChanged(); }
        }
        public void FilterShelf()
        {
            if (string.IsNullOrWhiteSpace(SearchTextShelf))
                FilteredShelf = new List<string>(AllShelf ?? new List<string>());
            else
            {
                FilteredShelf = AllShelf
                    .Where(items => items.ToLower()
                        .Contains(SearchTextShelf.ToLower()))
                    .ToList();
            }
        }
        #endregion



        //methods
        private void FilterProducts()
        {
            IList<ProductDTO> filterdProducts = Products;
            
            if(!String.IsNullOrWhiteSpace(SearchTextName))
                filterdProducts = filterdProducts?.Where(p => p.Name.ToLower().Contains(SearchTextName.ToLower())).ToList() ?? new List<ProductDTO>();

            if (!String.IsNullOrWhiteSpace(SearchTextBarcode))
                filterdProducts = filterdProducts?.Where(p => p.Barcode.ToLower().Contains(SearchTextBarcode.ToLower())).ToList() ?? new List<ProductDTO>();

            FilteredProducts = filterdProducts;
        }

        private async Task LoadDataAsync()
        {
            Products = await _apiService.GetProductsAsync();

            if(Products.Count == 0)
            {
                Products = new List<ProductDTO>()
                {
                    new ProductDTO(){ ProductId = 0, Name = "product 1", PurchasePrice = 13.20m, SalePrice1 = 15.60m, SalePrice2 = 17.60m, TaxRate = 6, CategoryName = "category 1", Barcode = "4534843785"},
                    new ProductDTO(){ ProductId = 1, Name = "special product 1", PurchasePrice = 13.20m, SalePrice1 = 15.60m, SalePrice2 = 17.60m, TaxRate = 6, CategoryName = "category 1", Barcode = "46348644"},
                    new ProductDTO(){ ProductId = 2, Name = "product 2", PurchasePrice = 13.20m, SalePrice1 = 15.60m, SalePrice2 = 17.60m, TaxRate = 6, CategoryName = "category 5", Barcode = "87641864164"},
                    new ProductDTO(){ ProductId = 3, Name = "extremely special product 1", PurchasePrice = 13.20m, SalePrice1 = 15.60m, SalePrice2 = 17.60m, TaxRate = 6, CategoryName = "category 2", Barcode = "5384353843"}
                };

            }
            //sort products by name
            FilteredProducts = Products.OrderBy(p => p.Name).ToList();

            //temp filling of categories
            AllCategories = new List<string> {
                "no category selected",
                "category 1",
                "category 2",
                "category 3",
            };
            FilteredCategories = AllCategories;

            //temp filling of tax rates
            AllTaxRate = new List<string> {
                "no tax rate selected",
                "6%",
                "21%"
            };
            FilteredTaxRate = AllTaxRate;

            //temp filling of shelves
            AllShelf = new List<string> {
                "no shelf selected",
                "shelf 1",
                "shelf 2",
                "shelf 3"
            };
            FilteredShelf = AllShelf;



        }


    }
}
