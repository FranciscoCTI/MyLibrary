using System.Windows.Input;

namespace Library.UI.Commands
{
    /// <summary>
    /// Relay command to be used in all Command calls from the UI
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;

        private Func<object, bool> _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Constructor for the <see cref="RelayCommand"/> class
        /// </summary>
        /// <param name="action">The method to be executed</param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            _execute = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
    
}
