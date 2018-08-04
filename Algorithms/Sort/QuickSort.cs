using System;
using System.Collections.Generic;

namespace Algorithms.Sort
{
    public class QuickSort : IAlgorithm
    {
        //private int[] data = new int[34] { 50, 66, 91, 52, 64, 84, 39, 46, 0, 16, 80, 79, 79, 81, 63, 72, 96, 3, 30, 6, 67, 54, 0, 36, 94, 8, 65, 1, 67, 93, 14, 52, 71, 58 };
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            Console.WriteLine(string.Join(",", data));
            //DoQuickSort_MiddlePartition(0, data.Length - 1);
            DoQuickSort_MiddlePartition_NoRecursion();
            //DoQuickSort_Classic(0, data.Length - 1);
            //DoQuickSort_Classic_NoRecursoin();
            Console.WriteLine(string.Join(",", data));
        }

        private void DoQuickSort_Classic(int leftIdx, int rightIdx)
        {
            if (rightIdx - leftIdx == 1)
            {
                if (data[leftIdx] > data[rightIdx])
                {
                    Swap(leftIdx, rightIdx);
                }
                return;
            }

            int wallIdx = leftIdx - 1;
            int pivot = data[rightIdx];

            for (int i = leftIdx; i < rightIdx; i++)
            {
                if (data[i] <= pivot)
                {
                    wallIdx++;
                    if (wallIdx == i) continue; //no self swap
                    Swap(wallIdx, i);
                }
            }

            wallIdx++;
            ShiftBack(rightIdx, wallIdx);

            if (leftIdx != wallIdx) DoQuickSort_Classic(leftIdx, wallIdx - 1);
            if (wallIdx != rightIdx) DoQuickSort_Classic(wallIdx + 1, rightIdx);
        }

        private void DoQuickSort_MiddlePartition(int leftIdx, int rightIdx)
        {
            if (leftIdx == rightIdx) return;

            if (rightIdx - leftIdx == 1)
            {
                if (data[leftIdx] > data[rightIdx])
                {
                    Swap(leftIdx, rightIdx);
                }

                return;
            }

            int pivot = data[leftIdx + (rightIdx - leftIdx) / 2];

            int i = leftIdx;
            int j = rightIdx;
            int delta = rightIdx - leftIdx;

            while (i <= j)
            {
                if (data[i] < pivot) i++;
                if (data[j] > pivot) j--;

                if (delta - (j - i) == 0)
                {
                    Swap(i, j);

                    i++;
                    j--;
                }

                delta = j - i;
            }

            if (leftIdx < i - 1) DoQuickSort_MiddlePartition(leftIdx, i - 1);
            if (i < rightIdx) DoQuickSort_MiddlePartition(i, rightIdx);
        }

        private void DoQuickSort_Classic_NoRecursoin()
        {
            var queue = new Queue<(int start, int end)>();
            queue.Enqueue((0, data.Length - 1));
            while (queue.Count > 0)
            {
                var interval = queue.Dequeue();

                if (interval.end - interval.start == 1)
                {
                    if (data[interval.start] > data[interval.end])
                    {
                        Swap(interval.start, interval.end);
                    }
                    continue;
                }

                int wallIdx = interval.start - 1;
                int pivot = data[interval.end];

                for (int i = interval.start; i < interval.end; i++)
                {
                    if (data[i] <= pivot)
                    {
                        wallIdx++;
                        if (wallIdx == i) continue; //no self swap
                        Swap(wallIdx, i);
                    }
                }

                wallIdx++;
                ShiftBack(interval.end, wallIdx);

                if (interval.start != wallIdx) queue.Enqueue((interval.start, wallIdx - 1));
                if (wallIdx != interval.end) queue.Enqueue((wallIdx + 1, interval.end));
            }
        }

        private void DoQuickSort_MiddlePartition_NoRecursion()
        {
            var queue = new Queue<(int start, int end)>();
            queue.Enqueue((0, data.Length - 1));

            while (queue.Count > 0)
            {
                var interval = queue.Dequeue();

                if (interval.start == interval.end) continue;

                if (interval.end - interval.start == 1)
                {
                    if (data[interval.start] > data[interval.end])
                    {
                        Swap(interval.start, interval.end);
                    }

                    continue;
                }

                int pivot = data[interval.start + (interval.end - interval.start) / 2];

                int i = interval.start;
                int j = interval.end;
                int delta = interval.end - interval.start;

                while (i <= j)
                {
                    if (data[i] < pivot) i++;
                    if (data[j] > pivot) j--;

                    if (delta - (j - i) == 0)
                    {
                        Swap(i, j);

                        i++;
                        j--;
                    }

                    delta = j - i;
                }

                if (interval.start < i - 1)  queue.Enqueue((interval.start, i - 1));
                if (i < interval.end) queue.Enqueue((i, interval.end));
            }
        }

        private void Swap(int i, int j)
        {
            var temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }

        private void ShiftBack(int sourceIdx, int destIdx)
        {
            for (int i = sourceIdx; i > destIdx; i--)
            {
                Swap(i, i - 1);
            }
        }
    }
}
