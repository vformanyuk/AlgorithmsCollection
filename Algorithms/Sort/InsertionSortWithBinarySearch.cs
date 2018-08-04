using System;

namespace Algorithms.Sort
{
    public class InsertionSortWithBinarySearch : IAlgorithm
    {
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            Console.WriteLine(string.Join(",", data));
            for (int currentIndex = 1; currentIndex < data.Length; currentIndex++)
            {
                var indexForSwap = this.BinarySearchForInsertIndex(0, currentIndex, data[currentIndex]);
                if (indexForSwap == currentIndex) continue;

                for (int idx = currentIndex; idx > indexForSwap; idx--)
                {
                    var buf = data[idx - 1];
                    data[idx - 1] = data[idx];
                    data[idx] = buf;
                }
            }

            Console.WriteLine(string.Join(",", data));
        }

        private int BinarySearchForInsertIndex(int startIdx, int endIdx, int currentElement)
        {
            if (Math.Abs(endIdx - startIdx) == 1)
            {
                return data[startIdx] > currentElement ? startIdx : endIdx;
            }

            var midIdx = (int) Math.Floor((endIdx - startIdx) / 2f);

            if (currentElement <= data[midIdx])
            {
                return BinarySearchForInsertIndex(startIdx, midIdx, currentElement);
            }

            return BinarySearchForInsertIndex(startIdx + midIdx, endIdx, currentElement);
        }
    }
}
