using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Commands.ReportsCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{


    public class VmReport : VmBase
    {
        private readonly ApiReport _apiReport;
        private int _pageNumber = 1;
        private readonly int _pageSize = 15;
        private int _totalProducts;
        private string _searchText;
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<ProductDTO> _filteredProducts;

        public ObservableCollection<ProductDTO> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProductDTO> FilteredProducts
        {
            get => _filteredProducts;
            set
            {
                _filteredProducts = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterProducts();
            }
        }

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                _pageNumber = value;
                OnPropertyChanged();
            }
        }

        public int TotalProducts
        {
            get => _totalProducts;
            set
            {
                _totalProducts = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateDashboardCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand PrintCommand { get; }

        public VmReport(NavigationStore navigationStore)
        {
            _apiReport = new ApiReport(AppConfig.ApiUrl);

            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));

            PreviousPageCommand = new ButtonCommand(PreviousPage);
            NextPageCommand = new ButtonCommand(NextPage);

            Products = new ObservableCollection<ProductDTO>();
            FilteredProducts = new ObservableCollection<ProductDTO>();
            PrintCommand = new UpdateCommand(this);
            ResetCommand = new ResetCommand(this);
            PrintCommand = new PrintCommand(this);
            LoadProducts(); 
        }

        private async void LoadProducts()
        {
            var list = await _apiReport.GetReportsAsync(PageNumber, _pageSize);
            Products.Clear();
            FilteredProducts.Clear();

            foreach (var p in list)
            {
                Products.Add(p);
                FilteredProducts.Add(p);
            }

            TotalProducts = await _apiReport.GetProductCountAsync();
        }

        private void FilterProducts()
        {
            FilteredProducts.Clear();
            foreach (var p in Products)
            {
                if (string.IsNullOrWhiteSpace(SearchText) || p.Name.ToLower().Contains(SearchText.ToLower()))
                {
                    FilteredProducts.Add(p);
                }
            }

            TotalProducts = FilteredProducts.Count;
        }

        private async void PreviousPage(object parameter)
        {
            if (PageNumber <= 1) return;

            PageNumber--;
            var list = await _apiReport.GetReportsAsync(PageNumber, _pageSize);
            Products = new ObservableCollection<ProductDTO>(list);
            FilterProducts();
        }

        private async void NextPage(object parameter)
        {
            int totalPages = (int)Math.Ceiling(TotalProducts / (double)_pageSize);
            if (PageNumber >= totalPages) return;

            PageNumber++;
            var list = await _apiReport.GetReportsAsync(PageNumber, _pageSize);
            Products = new ObservableCollection<ProductDTO>(list);
            FilterProducts();
        }

        public async void RefreshProducts()
        {
            var list = await _apiReport.GetReportsAsync(PageNumber, _pageSize);

            Products.Clear();
            FilteredProducts.Clear();

            foreach (var p in list)
            {
                Products.Add(p);
                FilteredProducts.Add(p);
            }

            TotalProducts = await _apiReport.GetProductCountAsync();
        }
    }
}
