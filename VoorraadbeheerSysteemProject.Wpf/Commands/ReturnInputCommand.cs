using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands
{
    class ReturnInputCommand : ICommand
    {

        private readonly VmNumPadDataEntry _vmSale;

        public ReturnInputCommand(VmNumPadDataEntry vmSale)
        {
            _vmSale = vmSale;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (!string.IsNullOrEmpty(_vmSale.InputText))
            {
                _vmSale.InputText = _vmSale.InputText.Substring(0, _vmSale.InputText.Length - 1);
            }
        }
    }
}
