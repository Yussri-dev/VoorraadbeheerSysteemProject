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
    class AddSelectedProductToPurchaseCommand : ICommand
    {
        private readonly VmPurchase _vmPurchase;
        private readonly VmNumPadDataEntry _vmNumPad;

        public event EventHandler? CanExecuteChanged;
        public AddSelectedProductToPurchaseCommand(VmPurchase vmPurchase, VmNumPadDataEntry vmNumPad)
        {
            _vmPurchase = vmPurchase;
            _vmNumPad = vmNumPad;
        }
        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            var selected = _vmPurchase.SelectedProduct;
            if (selected == null)
                return;

            if (!decimal.TryParse(_vmNumPad.InputText, out var quantity) || quantity <= 0)
                return;

            // Vérifie si le produit est déjà dans la liste
            var existing = _vmPurchase.SelectedProducts.FirstOrDefault(p => p.ProductId == selected.ProductId);


            if (existing == null)
            {
                decimal totalPrice = quantity * selected.PurchasePrice;
                decimal taxAmount = Math.Round(selected.TaxRate * totalPrice, 2);

                _vmPurchase.SelectedProducts.Add(new ProductSelectedRequest
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
                existing.AmountPrice += quantity * selected.PurchasePrice;
            }
            _vmNumPad.InputText = string.Empty;
            _vmPurchase.InputSearchNameText = string.Empty;
            _vmPurchase.InputSearchBarcodeText = string.Empty;
            _vmPurchase.CalculateTotalAmount();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
