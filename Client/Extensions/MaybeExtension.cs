using System;

namespace Client.Extensions
{
    public static class MaybeExtension
    {
        public static TResult MayBe<T, TResult>(this T obj, Func<T, TResult> accessor) where T : class
        {
            return obj == null ? default(TResult) : accessor(obj);
        }
    }
}