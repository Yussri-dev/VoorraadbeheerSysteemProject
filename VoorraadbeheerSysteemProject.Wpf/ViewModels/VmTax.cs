using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{

    public class VmTax : VmBase
    {
        private string _searchText;
        private string _totalTax;
        private ObservableCollection<Tax> _filteredTax;

        public VmTax(NavigationStore navigationStore)
        {

            _filteredTax = new ObservableCollection<Tax>
            {
                new Tax { Number = 1, Name = "21%" },
                new Tax { Number = 2, Name = "9%" },
                new Tax { Number = 3, Name = "0%" },
                new Tax { Number = 4, Name = "10%" }
            };

            _totalTax = "4";
        }

        public ObservableCollection<Tax> FilteredTax
        {
            get => _filteredTax;
            set
            {
                _filteredTax = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();

            }
        }

        public string TotalTax
        {
            get => _totalTax;
            set
            {
                _totalTax = value;
                OnPropertyChanged();
            }
        }


        public class Tax
        {
            public int Number { get; set; }
            public string Name { get; set; }
        }
    }
}


