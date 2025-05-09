using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;
using VoorraadbeheerSysteemProject.Wpf.Views;

namespace VoorraadbeheerSysteemProject.Wpf.Commands
{
    class ClosingCommand : ICommand
    {
        private readonly VmSale _vmSale;
        public event EventHandler? CanExecuteChanged;
        public ClosingCommand(VmSale vmSale)
        {
            _vmSale = vmSale;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (parameter is Panel parent) 
            {
                var userControlToRemove = parent.Children.OfType<UcSale>().FirstOrDefault();
                if (userControlToRemove != null)
                {
                    parent.Children.Remove(userControlToRemove);
                }
            }
            else if (parameter is Window parentWindow)
            {
                if (parentWindow.Content is UcSale userControlToRemove)
                {
                    parentWindow.Content = null;
                }
            }
        }
    }
}
