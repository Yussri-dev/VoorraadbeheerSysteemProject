using System;
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
            if (_vmNumPad.SelectedAmounts.Count == 0)
            {
                MessageBox.Show("No sale data to save.");
                return;
            }

            decimal totalAmount = _vmNumPad.SelectedAmounts.Sum(s => s.AmountPrice);

            var saleDto = new SaleDTO
            {
                SaleDate = DateTime.Now,
                CustomerId = 1, 
                EmployeeId = 2,
                TotalAmount = _vmNumPad.TotalAmount,
                AmountPaid = _vmNumPad.TotalAmount,
                TvaAmount = 0,
                DiscountPercentage = 0,
                CustomerName = "Testing Customer",
                SaasClientId = 1
            };

            var result = await _vmNumPad.SaveSaleAsync(saleDto);

            if (result != null)
            {
                MessageBox.Show($"✅ Sale saved! ID: {result.SaleId}");
            }
            else
            {
                MessageBox.Show("❌ Failed to save sale.");
            }
        }

    }
}
