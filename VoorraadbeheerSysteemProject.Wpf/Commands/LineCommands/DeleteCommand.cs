using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.LineCommands
{
    public class DeleteCommand : ICommand
    {
        private readonly VmLine _vm;

        public DeleteCommand(VmLine vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));
            _vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(VmLine.SelectedLine))
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public bool CanExecute(object parameter)
        {

            return _vm.SelectedLine != null;
        }

        public async void Execute(object parameter)
        {
            var line = _vm.SelectedLine;
            if (line == null)
                return;


            var confirm = MessageBox.Show(
                $"Weet je zeker dat je '{line.Name}' wilt verwijderen?",
                "Bevestiging",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                bool success = await _vm.ApiLine.DeleteLineAsync(line.LineId);

                if (success)
                {

                    _vm.Lines.Remove(line);
                    _vm.FilteredLines.Remove(line);
                    _vm.SelectedLine = null;
                    _vm.TotalLines = _vm.FilteredLines.Count;

                    MessageBox.Show($"line '{line.Name}' is verwijderd.", "Succes",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Verwijderen mislukt. Probeer het opnieuw.", "Fout",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Er is een fout opgetreden: {ex.Message}", "Fout",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}

