using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SuppliersCommands
{
    public class AddCommand : ICommand
    {
        private readonly VmSupplier _viewModel;

        public AddCommand(VmSupplier viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            // Eventueel validatie, bv. niet toevoegen als naam leeg is
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.AddSupplier();
        }

        public event EventHandler CanExecuteChanged;
    }

}
