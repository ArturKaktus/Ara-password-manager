using System.Windows.Input;

namespace APM.Core
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _executeAsync;

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            _executeAsync = null;
        }

        public DelegateCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
            _execute = null;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
            else if (_executeAsync != null)
            {
                _executeAsync(parameter).GetAwaiter();
            }
        }

        public async Task ExecuteAsync(object parameter)
        {
            if (_executeAsync != null)
            {
                await _executeAsync(parameter);
            }
            else if (_execute != null)
            {
                _execute(parameter);
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
