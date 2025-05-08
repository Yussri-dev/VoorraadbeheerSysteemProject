using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmSale : VmBase
    {
        private readonly ApiService _apiService;
        private readonly NavigationStore _navigationStore;



        public VmSale(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _apiService = new ApiService("https://inventoryapi-dtavbdhhgdama7cr.switzerlandnorth-01.azurewebsites.net/");

            Products = new ObservableCollection<ProductDTO>();
            Task.Run(async () => await LoadDataAsync());

            InitialCommands();

        }

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
        private async Task LoadDataAsync()
        {
            var products = await _apiService.GetProductsAsync();

            App.Current.Dispatcher.Invoke(() =>
            {
                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }
            });
        }



        #region commands
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

        #endregion

    }
}
