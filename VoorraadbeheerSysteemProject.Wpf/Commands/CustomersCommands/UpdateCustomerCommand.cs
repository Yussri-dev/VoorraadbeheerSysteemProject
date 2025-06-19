using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands.CustomersCommands
{
    class UpdateCustomerCommand : ICommand
    {
        private readonly VmCustomer _vmCustomer;

        public UpdateCustomerCommand(VmCustomer vmCustomer)
        {
            _vmCustomer = vmCustomer;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _vmCustomer.SelectedCustomer != null;
        }

        public async void Execute(object? parameter)
        {
            if (_vmCustomer.SelectedCustomer != null)
            {
                var updatedCustomer = new CustomerDTO
                {
                    CustomerId = _vmCustomer.SelectedCustomer.CustomerId,
                    Name = _vmCustomer.SelectedCustomer.Name,
                    PhoneNumber1 = _vmCustomer.SelectedCustomer.PhoneNumber1,
                    PhoneNumber2 = _vmCustomer.SelectedCustomer.PhoneNumber2,
                    Email = _vmCustomer.SelectedCustomer.Email,
                    City = _vmCustomer.SelectedCustomer.City,
                    Adresse = _vmCustomer.SelectedCustomer.Adresse,
                    Land = _vmCustomer.SelectedCustomer.Land
                };

                await _vmCustomer.ApiCustomer.UpdateCustomerAsync(updatedCustomer);
                _vmCustomer.RefreshCustomers();
            }
        }
    }
}
