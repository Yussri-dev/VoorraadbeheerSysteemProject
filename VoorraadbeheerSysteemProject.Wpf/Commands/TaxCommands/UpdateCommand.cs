using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.TaxCommands
{
    internal class UpdateCommand : ICommand
    {
        private readonly VmTax _viewModel;

        public UpdateCommand(VmTax viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {

        }
   
    }
}
