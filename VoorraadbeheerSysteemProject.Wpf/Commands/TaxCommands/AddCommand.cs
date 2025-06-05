using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.TaxCommands
{
   
        public class AddCommand : ICommand
        {
        
            private readonly VmTax _viewModel;

            public AddCommand(VmTax viewModel)
            {
                _viewModel = viewModel;
            }

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                _viewModel.AddTaxAsync();
            }

            public event EventHandler CanExecuteChanged;
        }

    }



