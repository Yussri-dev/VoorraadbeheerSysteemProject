using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmSupplier : VmBase
    {
        private string _searchText;
        private ObservableCollection<Supplier> _filteredSuppliers;
        private ObservableCollection<Supplier> _allSuppliers;
        private readonly NavigationStore _navigationStore;

        public VmSupplier(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            // voorbeeldleveranciers
            _allSuppliers = new ObservableCollection<Supplier>
            {
                    new Supplier { Number = 1, Name = " Supplier Name 1", Number1 = "0321", Number2 = "9876", Email = "email1@gmail.com", Created = "2023-01-12" },
                new Supplier { Number = 2, Name = " Supplier Name 2", Number1 = "0456", Number2 = "1234", Email =  "email2@gmail.com", Created = "2023-03-22" },
                new Supplier { Number = 3, Name = " Supplier Name 3", Number1 = "0789", Number2 = "4321", Email =  "email3@gmail.com", Created = "2024-07-10" },
                new Supplier { Number = 4, Name = " Supplier Name 4", Number1 = "0123", Number2 = "6543", Email =  "email4@gmail.com", Created = "2025-01-05" }
            };

            FilteredSuppliers = new ObservableCollection<Supplier>(_allSuppliers);
        }

        public ObservableCollection<Supplier> FilteredSuppliers
        {
            get => _filteredSuppliers;
            set
            {
                _filteredSuppliers = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalSuppliers));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilterSuppliers();
                }
            }
        }

        public int TotalSuppliers => FilteredSuppliers?.Count ?? 0;

        private void FilterSuppliers()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
            {
                FilteredSuppliers = new ObservableCollection<Supplier>(_allSuppliers);
            }
            else
            {
                var lowerSearch = _searchText.ToLower();
                var result = _allSuppliers
                    .Where(s => s.Name.ToLower().Contains(lowerSearch))
                    .ToList();

                FilteredSuppliers = new ObservableCollection<Supplier>(result);
            }
        }

        public class Supplier
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public string Number1 { get; set; }
            public string Number2 { get; set; }
            public string Email { get; set; }
            public string Created { get; set; }
        }

    }
}
