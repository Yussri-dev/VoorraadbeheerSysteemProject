using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Commands.CustomersCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmCustomer : VmBase
    {
        private readonly ApiCustomer _apiCustomer;
        private string _searchText;
        private int _totalCustomers;
        private int _pageNumber = 1;
        private int _pageSize = 10;
        private string _newCustomerName;
        private CustomerDTO _selectedCustomer;

        public ObservableCollection<CustomerDTO> Customers { get; set; }
        public ObservableCollection<CustomerDTO> FilteredCustomers { get; set; }

        public ApiCustomer ApiCustomer => _apiCustomer;

        #region ICommands
        //Navigations
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        // Commands
        public ICommand UpdateCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand NavigateDashBoardCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        #endregion

        public VmCustomer(NavigationStore navigationStore)
        {
            Customers = new ObservableCollection<CustomerDTO>();
            FilteredCustomers = new ObservableCollection<CustomerDTO>();

            _apiCustomer = new ApiCustomer(AppConfig.ApiUrl);

            NavigateDashBoardCommand = new NavigationCommand<VmDashboard>(
                navigationStore,() => new VmDashboard(navigationStore));

            UpdateCommand = new UpdateCustomerCommand(this);

            PreviousPageCommand = new ButtonCommand(PreviousPage);
            
            NextPageCommand = new ButtonCommand(NextPage);

            LoadCustomers();
        }
        public CustomerDTO SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
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
                FilterCustomers();
            }
        }

        public int TotalCustomers
        {
            get => _totalCustomers;
            set
            {
                _totalCustomers = value;
                OnPropertyChanged();
            }
        }


        public async void RefreshCustomers()
        {
            var listCustomers = await _apiCustomer.GetCustomersAsync(_pageNumber, _pageSize);

            Customers.Clear();
            FilteredCustomers.Clear();

            foreach (var customer in listCustomers)
            {
                Customers.Add(customer);
                FilteredCustomers.Add(customer);
            }

            TotalCustomers = await _apiCustomer.GetCustomerCountAsync();
        }

        private async void LoadCustomers()
        {
            var list = await _apiCustomer.GetCustomersAsync(_pageNumber, _pageSize);

            Customers.Clear();
            FilteredCustomers.Clear();

            foreach (var line in list)
            {
                Customers.Add(line);
                FilteredCustomers.Add(line);
            }

            TotalCustomers = await _apiCustomer.GetCustomerCountAsync();
        }

        public void FilterCustomers()
        {
            FilteredCustomers.Clear();
            foreach (var line in Customers)
            {
                if (string.IsNullOrWhiteSpace(SearchText) || line.Name.ToLower().Contains(SearchText.ToLower()))
                {
                    FilteredCustomers.Add(line);
                }
            }
            TotalCustomers = FilteredCustomers.Count;
        }

        public async void RefreshLines()
        {
            var list = await _apiCustomer.GetCustomersAsync(_pageNumber, _pageSize);

            Customers.Clear();
            FilteredCustomers.Clear();

            foreach (var line in list)
            {
                Customers.Add(line);
                FilteredCustomers.Add(line);
            }

            TotalCustomers = await _apiCustomer.GetCustomerCountAsync();
        }

        public string NewCustomerName
        {
            get => _newCustomerName;
            set
            {
                _newCustomerName = value;
                OnPropertyChanged();
            }
        }

        public async Task AddCustomerAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCustomerName))
                return;

            var newCustomer = new CustomerDTO { Name = NewCustomerName };
            await _apiCustomer.PostCustomerAsync(newCustomer);
            NewCustomerName = string.Empty;
            RefreshLines();
        }

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void PreviousPage(object parameter)
        {
            if (PageNumber <= 1) return;

            PageNumber--;
            var list = await _apiCustomer.GetCustomersAsync(PageNumber, _pageSize);
            Customers = new ObservableCollection<CustomerDTO>(list);
            FilterCustomers();
        }

        private async void NextPage(object parameter)
        {
            int totalPages = (int)Math.Ceiling(TotalCustomers / (double)_pageSize);
            if (PageNumber >= totalPages) return;

            PageNumber++;
            var list = await _apiCustomer.GetCustomersAsync(PageNumber, _pageSize);
            Customers = new ObservableCollection<CustomerDTO>(list);
            FilterCustomers();
        }

    }
}
