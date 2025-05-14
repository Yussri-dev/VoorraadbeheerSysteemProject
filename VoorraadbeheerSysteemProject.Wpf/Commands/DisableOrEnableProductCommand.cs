using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;
using VoorraadbeheerSysteemProject.Wpf.ViewModels;

namespace VoorraadbeheerSysteemProject.Wpf.Commands
{
    public class DisableOrEnableProductCommand : CommandBase
    {
        private Action<object> _action;

        public DisableOrEnableProductCommand(Action<object> action)
        {
            _action = action;
        }

        //public void UpdateSelectedProduct()
        //{
        //    RaiseCanExecuteChanged();
        //}

        //public override bool CanExecute(object? parameter)
        //{
        //    if(parameter == null)
        //        return false;

        //    return true;
        //}

        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                _action(parameter);
            }
            else
            {
                _action("");
            }
        }
    }
}
