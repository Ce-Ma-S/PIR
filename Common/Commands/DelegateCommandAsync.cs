using Common.Validation;
using System;
using System.Threading.Tasks;

namespace Common.Commands
{
    public class DelegateCommandAsync<T> :
        Command<T>
    {
        public DelegateCommandAsync(Func<T, Task> execute, Func<T, bool> canExecute = null)
        {
            ArgumentValidation.NonNull(execute, nameof(execute));
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public override bool CanExecute(T parameter) =>
            base.CanExecute(parameter) &&
            canExecute?.Invoke(parameter) != false;

        public async Task ExecuteAsync(T parameter)
        {
            var id = Guid.NewGuid();
            try
            {
                OnExecuting(id, parameter);
                await execute(parameter);
                OnExecuted(id, parameter);
            }
            catch (Exception e)
            {
                OnExecuted(id, parameter, e);
            }
        }
        public override async void Execute(T parameter) => await ExecuteAsync(parameter);

        private Func<T, Task> execute;
        private Func<T, bool> canExecute;
    }


    public class DelegateCommandAsync :
        DelegateCommandAsync<object>
    {
        public DelegateCommandAsync(Func<Task> execute, Func<bool> canExecute = null) :
            base(p => execute(), p => canExecute?.Invoke() != false)
        {
            ArgumentValidation.NonNull(execute, nameof(execute));
        }
    }
}
