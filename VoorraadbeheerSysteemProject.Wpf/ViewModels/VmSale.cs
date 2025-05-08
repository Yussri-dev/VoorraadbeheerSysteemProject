using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
