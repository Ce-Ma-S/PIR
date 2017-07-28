using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public static class Call
    {
        public static SynchronizationContext Context { get; private set; }
        public static SynchronizationContext ContextOrCurrent => Context ?? SynchronizationContext.Current;

        public static void SetCurrentContext() => Context = SynchronizationContext.Current;

        public static void Synchronize(this Action method) => ContextOrCurrent.Send(s => method(), null);
        public static T Synchronize<T>(this Func<T> method)
        {
            T result = default(T);
            ContextOrCurrent.Send(s => result = method(), null);
            return result;
        }

        public static Task SynchronizeAsync(this Action method)
        {
            var tcs = new TaskCompletionSource<object>();
            var methodWrapper = (SendOrPostCallback)(s =>
            {
                try
                {
                    method();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            ContextOrCurrent.Post(methodWrapper, null);
            return tcs.Task;
        }
        public static Task<T> SynchronizeAsync<T>(this Func<T> method)
        {
            T result = default(T);
            var tcs = new TaskCompletionSource<T>();
            var methodWrapper = (SendOrPostCallback)(s =>
            {
                try
                {
                    result = method();
                    tcs.SetResult(result);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            ContextOrCurrent.Post(methodWrapper, null);
            return tcs.Task;
        }
    }
}
