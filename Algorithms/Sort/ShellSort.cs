using System;

namespace Algorithms.Sort
{
    public class ShellSort : IAlgorithm
    {
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            Console.WriteLine(string.Join(",", data));
            int step = data.Length - 1;
            while (step > 0)
            {
                for (int i = step; i < data.Length; i ++)
                {
                    int swpIdx = i;
                    while (data[swpIdx] < data[swpIdx - step])
                    {
                        var buf = data[swpIdx - step];
                        data[swpIdx - step] = data[swpIdx];
                        data[swpIdx] = buf;

                        swpIdx -= step;

                        if(swpIdx - step < 0) break;
                    }
                }

                step >>= 1;
            }

            Console.WriteLine(string.Join(",", data));
        }
    }
}
