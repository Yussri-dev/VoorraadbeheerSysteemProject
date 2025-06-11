using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Commands.TaxCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{


    public class VmTax : VmBase
    {
        private readonly ApiTax _apiTax;
        private int _totalTaxes;
        private int _pageNumber = 1;
        private readonly int _pageSize = 15;
        private string _searchText;
        private string _newTaxRate;
        private TaxDTO _selectedTax;

        public ObservableCollection<TaxDTO> Taxes { get; set; }
        public ObservableCollection<TaxDTO> FilteredTaxes { get; set; }

        public ApiTax ApiTax => _apiTax;
        public ICommand UpdateTaxCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand NavigateDashboardCommand { get; }

        public VmTax(NavigationStore navigationStore)
        {
            Taxes = new ObservableCollection<TaxDTO>();
            FilteredTaxes = new ObservableCollection<TaxDTO>();

            _apiTax = new ApiTax(AppConfig.ApiUrl);

            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(
                navigationStore, () => new VmDashboard(navigationStore));
            UpdateTaxCommand = new UpdateTaxCommand(this);
            AddCommand = new AddCommand(this);
            ResetCommand = new ResetCommand(this);
            DeleteCommand = new DeleteCommand(this);
            PreviousPageCommand = new ButtonCommand(PreviousPage);
            NextPageCommand = new ButtonCommand(NextPage);

            LoadTaxes();
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterTaxes();
            }
        }

        public int TotalTaxes
        {
            get => _totalTaxes;
            set
            {
                _totalTaxes = value;
                OnPropertyChanged();
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

        public TaxDTO SelectedTax
        {
            get => _selectedTax;
            set
            {
                _selectedTax = value;
                OnPropertyChanged();
            }
        }

        public string NewTaxRate
        {
            get => _newTaxRate;
            set
            {
                _newTaxRate = value;
                OnPropertyChanged();
            }
        }

        private async void LoadTaxes()
        {
            var list = await _apiTax.GetTaxesAsync(_pageNumber, _pageSize);

            Taxes.Clear();
            FilteredTaxes.Clear();

            foreach (var tax in list)
            {
                Taxes.Add(tax);
                FilteredTaxes.Add(tax);
            }

            TotalTaxes = await _apiTax.GetTaxCountAsync();
        }

        private void FilterTaxes()
        {
            FilteredTaxes.Clear();
            foreach (var tax in Taxes)
            {
                if (string.IsNullOrWhiteSpace(SearchText) ||
                    tax.TaxRate.ToString(CultureInfo.InvariantCulture).Contains(SearchText))
                {
                    FilteredTaxes.Add(tax);
                }
            }

            TotalTaxes = FilteredTaxes.Count;
        }

        public async Task AddTaxAsync()
        {
            if (!decimal.TryParse(NewTaxRate, out decimal parsedRate))
                return;

            var newTax = new TaxDTO { TaxRate = parsedRate };
            await _apiTax.PostTaxAsync(newTax);

            NewTaxRate = string.Empty;
            RefreshTaxes();
        }

        public async Task DeleteSelectedTaxAsync()
        {
            if (SelectedTax == null)
                return;

            await _apiTax.DeleteTaxAsync(SelectedTax.TaxId);
            RefreshTaxes();
        }

        public async void RefreshTaxes()
        {
            var list = await _apiTax.GetTaxesAsync(_pageNumber, _pageSize);

            Taxes.Clear();
            FilteredTaxes.Clear();

            foreach (var tax in list)
            {
                Taxes.Add(tax);
                FilteredTaxes.Add(tax);
            }

            TotalTaxes = await _apiTax.GetTaxCountAsync();
        }

        private async void PreviousPage(object parameter)
        {
            if (PageNumber <= 1) return;

            PageNumber--;
            var list = await _apiTax.GetTaxesAsync(_pageNumber, _pageSize);
            Taxes = new ObservableCollection<TaxDTO>(list);
            FilterTaxes();
        }

        private async void NextPage(object parameter)
        {
            int totalPages = (int)Math.Ceiling(TotalTaxes / (double)_pageSize);
            if (PageNumber >= totalPages) return;

            PageNumber++;
            var list = await _apiTax.GetTaxesAsync(_pageNumber, _pageSize);
            Taxes = new ObservableCollection<TaxDTO>(list);
            FilterTaxes();
        }
    }
}