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

namespace VoorraadbeheerSysteemProject.Wpf.Views
{
    /// <summary>
    /// Logique d'interaction pour UcNumericPad.xaml
    /// </summary>
    public partial class UcNumericPad : UserControl
    {
        public UcNumericPad()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty AddCommandProperty =
        DependencyProperty.Register(
            nameof(AddCommand),
            typeof(ICommand),
            typeof(UcNumericPad),
            new PropertyMetadata(null));

        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }

        public ICommand Number1Command
        {
            get { return (ICommand)GetValue(Number1CommandProperty); }
            set { SetValue(Number1CommandProperty, value); }
        }

        public static readonly DependencyProperty Number1CommandProperty =
            DependencyProperty.Register(nameof(Number1Command), typeof(ICommand), typeof(UcNumericPad));

    }
}
