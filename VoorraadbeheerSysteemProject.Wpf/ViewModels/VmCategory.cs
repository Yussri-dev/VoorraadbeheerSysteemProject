using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
   
        public class VmCategory : VmBase

        {


             private string _searchText;
            private ObservableCollection<Category> _categories;
            private int _totalCategories;

            public VmCategory(NavigationStore navigationStore)
            {
                Categories = new ObservableCollection<Category>
            {
                new Category { Number = 1, Name = "category 1" },
                new Category { Number = 2, Name = "category 2" },
                new Category { Number = 3, Name = "category 3" },
                new Category { Number = 4, Name = "category 4" }
            };


        }

            public string SearchText
            {
                get => _searchText;
                set
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    OnPropertyChanged(nameof(FilteredCategories));
                }
            }

            public ObservableCollection<Category> Categories
            {
                get => _categories;
                set
                {
                    _categories = value;
                    OnPropertyChanged(nameof(Categories));
                    TotalCategories = _categories.Count;
                }
            }

            public ObservableCollection<Category> FilteredCategories
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                        return Categories;
                    return new ObservableCollection<Category>(Categories.Where(c => c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)));
                }
            }

            public int TotalCategories
            {
                get => _totalCategories;
                set
                {
                    _totalCategories = value;
                    OnPropertyChanged(nameof(TotalCategories));
                }
            }

       
    }



    public class Category
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }


}

