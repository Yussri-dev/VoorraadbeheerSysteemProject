using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.ReportsCommands
{
        public class ResetCommand : ICommand
        {
            private readonly VmReport _viewModel;

            public ResetCommand(VmReport viewModel)
            {
                _viewModel = viewModel;
            }

            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter) => true;

            public void Execute(object? parameter)
            {
                _viewModel.SearchText = string.Empty;

            }
        }

    }
