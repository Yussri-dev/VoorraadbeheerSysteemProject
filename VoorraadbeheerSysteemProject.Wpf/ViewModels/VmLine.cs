using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmLine : VmBase
    {
        private string _searchText;
        private int _totalCategories;
        private ObservableCollection<LineModel> _filteredLine;
        private ObservableCollection<LineModel> _allLines;

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

        public int TotalCategories
        {
            get => _totalCategories;
            set
            {
                _totalCategories = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LineModel> FilteredLine
        {
            get => _filteredLine;
            set
            {
                _filteredLine = value;
                OnPropertyChanged();
            }
        }

        public VmLine(NavigationStore navigationStore)
        {
            
            _allLines = new ObservableCollection<LineModel>
            {
                new LineModel { Number = 1, Name = "Line name 1" },
                new LineModel { Number = 2, Name = "Line name 2" },
                new LineModel { Number = 3, Name = "Line name 3" },
                new LineModel { Number = 4, Name = "Line name 4" },
            };

            FilteredLine = new ObservableCollection<LineModel>(_allLines);
            TotalCategories = _allLines.Count;
        }

        private void FilterLines()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredLine = new ObservableCollection<LineModel>(_allLines);
            }
            else
            {
                FilteredLine = new ObservableCollection<LineModel>(
                    _allLines.Where(l => l.Name.ToLower().Contains(SearchText.ToLower()))
                );
            }

            TotalCategories = FilteredLine.Count;
        }

        
    }
    public class LineModel
    {
        public int Number { get; set; }
        public string Name { get; set; }
    }
}

