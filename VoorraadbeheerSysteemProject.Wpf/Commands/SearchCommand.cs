using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Commands
{
    class SearchCommand : CommandBase
    {
        private readonly Action<object> _action;
        private readonly Func<object?, bool>? _canExecute;

        public SearchCommand(Action<object> action, Func<object?, bool>? canExecute = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _canExecute = canExecute;
        }

        public override void Execute(object? parameter)
        {
            _action(parameter ?? "");
        }

        public override bool CanExecute(object? parameter) =>
            _canExecute?.Invoke(parameter) ?? true;

    }
}
