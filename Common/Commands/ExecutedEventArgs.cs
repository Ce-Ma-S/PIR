using System;

namespace Common.Commands
{
    public class ExecutedEventArgs<T> :
        ExecuteEventArgs<T>
    {
        public ExecutedEventArgs(Guid executeId, T parameter, Exception error) :
            base(executeId, parameter)
        {
            Error = error;
        }

        public Exception Error { get; private set; }
        public void HandleError() => Error = null;
    }
}
