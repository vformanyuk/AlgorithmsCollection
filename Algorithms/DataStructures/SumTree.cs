using System;
using System.Text;

namespace Algorithms.DataStructures
{
    /// <summary>
    /// This implementation works fine for binary tree of 2^X capacity
    /// </summary>
    public class SumTree
    {
        private int _writeIdx;
        private float[] _tree;

        public SumTree(int capacity)
        {
            if (capacity % 2 == 1)
            {
                capacity++;
            }
            Capacity = capacity;
            _tree = new float[capacity * 2 - 1];
        }

        public int Capacity { get; }

        public float Total => _tree[_tree.Length - 1];

        public int Add(float item)
        {
            Propagate(_writeIdx, item - _tree[_writeIdx]);
            _tree[_writeIdx] = item;

            var oldIdx = _writeIdx;
            _writeIdx = ++_writeIdx % Capacity;
            return oldIdx;
        }

        public void Update(float item, int idx)
        {
            Propagate(idx, item - _tree[idx]);
            _tree[idx] = item;
        }

        public void Delete(int idx)
        {
            Propagate(idx, -_tree[idx]);
            _tree[idx] = 0;
        }

        public float Get(float search)
        {
            int lvlBaseIdx = _tree.Length - 1;
            if (_tree[lvlBaseIdx] < search)
            {
                throw new ArgumentOutOfRangeException("search", "Total tree sum is lesser then requested number");
            }

            int searchIdx = lvlBaseIdx;

            int lvlIdx = 0;
            int lvl = 1;

            do
            {
                lvlBaseIdx -= (int)Math.Pow(2, lvl);

                int leftChildIdx = lvlBaseIdx + lvlIdx * 2;
                int rightChildIdx = leftChildIdx + 1;

                if (_tree[leftChildIdx] > search)
                {
                    searchIdx = leftChildIdx;
                }
                else
                {
                    searchIdx = rightChildIdx;
                    search -= _tree[leftChildIdx];
                }

                if (lvlBaseIdx > 0)
                {
                    lvlIdx = searchIdx % lvlBaseIdx;
                }
                lvl++;
            }
            while (lvlBaseIdx >= Capacity);

            return _tree[searchIdx];
        }

        private void Propagate(int idx, float change)
        {
            if (idx >= _tree.Length - 1)
            {
                return;
            }

            int parentIdx = Capacity + idx / 2;
            _tree[parentIdx] += change;
            Propagate(parentIdx, change);
        }

        public override string ToString()
        {
            StringBuilder rep = new StringBuilder();
            int end = Capacity;
            int offset = 0;
            while (end > 0)
            {
                float startInterval = 0;
                float endInterval = 0;

                for (int i = 0; i < end; i++)
                {
                    endInterval += _tree[offset + i];
                    rep.AppendFormat("[{0} - {1}]({2})", startInterval, endInterval, _tree[offset + i]);
                    startInterval = endInterval;
                }
                rep.AppendLine();
                rep.AppendLine();
                offset += end;
                end /= 2;
            }
            return rep.ToString();
        }
    }
}
