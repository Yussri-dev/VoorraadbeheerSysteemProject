using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Helpers;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.Services.Users;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{
    class VmUserCreate : VmBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly UsersRequests _userService;

        public VmUserCreate(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _userService = new UsersRequests(AppConfig.ApiUrl);
            RegisterCommands = new ButtonCommand(async async => await RegisterAsync());
            NavigateToUserLoginCommand = new ButtonCommand(Navigate);
        }

        #region Properties
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _statusMessage;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private string _selectedRole;
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }
        public List<string> Roles { get; } = new List<string>
        {
            "Admin",
            "User"
        };

        public List<string> AvailableRoles { get; } = new List<string> { "User", "Admin" };
       

        #endregion

        #region Commands
        public ICommand RegisterCommands { get; set; }
        public ICommand NavigateToUserLoginCommand { get; }
        #endregion

        #region Methods
        private async Task RegisterAsync()
        {
            StatusMessage = string.Empty;

            if (
                string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                StatusMessage = "All fields are Required";
                return; 
            }

            if (Password != ConfirmPassword)
            {
                StatusMessage = "Passwords do not match.";
                return;
            }

            var responseDto = new RegisterDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                Roles = new List<string> { SelectedRole } 
            };

            var response = await _userService.RegisterAsync(responseDto);

            if (response.IsSuccess)
            {
                StatusMessage = "Registration Successful";
                JwtTokenStore.Token = response.Token;
                _navigationStore.CurrentViewModel = new VmUserLogin(_navigationStore);
            }
            else
            {
                StatusMessage = $"Registration Failed";
            }
        }


        private void Navigate(object parameter)
        {
            _navigationStore.CurrentViewModel = new VmUserLogin(_navigationStore);
        }
        #endregion
    }
}
