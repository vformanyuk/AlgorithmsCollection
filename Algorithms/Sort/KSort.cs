using System;
using System.Collections.Generic;

namespace Algorithms.Sort
{
    /// <summary>
    /// Paiper: https://arxiv.org/ftp/arxiv/papers/1107/1107.3622.pdf
    /// </summary>
    public class KSort : IAlgorithm
    {
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            Console.WriteLine(string.Join(",", data));
            //Sort(0, data.Length - 1);
            Sort_NoRecursion();
            Console.WriteLine(string.Join(",", data));
        }

        private void Sort(int left, int right)
        {
            int key = data[left];
            int i = left;
            int j = right + 1;
            int p = left + 1;
            int k = p;
            int temp = 0;
            bool flag = false;

            do
            {
                if (key <= data[p])
                {
                    if (j != p && j != (right + 1))
                    {
                        data[j] = data[p];
                    }
                    else if (j == right + 1)
                    {
                        temp = data[p];
                        flag = true;
                    }
                    j--;
                    p = j;
                }
                else
                {
                    data[i] = data[p];
                    i++;
                    k++;
                    p = k;
                }
            } while (j - i >= 2);

            data[i] = key;
            if (flag)
            {
                data[i + 1] = temp;
            }

            if (left < i - 1) Sort(left, i - 1);
            if (right > i + 1) Sort(i + 1, right);
        }

        private void Sort_NoRecursion()
        {
            //since it's a kind of QuickSort - use queue to sort sub-arrays
            var queue = new Queue<(int start, int end)>();
            queue.Enqueue((0, data.Length - 1));

            while (queue.Count > 0)
            {
                var interval = queue.Dequeue();

                int key = data[interval.start];
                int i = interval.start;
                int j = interval.end + 1;
                int p = interval.start + 1;
                int k = p;
                int temp = 0;
                bool flag = false;

                do
                {
                    if (key <= data[p])
                    {
                        if (j != p && j != (interval.end + 1))
                        {
                            data[j] = data[p];
                        }
                        else if (j == interval.end + 1)
                        {
                            temp = data[p];
                            flag = true;
                        }

                        j--;
                        p = j;
                    }
                    else
                    {
                        data[i] = data[p];
                        i++;
                        k++;
                        p = k;
                    }
                } while (j - i >= 2);

                data[i] = key;
                if (flag)
                {
                    data[i + 1] = temp;
                }

                if (interval.start < i - 1) queue.Enqueue((interval.start, i - 1)); //Sort(interval.start, i - 1);
                if (interval.end > i + 1) queue.Enqueue((i + 1, interval.end)); //Sort(i + 1, right);
            }
        }
    }
}
