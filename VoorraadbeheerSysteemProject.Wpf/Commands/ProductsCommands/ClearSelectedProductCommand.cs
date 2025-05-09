using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.ProductsCommands
{
    class ClearSelectedProductCommand : ICommand
    {
        private readonly VmSale _vmSale;

        public ClearSelectedProductCommand(VmSale vmSale)
        {
            _vmSale = vmSale;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;
        

        public void Execute(object? parameter)
        {
            _vmSale.SelectedProducts.Clear();
            _vmSale.SelectedProductInCart = null;
            _vmSale.CalculateTotalAmount();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
