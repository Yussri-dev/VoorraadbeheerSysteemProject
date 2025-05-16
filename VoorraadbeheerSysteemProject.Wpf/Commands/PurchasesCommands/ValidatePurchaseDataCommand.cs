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
        private readonly VmPurchase _vmPurchase;
        //private readonly VmNumPadDataEntry _vmNumPad;

        public ValidatePurchaseDataCommand(VmPurchase vmPurchase)
        {
            _vmPurchase = vmPurchase;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public async void Execute(object? parameter)
        {
            //var window = parameter as Window;

            if (_vmPurchase.SelectedProducts.Count == 0)
            {
                MessageBox.Show("No purchase data to save.");
                return;
            }

            //decimal paidAmount = _vmNumPad.SelectedAmounts.Sum(s => s.AmountPrice);
            decimal totalAmount = _vmPurchase.TotalAmount;

            //if (paidAmount >= totalAmount)
            {

                var purchaseDto = new PurchaseDTO
                {
                    /*
                     {
                        "saleDate": "2025-02-17T21:07:13.144Z",
                        "SupplierId": 2,
                        "employeeId": 2,
                        "tvaAmount": 10,
                        "totalAmount": 110,
                        "amountPaid": 110,
                        "outstandingBalance": 0,
                        "SupplierName": "string",
                        "SaasClientId" : 1
}
                     */
                    PurchaseDate = DateTime.Now,
                    SupplierId = 2,
                    EmployeeId = 2,
                    TvaAmount = 0,
                    TotalAmount = totalAmount,
                    AmountPaid = totalAmount,
                    SupplierName = "Testing Supplier",
                    SaasClientId = 1
                };

                var result = await _vmPurchase.SavePurchaseAsync(purchaseDto);

                if (result == null)
                {
                    MessageBox.Show("❌ Failed to save sale.");
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
                    MessageBox.Show($"✅ Purchase and all items saved! Sale ID: {purchaseId}");

                    //_vmNumPad.ClearSaleAmountCommand?.Execute(null);
                    _vmPurchase.ClearSelectedProductCommand?.Execute(null);
                    //_vmNumPad.CloseWindowCommand.Execute(window);
                }
                else
                {
                    MessageBox.Show($"⚠️ Purchase saved (ID: {purchaseId}) but some items failed.");
                }
            }
        }
    }
}
