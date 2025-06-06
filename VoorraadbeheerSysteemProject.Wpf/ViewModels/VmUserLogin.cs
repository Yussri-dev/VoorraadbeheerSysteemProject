using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public class VmUserLogin : VmBase
    {
        public event Action? LoginSucceeded;

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
            NavigateToUserCreateCommand = new ButtonCommand(Navigate);

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
        public ICommand NavigateToUserCreateCommand { get; }

        private async void Login(object parameter)
        {
            StatusMessage = string.Empty;

            UserName = "ghanmiyoussri@gmail.com";
            Password = "077671699Yos@";
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                StatusMessage = "Enter both username and password.";
                return;
            }

            var result = await _userService.LoginAsync(UserName, Password);

            if (result != null)
            {
                // Save session/token
                JwtTokenStore.Token = result.Token;
                UserSession.Token = result.Token;
                UserSession.Email = result.Email;
                UserSession.IdUSer = result.IdUser;
                //UserSession.IdSaasClient = result.id
                LoginSucceeded?.Invoke();
            }
            else
            {
                StatusMessage = "Invalid Credentials.";
            }
        }

        private void Navigate(object parameter)
        {
            _navigationStore.CurrentViewModel = new VmUserCreate(_navigationStore);

        }

    }

}
