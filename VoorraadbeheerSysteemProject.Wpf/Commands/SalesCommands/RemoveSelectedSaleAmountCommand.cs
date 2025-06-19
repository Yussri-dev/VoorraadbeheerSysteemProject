using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SalesCommands
{
    class RemoveSelectedSaleAmountCommand :ICommand
    {
        private readonly VmNumPadDataEntry _vmNumPad;
        public event EventHandler? CanExecuteChanged;

        public RemoveSelectedSaleAmountCommand(VmNumPadDataEntry vmNumPad)
        {
            _vmNumPad = vmNumPad;
        }
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (_vmNumPad.SelectedAmounts == null)
                return;

            _vmNumPad.SelectedAmounts.Remove(_vmNumPad.SelectedAmount);
            //_vmNumPad.SelectedAmounts = null;

            //_vmNumPad.CalculateTotalAmount();
            //if (_vmNumPad != null)
            //{
            //    if (_vmNumPad.SelectedAmounts == null)
            //        return;

            //    _vmNumPad.SelectedAmounts.Remove(_vmNumPad.SelectedAmount);
            //    _vmNumPad.SelectedAmounts = null;

            //    //_vmNumPad.SelectedAmounts();
            //}
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
