using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{

    public class VmReport : VmBase
    {
        private string _searchText;
        private ObservableCollection<Report> _filteredReports;
        private readonly ObservableCollection<Report> _allReports;
        private readonly NavigationStore _navigationStore;

        public VmReport(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            // voorbeelddata
            _allReports = new ObservableCollection<Report>
            {
                new Report { Number = 1, Name = "Product Name 1", Qty = 2, PurchasePrice = 20.50m, SalePrice = 30.00m, TaxRate = 10.00m, Customer = "Customer I",  SaleDate = "18/03/2025", Amount = 50.00m },
                new Report { Number = 2, Name = "Product Name 2", Qty = 25,PurchasePrice = 10.00m, SalePrice = 20.50m, TaxRate = 15.00m, Customer = "Customer II", SaleDate = "10/09/2025", Amount = 90.00m },
                new Report { Number = 3, Name = "Product Name 3", Qty = 10,PurchasePrice = 5.00m,  SalePrice = 15.00m, TaxRate = 5.00m,  Customer = "Customer III",SaleDate = "20/10/2025", Amount = 150.00m },
                new Report { Number = 4, Name = "Product Name 4", Qty = 5, PurchasePrice = 8.00m,  SalePrice = 12.00m, TaxRate = 8.00m,  Customer = "Customer IV", SaleDate = "15/11/2025", Amount = 60.00m }
            };

            _filteredReports = new ObservableCollection<Report>(_allReports);
        }

        public ObservableCollection<Report> FilteredReports
        {
            get => _filteredReports;
            set
            {
                _filteredReports = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalAmount));
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
                    UpdateFilteredReports();
                }
            }
        }

        public decimal TotalAmount => FilteredReports.Sum(r => r.Amount);

        private void UpdateFilteredReports()
        {
            if (string.IsNullOrWhiteSpace(_searchText))
            {
                FilteredReports = new ObservableCollection<Report>(_allReports);
            }
            else
            {
                var lower = _searchText.ToLower();
                var matches = _allReports
                    .Where(r => r.Name.ToLower().Contains(lower))
                    .ToList();
                FilteredReports = new ObservableCollection<Report>(matches);
            }

            OnPropertyChanged(nameof(TotalAmount));
        }



        public ObservableCollection<string> Customers { get; set; } = new ObservableCollection<string>
{
          "Customer I",
          "Customer II",
          "Customer III",
          "Customer IV"
};

        public ObservableCollection<string> AvailableDates { get; set; } = new ObservableCollection<string>
{
           "01/01/2025",
           "15/02/2025",
           "10/03/2025",
           "20/04/2025"
};

        public string SelectedCustomer { get; set; }
        public string SelectedStartDate { get; set; }
        public string SelectedEndDate { get; set; }


        public class Report
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public int Qty { get; set; }
            public decimal PurchasePrice { get; set; }
            public decimal SalePrice { get; set; }
            public decimal TaxRate { get; set; }
            public string Customer { get; set; }
            public string SaleDate { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
