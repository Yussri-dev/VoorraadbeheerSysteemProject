using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Commands.ProductsCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Requests;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmSale : VmBase
    {
        private readonly ApiService _apiService;
        private readonly NavigationStore _navigationStore;

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

        private ProductDTO? _selectedProduct;
        public ProductDTO? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));

                (AddSelectedProductCommand as AddSelectedProductCommand)?.RaiseCanExecuteChanged();
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

        public VmSale(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _apiService = new ApiService("https://inventoryapi-dtavbdhhgdama7cr.switzerlandnorth-01.azurewebsites.net/");

            _allProducts = new ObservableCollection<ProductDTO>();
            Products = new ObservableCollection<ProductDTO>();
            Task.Run(async () => await LoadDataAsync());

            InitialCommands();

        }

        

        private void InitialCommands()
        {
            Number0Command = new AppendNumberCommand(this, "0");
            Number00Command = new AppendNumberCommand(this, "00");
            Number1Command = new AppendNumberCommand(this, "1");
            Number2Command = new AppendNumberCommand(this, "2");
            Number3Command = new AppendNumberCommand(this, "3");
            Number4Command = new AppendNumberCommand(this, "4");
            Number5Command = new AppendNumberCommand(this, "5");
            Number6Command = new AppendNumberCommand(this, "6");
            Number7Command = new AppendNumberCommand(this, "7");
            Number8Command = new AppendNumberCommand(this, "8");
            Number9Command = new AppendNumberCommand(this, "9");
            NumberPuntCommand = new AppendNumberCommand(this, ".");
            DeleteCommand = new DeleteInputCommand(this);
            ReturnCommand = new ReturnInputCommand(this);

            ClosingCommand = new ClosingCommand(this);

            SearchProductsCommand = new SearchProductsCommand(this);

            AddSelectedProductCommand = new AddSelectedProductCommand(this);

        }

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

            App.Current.Dispatcher.Invoke(() =>
            {
                _allProducts = new ObservableCollection<ProductDTO>(products); // FIXED
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            });
        }

        #region commands
        public ICommand AddSelectedProductCommand { get; set; }
        public ICommand SearchProductsCommand { get; set; }
        public ICommand Number0Command { get; set; }
        public ICommand Number00Command { get; set; }
        public ICommand Number1Command { get; set; }
        public ICommand Number2Command { get; set; }
        public ICommand Number3Command { get; set; }
        public ICommand Number4Command { get; set; }
        public ICommand Number5Command { get; set; }
        public ICommand Number6Command { get; set; }
        public ICommand Number7Command { get; set; }
        public ICommand Number8Command { get; set; }
        public ICommand Number9Command { get; set; }
        public ICommand NumberPuntCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReturnCommand { get; set; }

        public ICommand ClosingCommand { get; set; }

        #endregion

    }
}
