using System.Collections.Generic;

namespace Algorithms.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<T> AsEnumerable<T>(this T self)
        {
            yield return self;
        }
    }
}
