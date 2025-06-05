using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.LineCommands
{
    public class AddCommand : ICommand
    {
        private readonly VmLine _viewModel;

        

        public AddCommand(VmLine viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _viewModel.AddLineAsync();
        }

        public event EventHandler CanExecuteChanged;
    }

}
