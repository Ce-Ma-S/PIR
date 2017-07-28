using System;
using System.Runtime.CompilerServices;

namespace Common.Commands
{
    public static class Commands
    {
        public static void ValidateCanExecute(bool canExecute, [CallerMemberName] string name = null)
        {
            if (!canExecute)
                throw new InvalidOperationException($"{name} cannot be executed.");
        }
    }
}
