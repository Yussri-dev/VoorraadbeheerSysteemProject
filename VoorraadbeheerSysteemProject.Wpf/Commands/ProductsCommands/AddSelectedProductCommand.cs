using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Requests;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.ProductsCommands
{
    class AddSelectedProductCommand : ICommand
    {
        private readonly VmSale _vmSale;

        public event EventHandler? CanExecuteChanged;
        public AddSelectedProductCommand(VmSale vmSale)
        {
            _vmSale = vmSale;
        }
        public bool CanExecute(object? parameter) => true;
        //{
        //    return _vmSale.SelectedProduct != null;
        //}

        public void Execute(object? parameter)
        {
            var selected = _vmSale.SelectedProduct;
            var Quantity = Convert.ToDecimal(_vmSale.InputText);
            if (selected == null)
                return;

            // Prevent duplicate entries
            if (!_vmSale.SelectedProducts.Any(p => p.ProductId == selected.ProductId))
            {
                _vmSale.SelectedProducts.Add(new ProductSelectedRequest
                {
                    ProductId = selected.ProductId,
                    Name = selected.Name,
                    Quantity = Quantity
                });
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
