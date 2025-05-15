using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SalesCommands
{
    class NumPadDataEntryCommand : ICommand
    {
        private readonly VmNumPadDataEntry _vmNumpad;
        private readonly string _number;

        public NumPadDataEntryCommand(VmNumPadDataEntry vmNumpad, string number)
        {
            _vmNumpad = vmNumpad;
            _number = number;

        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (_number == ".")
            {
                if (!_vmNumpad.InputText.Contains("."))
                {
                    _vmNumpad.InputText += _number;
                }
            }
            else
            {
                _vmNumpad.InputText += _number;
            }
        }
    }
}
