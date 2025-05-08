using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands
{
    class AppendNumberCommand : ICommand
    {
        private readonly VmSale _vmSale;
        private readonly string _number;

        public AppendNumberCommand(VmSale vmSale, string number)
        {
            _vmSale = vmSale;
            _number = number;

        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (_number == ".")
            {
                if (!_vmSale.InputText.Contains("."))
                {
                    _vmSale.InputText += _number;
                }
            }
            else
            {
                _vmSale.InputText += _number;
            }
        }
    }
}
