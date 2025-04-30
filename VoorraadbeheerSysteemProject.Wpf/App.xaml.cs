using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    NavigationStore _navigationStore = new NavigationStore();

    protected override void OnStartup(StartupEventArgs e)
    {
        _navigationStore.CurrentViewModel = new VmProducts(_navigationStore);

        MainWindow = new MainWindow()
        {
            DataContext = new VmMainWindow(_navigationStore)
        };
        MainWindow.Show();
        base.OnStartup(e);
    }
}

