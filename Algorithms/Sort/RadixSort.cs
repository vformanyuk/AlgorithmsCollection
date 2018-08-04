using System;
using System.Collections.Generic;

namespace Algorithms.Sort
{
    public class RadixSort : IAlgorithm
    {
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            int div = 1, mod = 10, @base = 10;
            Dictionary<int, List<int>> buckets = new Dictionary<int, List<int>>();
            for (int i = 0; i < @base; i++)
            {
                buckets.Add(i, new List<int>());
            }

            Console.WriteLine(string.Join(",", data));
            int counter = 0;
            do
            {
                counter = 0;
                foreach (var item in data)
                {
                    var a = item % mod;
                    var reminder = a / div;
                    counter += reminder;
                    buckets[reminder].Add(item);
                }

                int dataIdx = 0;
                foreach (var bucket in buckets)
                {
                    if (bucket.Value.Count == 0) continue;

                    foreach (var i in bucket.Value)
                    {
                        data[dataIdx] = i;
                        dataIdx++;
                    }
                    bucket.Value.Clear();
                }

                div *= @base;
                mod *= @base;

            } while (counter != 0);
            Console.WriteLine(string.Join(",", data));
        }
    }
}
