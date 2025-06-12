using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SuppliersCommands
{
    public class AddSupplierCommand : ICommand
    {
       

        private readonly VmSupplier _viewModel;

        public AddSupplierCommand(VmSupplier viewModel)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public bool CanExecute(object? parameter) => true;

        public async void Execute(object? parameter)
        {
            await _viewModel.AddSupplierAsync();
        }

        public event EventHandler? CanExecuteChanged;
    }

}
