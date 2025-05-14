using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.ProductsCommands
{
    class RemoveSelectedProductCommand : ICommand
    {
        private readonly VmSale _vmSale;
        public event EventHandler? CanExecuteChanged;

        public RemoveSelectedProductCommand(VmSale vmSale)
        {
            _vmSale = vmSale;
        }
        public bool CanExecute(object? parameter) => true;
        
        public void Execute(object? parameter)
        {
            if (_vmSale.SelectedProductInCart == null)
                return;

            _vmSale.SelectedProducts.Remove(_vmSale.SelectedProductInCart);
            _vmSale.SelectedProductInCart = null;

            _vmSale.CalculateTotalAmount();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
