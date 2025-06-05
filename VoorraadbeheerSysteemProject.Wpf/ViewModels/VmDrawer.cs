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
        private bool _shiftIsNotCreated = false;


        #region properties
        public CashShiftDTO CurrentShift { 
            get => _currentShift;
            set { _currentShift = value; OnPropertyChanged(nameof(CurrentShift)); }
        }
        public bool ShiftIsNotCreated
        {
            get => _shiftIsNotCreated;
            set {
                _shiftIsNotCreated = value;
                OnPropertyChanged(nameof(ShiftIsNotCreated));
            }
        }
        public ICommand RegisterShiftCommand => new ButtonCommand(async _ => await RegisterCashShiftAsync());
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
                ShiftIsNotCreated = true;
            else
                CurrentShift = existingShift;
        }


        #region Methods
        private async Task RegisterCashShiftAsync()
        {
            if (CurrentShift != null) return;

            var newShift = new CashShiftDTO
            {
                ShiftDate = DateTime.Now.Date,
                ShiftStart = DateTime.Now,
                DateCreated = DateTime.Now,
                UserId = UserSession.IdUSer,
                SaasClientId = 1, // Example SaaS client ID
            };

            CurrentShift =  await _drawerRequest.PostCashShiftAsync(newShift);
            ShiftIsNotCreated = false;
        }
        #endregion
    }
}
