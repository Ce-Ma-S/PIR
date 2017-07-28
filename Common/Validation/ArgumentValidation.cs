using System;
using System.Collections.Generic;
using System.Linq;
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

        public static T EqualTo<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IEquatable<T>
        {
            if (!value.Equals(referenceValue))
                throw new ArgumentOutOfRangeException(name, value, $"Value must be {referenceValue}");
            return value;
        }
        public static T GreaterThan<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) <= 0)
                throw new ArgumentOutOfRangeException(name, value, $"Value must be > {referenceValue}");
            return value;
        }
        public static T GreaterThanOrEqualTo<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) < 0)
                throw new ArgumentOutOfRangeException(name, value, $"Value must be >= {referenceValue}");
            return value;
        }
        public static T LessThan<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) >= 0)
                throw new ArgumentOutOfRangeException(name, value, $"Value must be < {referenceValue}");
            return value;
        }
        public static T LessThanOrEqualTo<T>(T value, T referenceValue, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(referenceValue) > 0)
                throw new ArgumentOutOfRangeException(name, value, $"Value must be <= {referenceValue}");
            return value;
        }
        public static T In<T>(T value, T from, T to, [CallerMemberName] string name = null)
            where T : IComparable<T>
        {
            GreaterThanOrEqualTo(value, from);
            LessThanOrEqualTo(value, to);
            return value;
        }
        public static T In<T>(T value, IEnumerable<T> values, [CallerMemberName] string name = null)
        {
            if (!values.Contains(value))
                throw new ArgumentOutOfRangeException(name, value, $"Value is unknown.");
            return value;
        }
    }
}
