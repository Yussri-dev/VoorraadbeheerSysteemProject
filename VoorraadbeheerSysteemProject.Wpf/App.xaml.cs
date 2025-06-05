using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using VoorraadbeheerSysteemProject.Wpf.Helpers;
using VoorraadbeheerSysteemProject.Wpf.Services.Users;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;
using VoorraadbeheerSysteemProject.Wpf.Views;

namespace VoorraadbeheerSysteemProject.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    NavigationStore _navigationStore = new NavigationStore();

    //protected override void OnStartup(StartupEventArgs e)
    //{
    //    _navigationStore.CurrentViewModel = new VmDashboard(_navigationStore);

    //    MainWindow = new MainWindow()
    //    {
    //        DataContext = new VmMainWindow(_navigationStore)
    //    };
    //    MainWindow.Show();
    //    base.OnStartup(e);
    //}

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        string? token = JwtTokenStore.Token;
        bool showMainWindow = false;

        if (!string.IsNullOrEmpty(token))
        {
            var userService = new UsersRequests(AppConfig.ApiUrl);
            bool isValid = await userService.ValidateTokenAsync(token);

            if (isValid)
            {
                showMainWindow = true;
            }
            else
            {
                JwtTokenStore.Token = null;
            }
        }

        if (showMainWindow)
        {
            ShowMainWindow();
        }
        else
        {
            var loginViewModel = new VmUserLogin(_navigationStore);
            var loginWindow = new LoginWindow()
            {
                DataContext = loginViewModel
            };

            // Set this to keep app alive while login is shown
            MainWindow = loginWindow;

            loginViewModel.LoginSucceeded += () =>
            {
                ShowMainWindow();
                loginWindow.Close();
            };

            loginWindow.Show();
        }

    }

    private void ShowMainWindow()
    {
        _navigationStore.CurrentViewModel = new VmDashboard(_navigationStore);

        MainWindow = new MainWindow()
        {
            DataContext = new VmMainWindow(_navigationStore)
        };
        MainWindow.Show();
    }



}

