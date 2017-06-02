using System;
using System.Runtime.CompilerServices;

namespace Common.Validation
{
    public static class ArgumentValidation
    {
        public static T NonNull<T>(T value, [CallerMemberName] string name = null)
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException(name);
            return value;
        }
    }
}
