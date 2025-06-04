using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Commands.DrawerCommands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services.Drawer;
using VoorraadbeheerSysteemProject.Wpf.Services.SaasClients;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmDrawer : VmBase
    {
        private readonly DrawerRequests _drawerRequest;
        private readonly NavigationStore _navigationStore;
        public VmDrawer(NavigationStore navigationStore)
        {
            validateCashShiftCommand = new ValidateCashShiftCommand(this);
        }

        #region Fields & commands

        private DateTime _shiftStart;
        public DateTime ShiftStart
        {
            get => _shiftStart;
            set
            {
                _shiftStart = value;
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private ICommand validateCashShiftCommand { get; set; }

        #endregion
        public async Task<CashShiftDTO> SaveCashShiftAsync(CashShiftDTO cashShiftDto)
        {
            try
            {
                var createdCashShift = await _drawerRequest.PostCashShiftAsync(cashShiftDto);
                return createdCashShift;
            }
            catch (Exception)
            {

                return null;
            }
        }

        #region Methods
        private async Task RegisterCashShiftAsync()
        {
            StatusMessage = string.Empty;

            if (
                string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Adresse) ||
                string.IsNullOrWhiteSpace(SubscriptionType)
                )
            {
                StatusMessage = "All Fields Are required";
            }
            var responseDto = new CashShiftDTO
            {
                CashRegisterId = 3,

            };

            await _drawerRequest.PostCashShiftAsync(responseDto);
        }
        #endregion
    }
}
