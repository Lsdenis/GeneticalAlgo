using System;
using System.Windows.Input;

namespace GeneticalAlgorithms.Custom
{
    internal class Command : ICommand
    {
        private readonly Action _action;
        private readonly bool _canExecute;

        public Command(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}