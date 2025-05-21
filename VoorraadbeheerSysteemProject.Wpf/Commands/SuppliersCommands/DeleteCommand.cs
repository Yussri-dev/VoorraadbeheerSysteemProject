using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.SuppliersCommands
{
    public class DeleteCommand : ICommand
    {
        private readonly VmSupplier _vm;

        public DeleteCommand(VmSupplier vm)
        {
            _vm = vm;
            _vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(VmSupplier.SelectedSupplier))
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public bool CanExecute(object parameter)
        {
            return _vm.SelectedSupplier != null;
        }

        public async void Execute(object parameter)
        {
            var supplier = _vm.SelectedSupplier;
            if (supplier == null) return;

            var confirm = MessageBox.Show(
                $"Weet je zeker dat je '{supplier.Name}' wilt verwijderen?",
                "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            var success = await _vm.ApiSupplier.DeleteSuppliersAsync(supplier.SupplierId);

            if (success)
            {
                _vm.Suppliers.Remove(supplier);
                _vm.FilteredSuppliers.Remove(supplier);
                _vm.SelectedSupplier = null;
                _vm.TotalSuppliers = _vm.FilteredSuppliers.Count;
            }
            else
            {
                MessageBox.Show("Verwijderen mislukt.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}