using System;
using System.Text;

namespace Algorithms.DataStructures
{
    public class SumTree
    {
        private int _writeIdx;
        private int[] _tree;

        public SumTree(int capacity)
        {
            if (capacity % 2 == 1)
            {
                capacity++;
            }
            Capacity = capacity;
            _tree = new int[capacity * 2 - 1];
        }

        public int Capacity { get; }

        public int Total => _tree[_tree.Length - 1];

        public int Add(int item)
        {
            _tree[_writeIdx] = item;

            Propagate(_writeIdx);

            var oldIdx = _writeIdx;

            _writeIdx = ++_writeIdx % Capacity;

            return oldIdx;
        }

        public void Update(int item, int idx)
        {
            _tree[idx] = item;

            Propagate(idx);
        }

        public void Delete(int idx)
        {
            _tree[idx] = 0;
            Propagate(idx);
        }

        public int Get(int search)
        {
            int lvlBaseIdx = _tree.Length - 1;
            if (_tree[lvlBaseIdx] < search)
            {
                throw new ArgumentOutOfRangeException("search", "Total tree sum is lesser then requested number");
            }


            int lvlIdx = 0;
            int searchIdx = lvlBaseIdx;
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
            while (lvlBaseIdx > 0);

            return _tree[searchIdx];
        }

        private void Propagate(int idx, int lvl = 1)
        {
            if (idx >= _tree.Length - 1)
            {
                return;
            }

            int coupleIdx = idx;
            int offset = 0;

            if (idx % 2 == 1)
            {
                coupleIdx--;
                offset = -1;
            }
            else
            {
                coupleIdx++;
            }

            int parentIdx = Capacity / lvl + (idx + offset) / 2;
            _tree[parentIdx] = _tree[idx] + _tree[coupleIdx];
            Propagate(parentIdx, lvl++);
        }

        public override string ToString()
        {
            StringBuilder rep = new StringBuilder();
            int end = Capacity;
            int offset = 0;
            while (end > 0)
            {
                int startInterval = 0;

                for (int i = 0; i < end; i++)
                {
                    rep.AppendFormat("[{0}]({1} - {2})\t", _tree[offset + i], startInterval, startInterval + _tree[offset + i] - 1);
                    startInterval += _tree[offset + i];
                }
                rep.AppendLine();
                offset += end;
                end /= 2;
            }
            return rep.ToString();
        }
    }
}
