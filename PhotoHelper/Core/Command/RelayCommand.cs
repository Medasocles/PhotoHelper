using PhotoHelper.Core.Exceptions;
using System;
using System.Windows.Input;

namespace PhotoHelper.Core.Command
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            if (parameter == null)
                return _canExecute.Invoke((T)new object());

            if (parameter is T)
                return _canExecute.Invoke((T)parameter);

            throw new IncompatibleTypesException($"Parameter of type {typeof(T)} expected");
        }

        public void Execute(object parameter)
        {
            if (parameter is T)
            {
                _execute.Invoke((T)parameter);
            }
            else if (parameter == null)
            {
                _execute.Invoke((T)new object());
            }
            else
                throw new IncompatibleTypesException($"Parameter of type {typeof(T)} expected");
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
