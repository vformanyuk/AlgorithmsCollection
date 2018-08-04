using System;
using System.Collections.Generic;

namespace Algorithms.Sort
{
    public class MergeSort : IAlgorithm
    {
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            Console.WriteLine(string.Join(",", data));
            int arrLength = 1;
            while (arrLength < data.Length)
            {
                int arraysCount = data.Length / arrLength;
                for (int idx = 0; idx < arraysCount; idx +=2)
                {
                    Sort(arrLength, idx * arrLength, (idx + 1) * arrLength);
                }

                arrLength <<= 1;
            }
            Console.WriteLine(string.Join(",", data));
        }

        private void Sort(int arrLength, int idx1, int idx2)
        {
            if (idx2 >= data.Length) return;

            int i = 0;

            var temp1 = new List<int>(arrLength);
            var temp2 = new List<int>(arrLength);

            for (i = 0; i < arrLength && idx1 + i < data.Length; i++)
            {
                temp1.Add(data[idx1 + i]); //memcpy
            }

            for (i = 0; i < arrLength && idx2 + i < data.Length; i++)
            {
                temp2.Add(data[idx2 + i]); //memcpy
            }

            int total = temp1.Count + temp2.Count, j = 0;
            i = 0;
            for (int k = 0; k < total; k++)
            {
                if (i >= temp1.Count) // first array complitely inspected, use second only
                {
                    data[idx1 + k] = temp2[j];
                    j++;
                    continue;
                }

                if (j >= temp2.Count) // second array complitely inspected, use first only
                {
                    data[idx1 + k] = temp1[i];
                    i++;
                    continue;
                }

                if (temp1[i] <= temp2[j])
                {
                    data[idx1 + k] = temp1[i];
                    i++;
                }
                else
                {
                    data[idx1 + k] = temp2[j];
                    j++;
                }
            }
        }
    }
}
