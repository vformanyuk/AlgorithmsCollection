using System;

namespace Algorithms.Sort
{
    public class CombSort : IAlgorithm
    {
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            var distance = data.Length - 1;

            Console.WriteLine(string.Join(",", data));
            do
            {
                var swapCount = 0;
                for (int startIdx = 0; startIdx + distance < data.Length; startIdx++)
                {
                    if (data[startIdx] > data[startIdx + distance])
                    {
                        var tmp = data[startIdx + distance];
                        data[startIdx + distance] = data[startIdx];
                        data[startIdx] = tmp;
                        swapCount++;
                    }
                }

                distance--;
                if (distance < 1 && swapCount != 0)
                {
                    distance++;
                }

            } while (distance >= 1);

            Console.WriteLine(string.Join(",", data));
        }
    }
}
