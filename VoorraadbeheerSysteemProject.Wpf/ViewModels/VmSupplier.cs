using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands.SuppliersCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmSupplier : VmBase
    {
        private readonly ApiSupplier _apiSupplier = new();
        private string _searchText;
        private int _totalSuppliers;
        public ObservableCollection<SupplierDTO> Suppliers { get; set; }
        public ObservableCollection<SupplierDTO> FilteredSuppliers { get; set; }

        public VmSupplier(NavigationStore navigationStore)
        {
            Suppliers = new ObservableCollection<SupplierDTO>();
            FilteredSuppliers = new ObservableCollection<SupplierDTO>();

            LoadSuppliers();

            UpdateCommand = new UpdateCommand(this);
            ResetCommand = new ResetCommand(this);
            //CloseCommand = new CloseCommand(navigationStore);
            SearchCommand = new SearchCommand(this);
            ;

        }


        public ICommand UpdateCommand { get; }
        public ICommand ResetCommand { get; }
        //public ICommand CloseCommand { get; }
        public ICommand SearchCommand { get; }

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

        public async void LoadSuppliers()
        {
            var list = await _apiSupplier.GetSuppliersAsync();

            Suppliers.Clear();
            FilteredSuppliers.Clear();

            int counter = 1;
            foreach (var cat in list)
            {
                cat.SupplierId = counter++;
                Suppliers.Add(cat);
                FilteredSuppliers.Add(cat);
            }

            TotalSuppliers = FilteredSuppliers.Count;
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
            var list = await _apiSupplier.GetSuppliersAsync();

            Suppliers.Clear();
            FilteredSuppliers.Clear();

            int counter = 1;
            foreach (var cat in list)
            {
                cat.SupplierId = counter++;
                Suppliers.Add(cat);
                FilteredSuppliers.Add(cat);
            }

            TotalSuppliers = FilteredSuppliers.Count;
        }

    }


}