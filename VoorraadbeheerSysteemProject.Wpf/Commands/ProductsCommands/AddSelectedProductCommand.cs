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
        private readonly VmNumPadDataEntry _vmNumPad;

        public event EventHandler? CanExecuteChanged;
        public AddSelectedProductCommand(VmSale vmSale, VmNumPadDataEntry vmNumPad)
        {
            _vmSale = vmSale;
            _vmNumPad = vmNumPad;
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

            if (!decimal.TryParse(_vmNumPad.InputText, out var quantity) || quantity <= 0)
                return;

            // Vérifie si le produit est déjà dans la liste
            var existing = _vmSale.SelectedProducts.FirstOrDefault(p => p.ProductId == selected.ProductId);


            if (existing == null)
            {
                decimal totalPrice = quantity * selected.SalePrice1;
                decimal taxAmount = Math.Round(selected.TaxRate * totalPrice, 2);

                _vmSale.SelectedProducts.Add(new ProductSelectedRequest
                {
                    ProductId = selected.ProductId,
                    Name = selected.Name,
                    Quantity = quantity,
                    SalePrice = selected.SalePrice1,
                    AmountPrice = totalPrice,
                    PurchasePrice = selected.PurchasePrice,
                    TaxAmount = taxAmount
                });

            }
            else
            {
                existing.Quantity += quantity;
                existing.AmountPrice += quantity * selected.SalePrice1;
            }
            _vmNumPad.InputText = string.Empty;
            _vmSale.InputSearchNameText = string.Empty;
            _vmSale.InputSearchBarcodeText = string.Empty;
            _vmSale.CalculateTotalAmount();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
