using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Common.Commands
{
    public abstract class Command<T> :
        ICommand
    {
        public bool IsExecuting { get; private set; }

        public event EventHandler CanExecuteChanged;
        public virtual bool CanExecute(T parameter) => !IsExecuting;
        bool ICommand.CanExecute(object parameter) =>
            parameter is T &&
            CanExecute((T)parameter);

        public static event EventHandler<ExecutingEventArgs<T>> Executing;
        public abstract void Execute(T parameter);
        void ICommand.Execute(object parameter) => Execute((T)parameter);
        public static event EventHandler<ExecutedEventArgs<T>> Executed;

        protected virtual void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        protected virtual void OnExecuting(Guid executeId, T parameter)
        {
            IsExecuting = true;
            OnIsExecutingChanged();
            Executing?.Invoke(this, new ExecutingEventArgs<T>(executeId, parameter));
        }
        protected virtual void OnIsExecutingChanged() => OnCanExecuteChanged();
        protected virtual void OnExecuted(Guid executeId, T parameter, Exception error = null)
        {
            IsExecuting = false;
            OnIsExecutingChanged();
            Executed?.Invoke(this, new ExecutedEventArgs<T>(executeId, parameter, error));
        }
    }
}
