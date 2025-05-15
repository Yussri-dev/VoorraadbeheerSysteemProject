using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;
using VoorraadbeheerSysteemProject.Wpf.Views;

namespace VoorraadbeheerSysteemProject.Wpf.Commands
{
    class OpenDialogCommand : ICommand
    {
        private readonly VmSale _vmSale;
        private readonly VmNumPadDataEntry _vmnumPad;

        public OpenDialogCommand(VmSale vmSale, VmNumPadDataEntry vmnumPad)
        {
            _vmSale = vmSale;
            _vmnumPad = vmnumPad;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            var numPadVM = new VmNumPadDataEntry(_vmSale);

            var dialog = new SaleDataEntryDialog
            {
                DataContext = numPadVM
            };

            var totalAmount = numPadVM.GetTotalAmount();
            var LineCount = numPadVM.GetLineCount();
            var totalQuantity = numPadVM.GetTotalQuantity();

            if (_vmSale.TotalAmount != 0)
            {
                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    _vmnumPad.TotalAmount = totalAmount;
                    _vmnumPad.TotalQuantity = totalQuantity;
                    _vmnumPad.LineCount = LineCount;
                }
            }
        }
    }
}
