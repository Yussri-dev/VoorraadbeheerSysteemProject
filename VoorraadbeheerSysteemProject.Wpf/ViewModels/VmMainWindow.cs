using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    public class VmMainWindow : VmBase
    {
        private readonly NavigationStore _navigationStore;

        public VmBase CurrentViewModel => _navigationStore.CurrentViewModel;
        public ICommand ProductsNavigationCommand { get; }
        public ICommand DashboardNavigationCommand { get; }

        public VmMainWindow(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            ProductsNavigationCommand = new NavigationCommand<VmProducts>(navigationStore,
                () => new VmProducts(navigationStore));
            DashboardNavigationCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));
        }

        private void _navigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
