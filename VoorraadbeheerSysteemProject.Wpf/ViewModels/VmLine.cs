using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Commands.LineCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
   
        public class VmLine : VmBase
        {
            private readonly ApiLine _apiLine;
            private string _searchText;
            private int _totalLines;
            private int _pageNumber = 1;
            private readonly int _pageSize = 15;
            private string _newLineName;
            private LineDTO _selectedLine;
            public string NewLine { get; set; }


        public ObservableCollection<LineDTO> Lines { get; set; }
            public ObservableCollection<LineDTO> FilteredLines { get; set; }

            public ApiLine ApiLine => _apiLine;

            public ICommand PreviousPageCommand { get; }
            public ICommand NextPageCommand { get; }

            // Commands
            public ICommand UpdateLineCommand { get; }
            public ICommand ResetLineCommand { get; }
            public ICommand NavigateDashboardCommand { get; }
            public ICommand AddLineCommand { get; }
            public ICommand DeleteLineCommand { get; }

            public VmLine(NavigationStore navigationStore)
            {
                Lines = new ObservableCollection<LineDTO>();
                FilteredLines = new ObservableCollection<LineDTO>();

                NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                    () => new VmDashboard(navigationStore));

            UpdateLineCommand = new UpdateLineCommand(this);
            ResetLineCommand = new ResetLineCommand(this);
            DeleteLineCommand = new DeleteLineCommand(this);
            AddLineCommand = new AddLineCommand(this);

            _apiLine = new ApiLine(AppConfig.ApiUrl);

            PreviousPageCommand = new ButtonCommand(PreviousPage);
                NextPageCommand = new ButtonCommand(NextPage);

                LoadLines();
            }

            public string SearchText
            {
                get => _searchText;
                set
                {
                    _searchText = value;
                    OnPropertyChanged();
                    FilterLines();
                }
            }

            public int TotalLines
            {
                get => _totalLines;
                set
                {
                    _totalLines = value;
                    OnPropertyChanged();
                }
            }

            private async void LoadLines()
            {
                var list = await _apiLine.GetLinesAsync(_pageNumber, _pageSize);

                Lines.Clear();
                FilteredLines.Clear();

                foreach (var line in list)
                {
                    Lines.Add(line);
                    FilteredLines.Add(line);
                }

                TotalLines = await _apiLine.GetLineCountAsync();
            }

            public void FilterLines()
            {
                FilteredLines.Clear();
                foreach (var line in Lines)
                {
                    if (string.IsNullOrWhiteSpace(SearchText) || line.Name.ToLower().Contains(SearchText.ToLower()))
                    {
                        FilteredLines.Add(line);
                    }
                }
                TotalLines = FilteredLines.Count;
            }

            public async void RefreshLines()
            {
                var list = await _apiLine.GetLinesAsync(_pageNumber, _pageSize);

                Lines.Clear();
                FilteredLines.Clear();

                foreach (var line in list)
                {
                    Lines.Add(line);
                    FilteredLines.Add(line);
                }

                TotalLines = await _apiLine.GetLineCountAsync();
            }

            public string NewLineName
            {
                get => _newLineName;
                set
                {
                    _newLineName = value;
                    OnPropertyChanged();
                }
            }

            public async Task AddLineAsync()
            {
                if (string.IsNullOrWhiteSpace(NewLineName))
                    return;

                var newLine = new LineDTO { Name = NewLineName };
                await _apiLine.PostLineAsync(newLine);
                NewLineName = string.Empty;
                RefreshLines();
            }

            public LineDTO SelectedLine
            {
                get => _selectedLine;
                set
                {
                    _selectedLine = value;
                    OnPropertyChanged();
                }
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
                var list = await _apiLine.GetLinesAsync(PageNumber, _pageSize);
                Lines = new ObservableCollection<LineDTO>(list);
                FilterLines();
            }

            private async void NextPage(object parameter)
            {
                int totalPages = (int)Math.Ceiling(TotalLines / (double)_pageSize);
                if (PageNumber >= totalPages) return;

                PageNumber++;
                var list = await _apiLine.GetLinesAsync(PageNumber, _pageSize);
                Lines = new ObservableCollection<LineDTO>(list);
                FilterLines();
            }
        }
    }


