using System;
using System.Collections.Generic;

namespace Algorithms
{
    public class Fibb : IAlgorithm
    {
        public void Run()
        {
            // xi = x(i-1)+x(i-2)
            // 0,1,1,2,3,5,8,13,21,34,55

            //Sequence(30); // I tierd waiting. Screw it...
            Console.WriteLine(string.Join(',', Sequence_NoRecursion(30)));
        }

        private int Sequence(int nth)
        {
            if (nth == 0) return 0;
            if (nth == 1) return 1;

            var summ = Sequence(nth - 1) + Sequence(nth - 2);

            return summ;
        }

        private IEnumerable<int> Sequence_NoRecursion(int n)
        {
            int a = 0, b = 1;
            for (int i = 0; i <= n; i++)
            {
                yield return a;
                var temp = a;
                a = b;
                b = temp + b;
            }
        }
    }
}
