using Common.Validation;
using System;

namespace Common.Commands
{
    public class DelegateCommand<T> :
        Command<T>
    {
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            ArgumentValidation.NonNull(execute, nameof(execute));
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public override bool CanExecute(T parameter) =>
            base.CanExecute(parameter) &&
            canExecute?.Invoke(parameter) != false;

        public override void Execute(T parameter)
        {
            var id = Guid.NewGuid();
            try
            {
                OnExecuting(id, parameter);
                execute(parameter);
                OnExecuted(id, parameter);
            }
            catch (Exception e)
            {
                OnExecuted(id, parameter, e);
            }
        }

        private Action<T> execute;
        private Func<T, bool> canExecute;
    }


    public class DelegateCommand :
        DelegateCommand<object>
    {
        public DelegateCommand(Action execute, Func<bool> canExecute = null) :
            base(p => execute(), p => canExecute?.Invoke() != false)
        {
            ArgumentValidation.NonNull(execute, nameof(execute));
        }
    }
}
