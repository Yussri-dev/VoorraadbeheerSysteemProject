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
        private readonly VmPurchase _vmPurchase;
        public event EventHandler? CanExecuteChanged;

        public RemoveSelectedProductCommand(VmSale vmSale)
        {
            _vmSale = vmSale;
        }

        public RemoveSelectedProductCommand(VmPurchase vmPurchase)
        {
            _vmPurchase = vmPurchase;
        }
        public bool CanExecute(object? parameter) => true;
        
        public void Execute(object? parameter)
        {
            if (_vmSale !=null)
            {
                if (_vmSale.SelectedProductInCart == null)
                    return;

                _vmSale.SelectedProducts.Remove(_vmSale.SelectedProductInCart);
                _vmSale.SelectedProductInCart = null;

                _vmSale.CalculateTotalAmount();
            }
            if (_vmPurchase != null)
            {
                if (_vmPurchase.SelectedProductInCart == null)
                    return;

                _vmPurchase.SelectedProducts.Remove(_vmPurchase.SelectedProductInCart);
                _vmPurchase.SelectedProductInCart = null;

                _vmPurchase.CalculateTotalAmount();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
