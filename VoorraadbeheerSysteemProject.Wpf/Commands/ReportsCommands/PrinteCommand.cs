using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.ReportsCommands
{
    public class PrintCommand : ICommand
    {
        private readonly VmReport _viewModel;

        public PrintCommand(VmReport viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            
            var doc = new FlowDocument();
            doc.PagePadding = new Thickness(50);
            doc.Blocks.Add(new Paragraph(new Run("Productrapport")));
            doc.Blocks.Add(new Paragraph(new Run(""))); 

            foreach (var product in _viewModel.FilteredProducts)
            {
                var info = $"Product: {product.Name} | Voorraad: {product.QuantityStock}";
                doc.Blocks.Add(new Paragraph(new Run(info)));
            }

            var pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Productrapport");
            }
        }
    }
}
