using System;
using System.Collections.Generic;

namespace Algorithms.Sort
{
    public class TreeSort : IAlgorithm
    {
        private int[] data = new int[12] { 7, 12, 3, 9, 4, 56, 9, 0, 2, 5, 1, 2 };

        public void Run()
        {
            Dictionary<int, ValueContainer> tree = new Dictionary<int, ValueContainer>
            {
                {0, new ValueContainer {Value = data[0]}}
            };

            Console.WriteLine(string.Join(",", data));
            for (int i = 1; i < data.Length; i++)
            {
                int idx = 0;
                while (tree.ContainsKey(idx))
                {
                    if (data[i] < tree[idx].Value) // left branch
                    {
                        idx = idx * 2 + 1;
                        continue;
                    }
                    if (data[i] > tree[idx].Value) // right branch
                    {
                        idx = idx * 2 + 2;
                        continue;
                    }

                    if (data[i] == tree[idx].Value)
                    {
                        tree[idx].Count++;
                        idx = -1;
                        break;
                    }
                }

                if (idx >= 0)
                {
                    tree.Add(idx, new ValueContainer {Value = data[i]});
                }
            }

            TraverseTree_NonRecursive(tree);
            Console.WriteLine();
            TraverseTree(tree);
        }

        private void TraverseTree_NonRecursive(Dictionary<int, ValueContainer> tree)
        {
            var stack = new Stack<int>();
            int idx = 0;

            stack.Push(idx);

            while (stack.Count > 0)
            {
                idx = idx * 2 + 1;
                if (tree.ContainsKey(idx))
                {
                    stack.Push(idx);
                    continue;
                }

                idx = stack.Pop();
                Console.Write(tree[idx] + " ");

                idx = idx * 2 + 2;
                if (tree.ContainsKey(idx))
                {
                    stack.Push(idx);
                    //continue;
                }
            }

        }

        private void TraverseTree(Dictionary<int, ValueContainer> tree, int idx = 0)
        {
            int lftIdx = idx * 2 + 1;
            if (tree.ContainsKey(lftIdx))
            {
                TraverseTree(tree, lftIdx);
            }

            Console.Write(tree[idx] + " ");

            int rghtIdx = idx * 2 + 2;
            if (tree.ContainsKey(rghtIdx))
            {
                TraverseTree(tree, rghtIdx);
            }
        }

        private class ValueContainer
        {
            public int Value { get; set; }
            public uint Count { get; set; } = 1;

            public override string ToString()
            {
                if (Count > 1)
                {
                    return $"{Value}({Count})";
                }

                return Value.ToString();
            }
        }
    }
}
