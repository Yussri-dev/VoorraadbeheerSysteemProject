using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VoorraadbeheerSysteemProject.Wpf.Commands;
using VoorraadbeheerSysteemProject.Wpf.Services.Users;
using VoorraadbeheerSysteemProject.Wpf.Stores;

namespace VoorraadbeheerSysteemProject.Wpf.ViewModels
{

    public class VmUserLogin : VmBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly UsersRequests _userService;

        private string _userName;
        private string _password;
        private string _statusMessage;

        public VmUserLogin(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _userService = new UsersRequests(AppConfig.ApiUrl);
            LoginCommand = new ButtonCommand(Login);
        }

        public string UserName
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        private async void Login(object parameter)
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                StatusMessage = "Enter both username and password.";
                return;
            }

            bool result = await _userService.LoginAsync(UserName, Password);
            if (result)
                _navigationStore.CurrentViewModel = new VmDashboard(_navigationStore);
            else
                StatusMessage = "Invalid login credentials.";
        }
    }

}
