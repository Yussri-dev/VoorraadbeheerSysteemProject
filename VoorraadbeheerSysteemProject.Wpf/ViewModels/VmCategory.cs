
using System.Collections.ObjectModel;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands.CategoriesCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{

    public class VmCategory : VmBase
    {
        private readonly ApiCategory _apiCategory = new();
        private string _searchText;
        private int _totalCategories;
        private string _newCategoryName;
        public ObservableCollection<CategoryDTO> Categories { get; set; }
        public ObservableCollection<CategoryDTO> FilteredCategories { get; set; }

        public VmCategory(NavigationStore navigationStore)
        {
            Categories = new ObservableCollection<CategoryDTO>();
            FilteredCategories = new ObservableCollection<CategoryDTO>();

            LoadCategories();

            UpdateCommand = new UpdateCommand(this);
            ResetCommand = new ResetCommand(this);
            CloseCommand = new CloseCommand(navigationStore);
            SearchCommand = new SearchCommand(this);
            AddCommand = new AddCommand(this);

        }


        public ICommand UpdateCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterCategories();
            }
        }

        public int TotalCategories
        {
            get => _totalCategories;
            set
            {
                _totalCategories = value;
                OnPropertyChanged();
            }
        }

        private async void LoadCategories()
        {
            var list = await _apiCategory.GetCategoriesAsync();

            Categories.Clear();
            FilteredCategories.Clear();

            int counter = 1;
            foreach (var cat in list)
            {
                cat.CategoryId = counter++;
                Categories.Add(cat);
                FilteredCategories.Add(cat);
            }

            TotalCategories = FilteredCategories.Count;
        }

        public void FilterCategories()
        {
            FilteredCategories.Clear();
            foreach (var cat in Categories)
            {
                if (string.IsNullOrWhiteSpace(SearchText) || cat.Name.ToLower().Contains(SearchText.ToLower()))
                {
                    FilteredCategories.Add(cat);
                }
            }

            TotalCategories = FilteredCategories.Count;

        }

        public async void RefreshCategories()
        {
            var list = await _apiCategory.GetCategoriesAsync();

            Categories.Clear();
            FilteredCategories.Clear();

            int counter = 1;
            foreach (var cat in list)
            {
                cat.CategoryId = counter++;
                Categories.Add(cat);
                FilteredCategories.Add(cat);
            }

            TotalCategories = FilteredCategories.Count;
        }


        public string NewCategoryName
        {
            get => _newCategoryName;
            set
            {
                _newCategoryName = value;
                OnPropertyChanged();
            }
        }



        // add New Category Name 

        public async Task AddCategoryAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCategoryName))
                return;

            var newCategory = new CategoryDTO { Name = NewCategoryName };
            await _apiCategory.PostCategoryAsync(newCategory);
            NewCategoryName = string.Empty;
            RefreshCategories();
        }

    }


}