using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using VoorraadbeheerSysteemProject.Wpf.Helpers;
using VoorraadbeheerSysteemProject.Wpf.Services.Users;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

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
        string? token = JwtTokenStore.Token;

        if (!string.IsNullOrEmpty(token))
        {
            var userService = new UsersRequests(AppConfig.ApiUrl);
            bool isValid = await userService.ValidateTokenAsync(token);

            if (isValid)
            {
                _navigationStore.CurrentViewModel = new VmDashboard(_navigationStore);
            }
            else
            {
                JwtTokenStore.Token = null;
                _navigationStore.CurrentViewModel = new VmUserLogin(_navigationStore);
            }
        }
        else
        {
            _navigationStore.CurrentViewModel = new VmUserLogin(_navigationStore);
        }

        MainWindow = new MainWindow()
        {
            DataContext = new VmMainWindow(_navigationStore)
        };
        MainWindow.Show();

        base.OnStartup(e);
    }

}

