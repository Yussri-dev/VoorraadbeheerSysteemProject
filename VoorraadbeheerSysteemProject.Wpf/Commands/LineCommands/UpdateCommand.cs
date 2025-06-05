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
    public class UpdateCommand : ICommand
    {
        private readonly VmLine _vm;

        public UpdateCommand(VmLine vm)
        {
            _vm = vm;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _vm.SelectedLine != null;
        }

        public async void Execute(object parameter)
        {
            if (_vm.SelectedLine != null)
            {
                var updatedLine = new LineDTO
                {
                    LineId = _vm.SelectedLine.LineId,
                    Name = _vm.SelectedLine.Name
                };

                await _vm.ApiLine.DeleteLineAsync(updatedLine.LineId);
                await _vm.ApiLine.PostLineAsync(updatedLine);

                _vm.RefreshLines();
            }
        }
    }

}
