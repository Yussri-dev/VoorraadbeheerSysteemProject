using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SalesCommands
{
    class ValidateSaleDataCommand : ICommand
    {
        private readonly VmSale _vmSale;
        private readonly VmNumPadDataEntry _vmNumPad;
        private List<SaleItemDTO> _savedSaleItems = new List<SaleItemDTO>();

        public ValidateSaleDataCommand(VmSale vmSale, VmNumPadDataEntry vmNumPad)
        {
            _vmSale = vmSale;
            _vmNumPad = vmNumPad;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        private void PrintTicket()
        {
            FlowDocument ticket = CreateTicketDocument(_savedSaleItems);

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                ticket.PageHeight = printDialog.PrintableAreaHeight;
                ticket.PageWidth = printDialog.PrintableAreaWidth;
                ticket.PagePadding = new Thickness(50);
                ticket.ColumnGap = 0;
                ticket.ColumnWidth = printDialog.PrintableAreaWidth;

                IDocumentPaginatorSource idpSource = ticket;
                printDialog.PrintDocument(idpSource.DocumentPaginator, "Ticket Print");
            }
        }

        private FlowDocument CreateTicketDocument(List<SaleItemDTO> items)
        {
            FlowDocument doc = new FlowDocument();

            // Header
            Paragraph header = new Paragraph(new Run("Ticket"))
            {
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            doc.Blocks.Add(header);

            // Customer info
            doc.Blocks.Add(new Paragraph(new Run("Naam: CUSTOMER NAME")));
            doc.Blocks.Add(new Paragraph(new Run("Datum: " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"))));

            // Separator
            doc.Blocks.Add(new Paragraph(new Run("-----------------------------")));

            // Sale items details
            foreach (var item in items)
            {
                var itemParagraph = new Paragraph();
                itemParagraph.Inlines.Add(new Run($"Qty:{item.Quantity} Product ID: {item.ProductName}  "));
                itemParagraph.Inlines.Add(new Run($"Price: {item.Price:C}  "));
                itemParagraph.Inlines.Add(new Run($"Total: {item.Total:C}"));
                doc.Blocks.Add(itemParagraph);
            }

            // Separator
            doc.Blocks.Add(new Paragraph(new Run("-----------------------------")));

            // Total amount
            decimal totalAmount = items.Sum(i => i.Total);
            Paragraph amount = new Paragraph(new Run($"Totaal: {totalAmount:C}"))
            {
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Right
            };
            doc.Blocks.Add(amount);

            return doc;
        }

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

                _savedSaleItems.Clear();

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
                    else
                    {
                        _savedSaleItems.Add(itemResult);
                    }
                }

                if (allItemsSaved)
                {
                    MessageBox.Show($"✅ Sale and all items saved! Sale ID: {saleId}");
                    _vmNumPad.ClearSaleAmountCommand?.Execute(null);
                    _vmSale.ClearSelectedProductCommand?.Execute(null);
                    _vmNumPad.CloseWindowCommand.Execute(window);
                    PrintTicket();
                }
                else
                {
                    MessageBox.Show($"⚠️ Sale saved (ID: {saleId}) but some items failed.");
                }
            }
            else
            {
                MessageBox.Show("Paid amount is less than total amount.");
            }
        }
    }
}
