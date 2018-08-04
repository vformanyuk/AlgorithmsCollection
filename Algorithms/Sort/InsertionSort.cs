using System;

namespace Algorithms.Sort
{
    public class InsertionSort : IAlgorithm
    {
        public void Run()
        {
            int[] data = new int[12] {7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2};

            Console.WriteLine(string.Join(",", data));
            for (int i = 1; i < data.Length; i++)
            {
                var swpIdx = i;
                while (swpIdx > 0 && data[swpIdx] < data[swpIdx - 1])
                {
                    var buf = data[swpIdx - 1];
                    data[swpIdx - 1] = data[swpIdx];
                    data[swpIdx] = buf;
                    swpIdx--;
                }
            }
            Console.WriteLine(string.Join(",", data));
        }
    }
}
