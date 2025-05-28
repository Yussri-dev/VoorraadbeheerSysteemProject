using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VoorraadbeheerSysteemProject.Wpf.Behaviors
{
    public class SelectAllTextOnFocusBehavior
    {
       public static readonly DependencyProperty EnablePropperty = 
            DependencyProperty.RegisterAttached(
                "Enable", 
                typeof(bool), 
                typeof(SelectAllTextOnFocusBehavior), 
                new PropertyMetadata(false, OnEnableChanged));
        public static bool GetEnable(DependencyObject obj) => (bool)obj.GetValue(EnablePropperty);
        public static void SetEnable(DependencyObject obj, bool value) => obj.SetValue(EnablePropperty, value);

        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                    textBox.GotKeyboardFocus += TextBox_GotKeyboardFocus;
                else
                    textBox.GotKeyboardFocus -= TextBox_GotKeyboardFocus;
            }
        }

        private static void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(sender is TextBox textBox)
                textBox.SelectAll();
        }
    }
}
