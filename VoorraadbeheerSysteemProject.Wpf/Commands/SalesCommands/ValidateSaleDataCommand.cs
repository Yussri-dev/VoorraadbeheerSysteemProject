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
using System.Windows.Xps;
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

            // Mise en page du document
            ticket.PageHeight = 500;
            ticket.PageWidth = 350;
            ticket.PagePadding = new Thickness(20);
            ticket.ColumnGap = 0;
            ticket.ColumnWidth = 350;

            // Sélectionne l'imprimante par défaut
            LocalPrintServer printServer = new LocalPrintServer();
            PrintQueue defaultPrintQueue = printServer.DefaultPrintQueue;

            // Prépare le paginator
            IDocumentPaginatorSource idpSource = ticket;

            // Imprime sans dialogue
            XpsDocumentWriter xpsWriter = PrintQueue.CreateXpsDocumentWriter(defaultPrintQueue);
            xpsWriter.Write(idpSource.DocumentPaginator);
        }


        private FlowDocument CreateTicketDocument(List<SaleItemDTO> items)
        {
            FlowDocument doc = new FlowDocument
            {
                FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                FontSize = 12,
                PagePadding = new Thickness(20)
            };

            doc.Blocks.Add(new Paragraph(new Run("***Supermarkt B.V.***"))
            {
                TextAlignment = TextAlignment.Center,
                FontWeight = FontWeights.Bold
            });
            doc.Blocks.Add(new Paragraph(new Run("Amsterdam")) { TextAlignment = TextAlignment.Center });
            doc.Blocks.Add(new Paragraph(new Run("KvK: 12345678")) { TextAlignment = TextAlignment.Center });
            doc.Blocks.Add(new Paragraph(new Run("Tel. Winkel 020-1234567")) { TextAlignment = TextAlignment.Center });
            doc.Blocks.Add(new Paragraph(new Run("Klantenservice 0800-1234567")) { TextAlignment = TextAlignment.Center });
            doc.Blocks.Add(new Paragraph(new Run(new string('*', 40))) { TextAlignment = TextAlignment.Center });

            string klantNaam = _vmSale.SelectedCustomer.Name ?? "Klant";

            doc.Blocks.Add(new Paragraph(new Run($"Klant: {klantNaam}")));
            doc.Blocks.Add(new Paragraph(new Run($"Datum: {DateTime.Now:dd-MM-yyyy HH:mm}")));

            doc.Blocks.Add(new Paragraph(new Run(new string('-', 40))));

            // Utilisez le même format pour l'en-tête et les lignes
            string dataHeader = string.Format("{0,2} {1,8} {2,-20} {3,8}", "Aant", "Prijs", "Product", "Totaal");
            doc.Blocks.Add(new Paragraph(new Run(dataHeader)));

            foreach (var item in items)
            {
                string line = string.Format("{0,2} {1,8:0.00} {2,-20} {3,8:0.00}",
                    item.Quantity,
                    item.Price / (item.Quantity == 0 ? 1 : item.Quantity),
                    Truncate(item.ProductName, 20),
                    item.Total);
                doc.Blocks.Add(new Paragraph(new Run(line)));
            }

            doc.Blocks.Add(new Paragraph(new Run(new string('*', 40))));

            decimal totaalBedrag = items.Sum(i => i.Total);
            Paragraph totaal = new Paragraph(new Run($"TOTAAL: {totaalBedrag,30:0.00}"))
            {
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                TextAlignment = TextAlignment.Right
            };
            doc.Blocks.Add(totaal);

            return doc;
        }

        // Petite méthode utilitaire pour tronquer les noms longs
        private string Truncate(string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
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

            int customerId = _vmSale.SelectedCustomer.CustomerId;
            
            string userId = UserSession.IdUSer;

            if (_vmSale.SelectedCustomer == null || _vmSale.SelectedCustomer.CustomerId == 0)
            {
                MessageBox.Show("No Client Selected.");
                return;
            }
            if (paidAmount >= totalAmount)
            {
                var saleDto = new SaleDTO
                {
                    SaleDate = DateTime.Now,
                    CustomerId = customerId,
                    //EmployeeId = 2,
                    UserId = userId,
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
