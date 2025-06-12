using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services.CashRegister;
using VoorraadbeheerSysteemProject.Wpf.Services.Drawer;
using VoorraadbeheerSysteemProject.Wpf.Services.SaasClients;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmDrawer : VmBase
    {
        private readonly DrawerRequests _drawerRequest;
        private readonly CashRegisterRequest _cashRegisterRequest;
        private bool _shiftIsNotCreated = false;
        private string _title = "Created new Shift";
        private bool _isReadOnly = false; 
        private CashRegisterDTO? _cashRegister;

        private CashShiftDTO _currentShift = new CashShiftDTO() 
        { 
            ShiftStart = DateTime.Now,
            DateCreated = DateTime.Now,
            UserId = UserSession.IdUSer,
            SaasClientId = 1
        };


        #region properties

        public CashRegisterDTO? CashRegister
        {
            get => _cashRegister;
            set { 
                _cashRegister = value;
                OnPropertyChanged(nameof(CashRegister));
            }
        }

        public CashShiftDTO CurrentShift
        {
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

        public string Title
        {
            get => _title;
            set { 
                _title = value; 
                OnPropertyChanged(nameof(Title));
            }
        }


        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set { 
                _isReadOnly = value; 
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }

        public ICommand NavigateDashboardCommand { get; }
        public ICommand RegisterShiftCommand => new ButtonCommand(async _ => await RegisterCashShiftAsync());
        #endregion

        public VmDrawer(NavigationStore navigationStore)
        {
            NavigateDashboardCommand = new NavigationCommand<VmDashboard>(navigationStore,
                () => new VmDashboard(navigationStore));

            _drawerRequest = new DrawerRequests(AppConfig.ApiUrl);
            _cashRegisterRequest = new CashRegisterRequest(AppConfig.ApiUrl);

            CheckShift();
            GetCashRegister();

        }


        private async Task CheckShift()
        {
            CashShiftDTO? existingShift = await _drawerRequest.GetCashShiftTodayByEmployeeId(UserSession.IdUSer);
            if (existingShift is null)
            {
                ShiftIsNotCreated = true;
                Title = "Create new Shift";
                IsReadOnly = false;
            }
            else
            {
                CurrentShift = existingShift;
                Title = "Shift information";
                IsReadOnly = true;
            }
        }

        private async Task GetCashRegister()
        {
            CashRegister = await _cashRegisterRequest.GetShiftByUserIdAsync(UserSession.IdUSer);
        }


        #region Methods
        private async Task RegisterCashShiftAsync()
        {
            if (CurrentShift.CashShiftId != 0) return;

            var newShift = new CashShiftDTO
            {
                ShiftDate = DateTime.Now.Date,
                ShiftStart = DateTime.Now,
                DateCreated = DateTime.Now,
                UserId = UserSession.IdUSer,
                SaasClientId = 1, // Example SaaS client ID
            };
            //check if filled in correctly
            if(CurrentShift.OpeningBalance <= 0)
            {
                MessageBox.Show("Opening balance must be greater than zero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //if (CurrentShift.CashRegisterId <= 0)
            //{
            //    MessageBox.Show("Cash register ID must be greater than zero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            //fill in the properties
            newShift.CashRegisterId = (await _cashRegisterRequest.GetShiftByUserIdAsync(UserSession.IdUSer)).CashRegisterId;
            newShift.OpeningBalance = CurrentShift.OpeningBalance;

            var createdShift =  await _drawerRequest.PostCashShiftAsync(newShift);

            //check if the shift was created successfully
            if (createdShift is null)
                MessageBox.Show("Failed to create shift. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                ShiftIsNotCreated = false;
                IsReadOnly = true;
                MessageBox.Show("Shift created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }    
        }
        #endregion
    }
}
