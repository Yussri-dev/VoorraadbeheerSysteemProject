using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmProducts : VmBase
    {
        public VmProducts(NavigationStore navigationStore)
        {

            //NavigateDataCommand = new NavigationCommand<vmLogin>(navigationStore,
            //    () => new vmLogin(navigationStore));




            //temp filling of products
            Products = new List<Product>()
            {
                new Product()
                {
                    Number = 1,
                    Name = "Product 1",
                    Purchase = 10.0,
                    Sale1 = 15.0,
                    Sale2 = 20.0,
                    TaxRate = "21%",
                    Category = "Category 1",
                    Barcode = "1234567890123"
                },
                new Product()
                {
                    Number = 2,
                    Name = "Product 2",
                    Purchase = 20.0,
                    Sale1 = 25.0,
                    Sale2 = 30.0,
                    TaxRate = "6%",
                    Category = "Category 2",
                    Barcode = "2345678901234"
                },
                new Product()
                {
                    Number = 3,
                    Name = "Product 3",
                    Purchase = 30.0,
                    Sale1 = 35.0,
                    Sale2 = 40.0,
                    TaxRate = "21%",
                    Category = "Category 3",
                    Barcode = "3456789012345"
                },
                new Product()
                {
                    Number = 4,
                    Name = "Product 4",
                    Purchase = 40.0,
                    Sale1 = 45.0,
                    Sale2 = 50.0,
                    TaxRate = "6%",
                    Category = "Category 4",
                    Barcode = "4567890123456"
                }
            };


            //temp filling of categories
            AllCategories = new List<string> {
                "no category selected",
                "category 1",
                "category 2",
                "category 3",
            };
            FilteredCategories = AllCategories;

            //temp filling of tax rates
            AllTaxRate = new List<string> {
                "no tax rate selected",
                "6%",
                "21%"
            };
            FilteredTaxRate = AllTaxRate;

            //temp filling of shelves
            AllShelf = new List<string> {
                "no shelf selected",
                "shelf 1",
                "shelf 2",
                "shelf 3"
            };
            FilteredShelf = AllShelf;

        }

        public IList<Product> Products { get; set; }


        #region category filter
        private string _searchTextCategories;
        public string SearchTextCategories
        {
            get => _searchTextCategories;
            set
            {
                _searchTextCategories = value;
                OnPropertyChanged();
                FilterCategories();
            }
        }

        private IList<string> _allCategories;
        public IList<string> AllCategories
        {
            get => _allCategories;
            set { _allCategories = value; OnPropertyChanged(); }
        }

        private IList<string> _filteredCategories;
        public IList<string> FilteredCategories
        {
            get => _filteredCategories;
            set { _filteredCategories = value; OnPropertyChanged(); }
        }

        private void FilterCategories()
        {
            if (string.IsNullOrWhiteSpace(SearchTextCategories))
                FilteredCategories = new List<string>(AllCategories ?? new List<string>());
            else
            {
                FilteredCategories = AllCategories
                    .Where(items => items.ToLower()
                        .Contains(SearchTextCategories.ToLower()))
                    .ToList();
            }
        }
        #endregion

        #region tax rate filter
        private string _searchTextTaxRate;
        public string SearchTextTaxRate
        {
            get => _searchTextTaxRate;
            set
            {
                _searchTextTaxRate = value;
                OnPropertyChanged();
                FilterTaxRate();
            }
        }
        private IList<string> _filteredTaxRate;
        public IList<string> FilteredTaxRate
        {
            get => _filteredTaxRate;
            set { _filteredTaxRate = value; OnPropertyChanged(); }
        }

        private IList<string> _allTaxRate;
        public IList<string> AllTaxRate
        {
            get => _allTaxRate;
            set { _allTaxRate = value; OnPropertyChanged(); }
        }
        public void FilterTaxRate()
        {
            if (string.IsNullOrWhiteSpace(SearchTextTaxRate))
                FilteredTaxRate = new List<string>(AllTaxRate ?? new List<string>());
            else
            {
                FilteredTaxRate = AllTaxRate
                    .Where(items => items.ToLower()
                        .Contains(SearchTextTaxRate.ToLower()))
                    .ToList();
            }
        }
        #endregion

        #region Shelf filter
        private string _searchTextShelf;
        public string SearchTextShelf
        {
            get => _searchTextShelf;
            set
            {
                _searchTextShelf = value;
                OnPropertyChanged();
                FilterShelf();
            }
        }
        private IList<string> _filteredShelf;
        public IList<string> FilteredShelf
        {
            get => _filteredShelf;
            set { _filteredShelf = value; OnPropertyChanged(); }
        }

        private IList<string> _allShelf;
        public IList<string> AllShelf
        {
            get => _allShelf;
            set { _allShelf = value; OnPropertyChanged(); }
        }
        public void FilterShelf()
        {
            if (string.IsNullOrWhiteSpace(SearchTextShelf))
                FilteredShelf = new List<string>(AllShelf ?? new List<string>());
            else
            {
                FilteredShelf = AllShelf
                    .Where(items => items.ToLower()
                        .Contains(SearchTextShelf.ToLower()))
                    .ToList();
            }
        }
        #endregion



    }

    //temp class
    public class Product
    {
        public int Number { get; set; }
        public string? Name { get; set; }
        public double Purchase { get; set; }
        public double Sale1 { get; set; }
        public double Sale2 { get; set; }
        public string? TaxRate { get; set; }
        public string? Category { get; set; }
        public string? Barcode { get; set; }
    }


}
