using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.DrawerCommands
{
    class ValidateCashShiftCommand : ICommand
    {
        private readonly VmDrawer _vmDrawer;

        public event EventHandler? CanExecuteChanged;

        public ValidateCashShiftCommand(VmDrawer vmDrawer)
        {
            _vmDrawer = vmDrawer;
        }
        public bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
