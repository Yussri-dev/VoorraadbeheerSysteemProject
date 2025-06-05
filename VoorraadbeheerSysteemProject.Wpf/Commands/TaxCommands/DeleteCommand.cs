using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.TaxCommands
{
   
        public class DeleteCommand : ICommand
        {
        private readonly VmTax _vm;

        public DeleteCommand(VmTax vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));
            _vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(VmTax.SelectedTax))
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public bool CanExecute(object parameter)
        {

            return _vm.SelectedTax != null;
        }

        public async void Execute(object parameter)
        {
            var tax = _vm.SelectedTax;
            if (tax == null)
                return;


            var confirm = MessageBox.Show(
                $"Weet je zeker dat je '{tax.TaxRate}' wilt verwijderen?",
                "Bevestiging",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                bool success = await _vm.ApiTax.DeleteTaxAsync(tax.TaxId);

                if (success)
                {

                    _vm.Taxes.Remove(tax);
                    _vm.FilteredTaxes.Remove(tax);
                    _vm.SelectedTax = null;
                    _vm.TotalTaxes = _vm.FilteredTaxes.Count;

                    MessageBox.Show($"tax'{tax.TaxRate}' is verwijderd.", "Succes",
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


