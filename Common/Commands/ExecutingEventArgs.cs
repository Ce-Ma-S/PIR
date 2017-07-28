using System;

namespace Common.Commands
{
    public class ExecutingEventArgs<T> :
        ExecuteEventArgs<T>
    {
        public ExecutingEventArgs(Guid executeId, T parameter) :
            base(executeId, parameter)
        { }
    }
}
