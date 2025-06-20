﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.CategoriesCommands
{

    public class DeleteCategoryCommand : ICommand
    {
        private readonly VmCategory _vm;

        public DeleteCategoryCommand(VmCategory vm)
        {
            _vm = vm ?? throw new ArgumentNullException(nameof(vm));
            _vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(VmCategory.SelectedCategory))
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            };
        }

        public bool CanExecute(object parameter)
        {
            
            return _vm.SelectedCategory != null;
        }

        public async void Execute(object parameter)
        {
            var category = _vm.SelectedCategory;
            if (category == null)
                return;

            
            var confirm = MessageBox.Show(
                $"Weet je zeker dat je '{category.Name}' wilt verwijderen?",
                "Bevestiging",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                bool success = await _vm.ApiCategory.DeleteCategoryAsync(category.CategoryId);

                if (success)
                {
                   
                    _vm.Categories.Remove(category);
                    _vm.FilteredCategories.Remove(category);
                    _vm.SelectedCategory = null;
                    _vm.TotalCategories = _vm.FilteredCategories.Count;

                    MessageBox.Show($"Categorie '{category.Name}' is verwijderd.", "Succes",
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