using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SuppliersCommands
{
    public class UpdateSupplierCommand : ICommand
    {
        private readonly VmSupplier _viewModel;

        public UpdateSupplierCommand(VmSupplier viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {

            _viewModel.RefreshSuppliers();
        }
    }
}
