using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Commands.SuppliersCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    
       
        public class VmSupplier : VmBase
        {
            //  Private fields
            private readonly ApiSupplier _apiSupplier = new();
            private string _searchText;
            private int _totalSuppliers;
            private int _pageNumber = 1;
            private readonly int _pageSize = 10;
            private SupplierDTO _selectedSupplier;

            // 🔹 Publieke collecties
            public ObservableCollection<SupplierDTO> Suppliers { get; set; }
            public ObservableCollection<SupplierDTO> FilteredSuppliers { get; set; }

            // API Accessor
            public ApiSupplier ApiSupplier => _apiSupplier;

            // Properties 
            public string NewSupplierName { get; set; }
            public string NewPhone1 { get; set; }
            public string NewPhone2 { get; set; }
            public string NewEmail { get; set; }
            public SupplierDTO NewSupplier { get; set; } = new SupplierDTO();

            //  Commands
            public ICommand UpdateCommand { get; }
            public ICommand ResetCommand { get; }
            public ICommand NavigateDashboardCommand { get; }
            //public ICommand SearchCommand { get; }
            public ICommand AddCommand { get; }
            public ICommand DeleteCommand { get; }
            public ICommand PreviousPageButtonCommand { get; }
            public ICommand NextPageButtonCommand { get; }

            //  constructor
            public VmSupplier(NavigationStore navigationStore)
            {
                Suppliers = new ObservableCollection<SupplierDTO>();
                FilteredSuppliers = new ObservableCollection<SupplierDTO>();

                NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                    () => new VmDashboard(navigationStore));

                UpdateCommand = new UpdateCommand(this);
                ResetCommand = new ResetCommand(this);
                //SearchCommand = new SearchCommand(this);
                AddCommand = new AddCommand(this);
                DeleteCommand = new DeleteCommand(this);
                PreviousPageButtonCommand = new ButtonCommand(PreviousPage);
                NextPageButtonCommand = new ButtonCommand(NextPage);

                LoadSuppliers();
            }

            //  Properties
            public string SearchText
            {
                get => _searchText;
                set
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilterSuppliers();
                }
            }

            public int TotalSuppliers
            {
                get => _totalSuppliers;
                set
                {
                    _totalSuppliers = value;
                    OnPropertyChanged();
                }
            }

            public SupplierDTO SelectedSupplier
            {
                get => _selectedSupplier;
                set
                {
                    _selectedSupplier = value;
                    OnPropertyChanged();
                }
            }

            //  Data-ophalen en filtering
            public async void LoadSuppliers()
            {
                var list = await _apiSupplier.GetSuppliersAsync(_pageNumber);
                Suppliers.Clear();
                FilteredSuppliers.Clear();

                int counter = (_pageNumber - 1) * _pageSize + 1;
                foreach (var cat in list)
                {
                    cat.SupplierId = counter++;
                    Suppliers.Add(cat);
                    FilteredSuppliers.Add(cat);
                }

                TotalSuppliers = await _apiSupplier.GetSuppliersCountAsync();
            }

            public void FilterSuppliers()
            {
                FilteredSuppliers.Clear();
                foreach (var cat in Suppliers)
                {
                    if (string.IsNullOrWhiteSpace(SearchText) || cat.Name.ToLower().Contains(SearchText.ToLower()))
                    {
                        FilteredSuppliers.Add(cat);
                    }
                }

                TotalSuppliers = FilteredSuppliers.Count;
            }

            public async void RefreshSuppliers()
            {
                var list = await _apiSupplier.GetSuppliersAsync(_pageNumber);
                Suppliers.Clear();
                FilteredSuppliers.Clear();

                int counter = (_pageNumber - 1) * _pageSize + 1;
                foreach (var cat in list)
                {
                    cat.SupplierId = counter++;
                    Suppliers.Add(cat);
                    FilteredSuppliers.Add(cat);
                }

                TotalSuppliers = await _apiSupplier.GetSupplierCountAsync();
            }

            // CRUD – Toevoegen
            public async void AddSupplier()
            {
                var newSupplier = new SupplierDTO
                {
                    Name = NewSupplierName,
                    PhoneNumber1 = NewPhone1,
                    PhoneNumber2 = NewPhone2,
                    Email = NewEmail,
                    DateCreated = DateTime.Now
                };

                await _apiSupplier.PostSuppliersAsync(newSupplier);
                RefreshSuppliers();
            }

            //  Navigatie – Pagina’s wisselen
            private async void PreviousPage(object parameter)
            {
                if (_pageNumber <= 1) return;

                _pageNumber--;
                var list = await _apiSupplier.GetSuppliersAsync(_pageNumber);
                Suppliers = new ObservableCollection<SupplierDTO>(list);
                FilterSuppliers();
            }

            private async void NextPage(object parameter)
            {
                int totalPages = (int)Math.Ceiling(TotalSuppliers / (double)_pageSize);
                if (_pageNumber >= totalPages) return;

                _pageNumber++;
                var list = await _apiSupplier.GetSuppliersAsync(_pageNumber);
                Suppliers = new ObservableCollection<SupplierDTO>(list);
                FilterSuppliers();
            }
        }
    }



