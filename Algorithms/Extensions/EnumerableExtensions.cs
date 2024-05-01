using System.Collections.Generic;

namespace Algorithms.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<T> Chain<T>(this IEnumerable<T> self, params IEnumerable<T>[] others)
        {
            foreach(T item in self)
            {
                yield return item;
            }

            foreach(var collection in others)
            {
                foreach (T item in collection)
                {
                    yield return item;
                }
            }

            yield break;
        }

        internal static IEnumerable<T> AsEnumerable<T>(this T self)
        {
            yield return self;
        }
    }
}
