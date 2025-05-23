﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;
using VoorraadbeheerSysteemProject.Wpf.Views;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SalesCommands
{
    class ValidateSaleDataCommand : ICommand
    {
        private readonly VmSale _vmSale;
        private readonly VmNumPadDataEntry _vmNumPad;

        public ValidateSaleDataCommand(VmSale vmSale, VmNumPadDataEntry vmNumPad)
        {
            _vmSale = vmSale;
            _vmNumPad = vmNumPad;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public async void Execute(object? parameter)
        {
            var window = parameter as Window;

            if (_vmNumPad.SelectedAmounts.Count == 0)
            {
                MessageBox.Show("No sale data to save.");
                return;
            }

            decimal paidAmount = _vmNumPad.SelectedAmounts.Sum(s => s.AmountPrice);
            decimal totalAmount = _vmSale.TotalAmount;

            if (paidAmount >= totalAmount)
            {

                var saleDto = new SaleDTO
                {
                    SaleDate = DateTime.Now,
                    CustomerId = 1,
                    EmployeeId = 2,
                    TotalAmount = totalAmount,
                    AmountPaid = paidAmount,
                    TvaAmount = 0,
                    DiscountPercentage = 0,
                    CustomerName = "Testing Customer",
                    SaasClientId = 1
                };

                var result = await _vmNumPad.SaveSaleAsync(saleDto);

                if (result == null)
                {
                    MessageBox.Show("❌ Failed to save sale.");
                    return;
                }

                int saleId = result.SaleId;
                bool allItemsSaved = true;

                foreach (var item in _vmSale.SelectedProducts)
                {
                    var saleItemDto = new SaleItemDTO
                    {
                        SaleId = saleId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.AmountPrice,
                        ProfitMarge = 0,
                        PurchasePrice = 10,
                        Discount = 0,
                        TaxAmount = 10,
                        Total = item.AmountPrice,
                        DateCreated = DateTime.Now,
                        SaasClientId = 1
                    };

                    var itemResult = await _vmNumPad.SaveSaleItemAsync(saleItemDto);
                    if (itemResult == null)
                    {
                        allItemsSaved = false;
                    }
                }

                if (allItemsSaved)
                {
                    MessageBox.Show($"✅ Sale and all items saved! Sale ID: {saleId}");

                    _vmNumPad.ClearSaleAmountCommand?.Execute(null);
                    _vmSale.ClearSelectedProductCommand?.Execute(null);
                    _vmNumPad.CloseWindowCommand.Execute(window);
                }
                else
                {
                    MessageBox.Show($"⚠️ Sale saved (ID: {saleId}) but some items failed.");
                }
            }
        }
    }
}
