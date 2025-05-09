using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.ProductsCommands
{
    class SearchProductsCommand : ICommand
    {
        private readonly VmSale _vmSale;
        public event EventHandler? CanExecuteChanged;
        public SearchProductsCommand(VmSale vmSale)
        {
            _vmSale = vmSale;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            _vmSale.FilterProducts();
        }
    }
}
