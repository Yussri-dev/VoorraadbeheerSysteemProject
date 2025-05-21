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
        private readonly VmPurchase _vmPurchase;

        public ClearSelectedProductCommand(VmSale vmSale)
        {
            _vmSale = vmSale;
        }

        public ClearSelectedProductCommand(VmPurchase vmPurchase)
        {
            _vmPurchase = vmPurchase;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;
        

        public void Execute(object? parameter)
        {
            if (_vmSale != null)
            {
                _vmSale.SelectedProducts.Clear();
                _vmSale.SelectedProductInCart = null;
                _vmSale.CalculateTotalAmount();
            }

            if (_vmPurchase != null)
            {
                _vmPurchase.SelectedProducts.Clear();
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
