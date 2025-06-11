using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoorraadbeheerSysteemProject.Wpf.Services;
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Views
{
    /// <summary>
    /// Logique d'interaction pour UcSupplier.xaml
    /// </summary>
    public partial class UcSupplier : UserControl
    {
        public UcSupplier()
        {
            InitializeComponent();

            //DataContext = new VmSupplier(new NavigationStore());

        }
    }
}
