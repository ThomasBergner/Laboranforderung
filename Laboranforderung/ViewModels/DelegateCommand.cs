using System;
using System.Windows.Input;

namespace Laboranforderung.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private readonly Action m_Execute;
        private readonly Func<bool>? m_CanExecute;

        public DelegateCommand(Action execute, Func<bool>? canExecute = null)
        {
            m_Execute = execute;
            m_CanExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return m_CanExecute?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            m_Execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
