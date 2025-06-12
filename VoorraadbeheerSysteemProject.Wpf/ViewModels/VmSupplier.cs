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
using VoorraadbeheerSysteemProject.Wpf.Views;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    
       
        public class VmSupplier : VmBase
        {
            //  Private fields
            private readonly ApiSupplier _apiSupplier ;
            private readonly NavigationStore _navigationStore;

            private string _searchText;
            private int _totalSuppliers;
            private int _pageNumber = 1;
            private readonly int _pageSize = 200;
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
            public ICommand UpdateSupplierCommand { get; }
            public ICommand ResetCommand { get; }
            public ICommand NavigateDashboardCommand { get; }
            //public ICommand SearchCommand { get; }
            public ICommand AddSupplierCommand { get; }
            public ICommand DeleteCommand { get; }
            public ICommand PreviousPageCommand { get; }
            public ICommand NextPageCommand { get; }

            //  constructor
            public VmSupplier(NavigationStore navigationStore)
            {
                Suppliers = new ObservableCollection<SupplierDTO>();
                FilteredSuppliers = new ObservableCollection<SupplierDTO>();

                NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                    () => new VmDashboard(navigationStore));

            UpdateSupplierCommand = new UpdateSupplierCommand(this);
            ResetCommand = new ResetSupplierCommand(this);
            //SearchCommand = new SearchCommand(this);
            AddSupplierCommand = new AddSupplierCommand(this);
            _apiSupplier = new ApiSupplier(AppConfig.ApiUrl);
             DeleteCommand = new DeleteSupplierCommand(this);
            PreviousPageCommand = new ButtonCommand(PreviousPage);
            NextPageCommand = new ButtonCommand(NextPage);

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
                var list = await _apiSupplier.GetSuppliersAsync(_pageNumber, _pageSize);
                Suppliers.Clear();
                FilteredSuppliers.Clear();

            foreach (var cat in list)
            {
                Suppliers.Add(cat);
                FilteredSuppliers.Add(cat);
            }

            TotalSuppliers = await _apiSupplier.GetSupplierCountAsync();
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
                var list = await _apiSupplier.GetSuppliersAsync(_pageNumber, _pageSize);
                Suppliers.Clear();
                FilteredSuppliers.Clear();

            foreach (var cat in list)
            {
                Suppliers.Add(cat);
                FilteredSuppliers.Add(cat);
            }

            TotalSuppliers = await _apiSupplier.GetSupplierCountAsync();
            }

        //  Toevoegen

        public async Task AddSupplierAsync()
        {
            var newSupplier = new SupplierDTO
            {
                Name = NewSupplierName,
                PhoneNumber1 = NewPhone1,
                PhoneNumber2 = NewPhone2,
                Email = NewEmail,
                DateCreated = DateTime.Now
            };

            Console.WriteLine($"Toevoegen Supplier met Naam: {newSupplier.Name}, Telefoon1: {newSupplier.PhoneNumber1}, Email: {newSupplier.Email}");

            var result = await _apiSupplier.PostSupplierAsync(newSupplier);

            if (!result)
            {
                Console.WriteLine("POST mislukt – controleer of alle verplichte velden gevuld zijn.");
                
            }
            else
            {
                
                RefreshSuppliers();
               
                NewSupplierName = string.Empty;
                NewPhone1 = string.Empty;
                NewPhone2 = string.Empty;
                NewEmail = string.Empty;
                OnPropertyChanged(nameof(NewSupplierName));
                OnPropertyChanged(nameof(NewPhone1));
                OnPropertyChanged(nameof(NewPhone2));
                OnPropertyChanged(nameof(NewEmail));
            }
        }

        //property

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


        //  Navigatie – Pagina’s wisselen
        private async void PreviousPage(object parameter)
            {
                if (_pageNumber <= 1) return;

                _pageNumber--;
                var list = await _apiSupplier.GetSuppliersAsync(_pageNumber, _pageSize);
                Suppliers = new ObservableCollection<SupplierDTO>(list);
                FilterSuppliers();
            }

            private async void NextPage(object parameter)
            {
                int totalPages = (int)Math.Ceiling(TotalSuppliers / (double)_pageSize);
                if (_pageNumber >= totalPages) return;

                _pageNumber++;
                var list = await _apiSupplier.GetSuppliersAsync(_pageNumber , _pageSize);
                Suppliers = new ObservableCollection<SupplierDTO>(list);
                FilterSuppliers();
            }
        }
    }



