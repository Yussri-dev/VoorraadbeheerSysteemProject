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
        //    return _vmSale.SelectedProductInCart != null;
        //}

        public void Execute(object? parameter)
        {
            var selected = _vmSale.SelectedProduct;
            if (selected == null)
                return;

            if (!decimal.TryParse(_vmSale.InputText, out var quantity) || quantity <= 0)
                return;

            // Vérifie si le produit est déjà dans la liste
            var existing = _vmSale.SelectedProducts.FirstOrDefault(p => p.ProductId == selected.ProductId);


            if (existing == null)
            {
                _vmSale.SelectedProducts.Add(new ProductSelectedRequest
                {
                    ProductId = selected.ProductId,
                    Name = selected.Name,
                    Quantity = quantity,
                    SalePrice = selected.SalePrice1,
                    AmountPrice = quantity * selected.SalePrice1
                });
            }
            else
            {
                existing.Quantity += quantity;
                existing.AmountPrice += quantity * selected.SalePrice1;
            }
            _vmSale.InputText = string.Empty;
        }


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
