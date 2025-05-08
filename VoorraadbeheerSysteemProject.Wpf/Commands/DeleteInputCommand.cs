using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands
{
    class DeleteInputCommand : ICommand
    {
        private readonly VmSale _vmSale;

        public DeleteInputCommand(VmSale vmSale)
        {
            _vmSale = vmSale;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (!string.IsNullOrEmpty(_vmSale.InputText))
            {
                _vmSale.InputText = string.Empty;
            }
        }
    }
}
