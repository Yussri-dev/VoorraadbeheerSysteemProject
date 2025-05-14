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
    public class ButtonCommand : CommandBase
    {
        private Action<object> _action;

        public ButtonCommand(Action<object> action)
        {
            _action = action;
        }

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
