
using System.Collections.ObjectModel;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
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
        private int _pageNumber = 1;
        private readonly int _pageSize = 10; 
        private string _newCategoryName;
        private CategoryDTO _selectedCategory;

        public ObservableCollection<CategoryDTO> Categories { get; set; }
        public ObservableCollection<CategoryDTO> FilteredCategories { get; set; }

        public ApiCategory ApiCategory => _apiCategory;

        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }

        // commands
        public ICommand UpdateCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand NavigateDashboardCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public VmCategory(NavigationStore navigationStore)
        {
            Categories = new ObservableCollection<CategoryDTO>();
            FilteredCategories = new ObservableCollection<CategoryDTO>();

            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));

            UpdateCommand = new UpdateCommand(this);
            ResetCommand = new ResetCommand(this);
            DeleteCategoryCommand = new DeleteCategoryCommand(this);
            SearchCommand = new SearchCommand(this);
            AddCommand = new AddCommand(this);

            PreviousPageCommand = new ButtonCommand(PreviousPage);
            NextPageCommand = new ButtonCommand(NextPage);

            LoadCategories();
        }

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
            var list = await _apiCategory.GetCategoriesAsync(_pageNumber, _pageSize);

            Categories.Clear();
            FilteredCategories.Clear();

            int counter = (_pageNumber - 1) * _pageSize + 1;
            foreach (var cat in list)
            {
                cat.CategoryId = counter++;
                Categories.Add(cat);
                FilteredCategories.Add(cat);
            }

            TotalCategories = await _apiCategory.GetCategoryCountAsync();
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
            var list = await _apiCategory.GetCategoriesAsync(_pageNumber, _pageSize);

            Categories.Clear();
            FilteredCategories.Clear();

            int counter = (_pageNumber - 1) * _pageSize + 1;
            foreach (var cat in list)
            {
                cat.CategoryId = counter++;
                Categories.Add(cat);
                FilteredCategories.Add(cat);
            }

            TotalCategories = await _apiCategory.GetCategoryCountAsync();
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

        public async Task AddCategoryAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCategoryName))
                return;

            var newCategory = new CategoryDTO { Name = NewCategoryName };
            await _apiCategory.PostCategoryAsync(newCategory);
            NewCategoryName = string.Empty;
            RefreshCategories();
        }

        public CategoryDTO SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        // Pagina navigatie methodes
        private async void PreviousPage(object parameter)
        {
            if (_pageNumber <= 1) return;

            _pageNumber--;
            var list = await _apiCategory.GetCategoriesAsync(_pageNumber, _pageSize);
            Categories = new ObservableCollection<CategoryDTO>(list);
            FilterCategories();
        }

        private async void NextPage(object parameter)
        {
            int totalPages = (int)Math.Ceiling(TotalCategories / (double)_pageSize);
            if (_pageNumber >= totalPages) return;

            _pageNumber++;
            var list = await _apiCategory.GetCategoriesAsync(_pageNumber, _pageSize);
            Categories = new ObservableCollection<CategoryDTO>(list);
            FilterCategories();
        }
    }
}