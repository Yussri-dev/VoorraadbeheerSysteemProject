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
            if (_vmNumPad.SelectedAmount == null)
                return;

            //_vmNumPad.SelectedAmount.Remove(_vmNumPad.SelectedAmount);
            //_vmNumPad.SelectedProductInCart = null;

            //_vmNumPad.CalculateTotalAmount();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
