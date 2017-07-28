using System;

namespace Common.Commands
{
    public abstract class ExecuteEventArgs<T> :
        EventArgs
    {
        public ExecuteEventArgs(Guid executeId, T parameter)
        {
            ExecuteId = executeId;
            Parameter = parameter;
        }

        public Guid ExecuteId { get; }
        public T Parameter { get; }
    }
}
