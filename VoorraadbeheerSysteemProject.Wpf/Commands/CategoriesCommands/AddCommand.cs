using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.CategoriesCommands
{
    public class AddCommand : ICommand
    {
        private readonly VmCategory _viewModel;

        public AddCommand(VmCategory viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _viewModel.AddCategoryAsync();
        }

        public event EventHandler CanExecuteChanged;
    }

}
