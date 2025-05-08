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
using VoorraadbeheerSysteemProject.Wpf.Stores;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Views
{
    /// <summary>
    /// Logique d'interaction pour UcCategory.xaml
    /// </summary>
    public partial class UcCategory : UserControl
    {
        public UcCategory()
        {
            InitializeComponent();
  this.DataContext = new VmCategory(new NavigationStore());
        }
    }
}
