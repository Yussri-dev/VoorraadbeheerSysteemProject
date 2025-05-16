using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Requests;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SalesCommands
{
    class AddSaleAmountCommand : ICommand
    {
        private readonly VmNumPadDataEntry _vmNumPad;

        public event EventHandler? CanExecuteChanged;
        public AddSaleAmountCommand(VmNumPadDataEntry vmNumPad)
        {
            _vmNumPad = vmNumPad;
        }
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (!decimal.TryParse(_vmNumPad.InputText, out var amountPrice) || amountPrice <= 0)
                return;

            _vmNumPad.SelectedAmounts.Add(new SaleSelectedAmountRequest
            {
                AmountPrice = amountPrice
            });

            _vmNumPad.InputText = string.Empty;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
