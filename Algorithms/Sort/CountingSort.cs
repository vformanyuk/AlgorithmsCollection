using System;

namespace Algorithms.Sort
{
    public class CountingSort : IAlgorithm
    {
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            Console.WriteLine(string.Join(",", data));

            int @base = 10, m=10, n=1;
            int totalIdx = 0;
            var idxs = new int[@base];

            do
            {
                totalIdx = 0;
                foreach (var item in data)
                {
                    var idx = item / n % m;
                    idxs[idx]++;
                    totalIdx += idx;
                }

                if (totalIdx == 0) break;

                for (int i = 1; i < @base; i++)
                {
                    idxs[i] = idxs[i] + idxs[i - 1];
                }

                var result = new int[data.Length];
                for (int i = data.Length - 1; i >= 0; i--)
                {
                    var idx = data[i] / n % m;
                    var count = --idxs[idx];
                    result[count] = data[i];
                }

                result.CopyTo(data, 0);
                Array.Clear(idxs, 0, idxs.Length);

                m *= 10;
                n *= 10;
            } while (totalIdx != 0);

            Console.WriteLine(string.Join(",", data));
        }
    }
}
