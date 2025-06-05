using System;
using System.CodeDom.Compiler;
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

        private CashShiftDTO _currentShift;
        

        #region properties
        public CashShiftDTO CurrentShift { 
            get => _currentShift;
            set { _currentShift = value; OnPropertyChanged(nameof(CurrentShift)); }
        }
        #endregion

        public VmDrawer(NavigationStore navigationStore)
        {
            _drawerRequest = new DrawerRequests(AppConfig.ApiUrl);
            CheckShift();
        }

        private async Task CheckShift()
        {
            CashShiftDTO? existingShift = await _drawerRequest.GetCashShiftTodayByEmployeeId(UserSession.IdUSer);
            if (existingShift is null)
                await RegisterCashShiftAsync();
            else
                CurrentShift = existingShift;
        }


        #region Methods
        private async Task RegisterCashShiftAsync()
        {
            var newShift = new CashShiftDTO
            {
                CashRegisterId = 3,
                ShiftDate = DateTime.Now.Date,
                ShiftStart = DateTime.Now,
                OpeningBalance = 100.00m, // Example opening balance
                DateCreated = DateTime.Now,
                EmployeeId = UserSession.IdUSer,
                SaasClientId = 1, // Example SaaS client ID
            };

            CurrentShift =  await _drawerRequest.PostCashShiftAsync(newShift);
        }
        #endregion
    }
}
