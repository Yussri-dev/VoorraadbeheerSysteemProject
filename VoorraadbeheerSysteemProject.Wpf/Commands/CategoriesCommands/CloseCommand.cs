using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.CategoriesCommands
{
   
        public class CloseCommand : ICommand
        {
            private readonly NavigationStore _navigationStore;

            public CloseCommand(NavigationStore navigationStore)
            {
                _navigationStore = navigationStore;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                
                _navigationStore.CurrentViewModel = null;
       
            _navigationStore.CurrentViewModel = new VmDashboard(_navigationStore);
        }
    }
   }
