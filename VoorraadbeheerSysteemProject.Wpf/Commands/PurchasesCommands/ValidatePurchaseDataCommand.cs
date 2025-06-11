using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.PurchasesCommands
{
    class ValidatePurchaseDataCommand : ICommand
    {
        #region Fields & Events & Constructors
        private readonly VmPurchase _vmPurchase;

        public ValidatePurchaseDataCommand(VmPurchase vmPurchase)
        {
            _vmPurchase = vmPurchase;
        }
        public event EventHandler? CanExecuteChanged;
        #endregion

        #region Execute Commands
        public bool CanExecute(object? parameter) => true;

        public async void Execute(object? parameter)
        {
            //var window = parameter as Window;

            if (_vmPurchase.SelectedProducts.Count == 0)
            {
                MessageBox.Show("No purchase data to save.");
                return;
            }

            decimal totalAmount = _vmPurchase.TotalAmount;
            string userId = UserSession.IdUSer;
            int saasId = UserSession.IdSaasClient;
            var purchaseDto = new PurchaseDTO
            {
                PurchaseDate = DateTime.Now,
                SupplierId = 1,
                UserId = userId,
                TvaAmount = 0,
                TotalAmount = totalAmount,
                AmountPaid = totalAmount,
                SupplierName = "Supplier",
                SaasClientId = 1
            };

            var result = await _vmPurchase.SavePurchaseAsync(purchaseDto);

            if (result == null)
            {
                MessageBox.Show("❌ Failed to save Purchase.");
                return;
            }

            int purchaseId = result.PurchaseId;
            bool allItemsSaved = true;

            foreach (var item in _vmPurchase.SelectedProducts)
            {
                var purchaseItemDto = new PurchaseItemDTO
                {
                    PurchaseId = purchaseId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.AmountPrice,
                    Discount = 0,
                    TaxAmount = 10,
                    Total = item.AmountPrice,
                    DateCreated = DateTime.Now,
                    SaasClientId = 1
                };

                var itemResult = await _vmPurchase.SavePurchaseItemAsync(purchaseItemDto);
                if (itemResult == null)
                {
                    allItemsSaved = false;
                }
            }

            if (allItemsSaved)
            {
                MessageBox.Show($"✅ Purchase and all items saved! Purchase ID: {purchaseId}");

                _vmPurchase.ClearSelectedProductCommand?.Execute(null);
            }
            else
            {
                MessageBox.Show($"⚠️ Purchase saved (ID: {purchaseId}) but some items failed.");
            }
        }
        #endregion


    }
}
