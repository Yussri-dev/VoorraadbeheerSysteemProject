using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
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

        private DateTime _shiftStart;
        private string _statusMessage;
        private CashShiftDTO _currentShift;
        

        #region properties

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public CashShiftDTO CurrentShift { 
            get => _currentShift;
            set { _currentShift = value; OnPropertyChanged(nameof(CurrentShift)); }
        }


        #endregion

        public VmDrawer(NavigationStore navigationStore)
        {
            _drawerRequest = new DrawerRequests(AppConfig.ApiUrl);
            RegisterCashShiftAsync();
        }


        #region Methods
        private async Task RegisterCashShiftAsync()
        {
            StatusMessage = string.Empty;

            var newShift = new CashShiftDTO
            {
                CashRegisterId = 3,
                ShiftDate = DateTime.Now.Date,
                ShiftStart = DateTime.Now,
                OpeningBalance = 100.00m, // Example opening balance
                DateCreated = DateTime.Now,
                EmployeeId = 2, // Example employee ID
                SaasClientId = 1, // Example SaaS client ID
            };

            CurrentShift =  await _drawerRequest.PostCashShiftAsync(newShift);
        }
        #endregion
    }
}
