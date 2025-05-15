using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SalesCommands
{
    class ClearSaleAmountCommand : ICommand
    {
        private readonly VmNumPadDataEntry _vmNumPad;

        public ClearSaleAmountCommand(VmNumPadDataEntry vmNumPad)
        {
            _vmNumPad = vmNumPad;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;


        public void Execute(object? parameter)
        {
            _vmNumPad.SelectedAmounts.Clear();
            //_vmNumPad.SelectedAmounts = null;
            //_vmSale.CalculateTotalAmount();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
