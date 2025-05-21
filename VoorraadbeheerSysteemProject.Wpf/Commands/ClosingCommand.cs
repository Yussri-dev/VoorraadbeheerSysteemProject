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

        public event EventHandler? CanExecuteChanged;

        public ClosingCommand()
        {
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (parameter is Window window)
            {
                window.Close(); // ✅ This cleanly closes the window
            }
        }
    }
}
