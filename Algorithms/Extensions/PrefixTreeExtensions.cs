using Algorithms.DataStructures;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithms.Extensions
{
    public static class PrefixTreeExtensions
    {
        /// <summary>
        /// tries to find following patterns in prefix tree
        /// pattern
        /// *pattern
        /// **pattern
        /// ...
        /// *...*pattern
        /// Number of wildcards <= longest word length - pattern length
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="pattern"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public static IEnumerable<string> MatchContains(this PrefixTree tree, string pattern, bool caseSensitive) 
        {
            var promptsCount = tree.LengthHistogram.Keys.Max() - pattern.Length;
            if (promptsCount <= 0)
            {
                return Enumerable.Empty<string>();
            }

            var prompts = new string[promptsCount];
            for (int i = 0; i < promptsCount; i++)
            {
                string wildcardPrefix = string.Empty;
                if (i > 0)
                {
                    wildcardPrefix = new string(tree.Wildcard, i);
                }
                prompts[i] = $"{wildcardPrefix}{pattern}";
            }

            var searchResult = new ConcurrentBag<IEnumerable<string>>();
            Parallel.ForEach(prompts, p => searchResult.Add(tree.Match(p, caseSensitive)));
            return searchResult.SelectMany(x => x);
        }
    }
}
