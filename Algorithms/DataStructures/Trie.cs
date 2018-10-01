using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Algorithms.DataStructures
{
    public class Trie
    {
        private readonly Dictionary<char, TrieNode> _subTries = new Dictionary<char, TrieNode>();
        private readonly HashSet<HashContainer> _words = new HashSet<HashContainer>();
        private readonly bool _isCaseSensetive;

        public int WordsCount => _words.Count;

        public Trie(bool caseSensetive = false) => _isCaseSensetive = caseSensetive;

        public void Add(string word)
        {
            var actualWord = _isCaseSensetive ? word : word.ToLower();

            var wordBytes = Encoding.UTF8.GetBytes(actualWord);
            bool isNewWord = false;
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(wordBytes);
                var wordContainer = new HashContainer(hash);
                isNewWord = _words.Add(wordContainer);
            }

            if (!isNewWord) return;

            var firstChar = actualWord[0];
            if (!_subTries.ContainsKey(firstChar))
            {
                _subTries.Add(firstChar, new TrieNode(actualWord));
                return;
            }
            _subTries[firstChar].Add(actualWord);
        }

        public void Clear()
        {
            _subTries.Clear();
            _words.Clear();
        }

        public IEnumerable<string> Get(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                var result = new List<string>();
                foreach (var n in _subTries.OrderBy(x => x.Key).Select(x => x.Value))
                {
                    result.AddRange(n.GetWords(pattern));
                }
                return result;
            }

            var actualWord = _isCaseSensetive ? pattern : pattern.ToLower();

            var node = _subTries[actualWord[0]];

            return node.GetWords(actualWord);
        }

        public bool Remove(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentNullException(nameof(word));
            }

            var actualWord = _isCaseSensetive ? word : word.ToLower();

            var wordBytes = Encoding.UTF8.GetBytes(actualWord);
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(wordBytes);
                var wordContainer = new HashContainer(hash);
                if (!_words.Contains(wordContainer))
                {
                    return false;
                }
            }

            var node = _subTries[actualWord[0]];

            return node.Remove(actualWord);
        }

        private class TrieNode
        {
            public bool IsFinal { get; private set; }
            public string Key { get; private set; }

            private IDictionary<char, TrieNode> _children;

            public TrieNode(string word)
            {
                Key = word;
                IsFinal = true;
            }

            public void Add(string word)
            {
                if (Char.ToUpper(word[0]) != Char.ToUpper(Key[0]))
                {
                    throw new KeyNotFoundException($"Incorrect key {word[0]}");
                }

                if (Key.Length > 1)
                {
                    AppendWord(Key.Substring(1));
                    Key = Key[0].ToString();
                    IsFinal = false;
                }

                if (word.Length == 1)
                {
                    IsFinal = true;
                    return;
                }

                AppendWord(word.Substring(1));
            }

            private void AppendWord(string word)
            {
                if (_children == null)
                {
                    _children = new Dictionary<char, TrieNode>();
                }

                var key = word[0];
                if (!_children.ContainsKey(key))
                {
                    _children.Add(key, new TrieNode(word));
                    return;
                }
                _children[key].Add(word);
            }

            public IEnumerable<string> GetWords(string pattern)
            {
                var originalPatternLength = pattern.Length;
                string result = string.Empty;

                var branches = new Dictionary<TrieNode, string>();
                var nodes = new Stack<TrieNode>();

                nodes.Push(this);

                if (!string.IsNullOrEmpty(pattern))
                {
                    pattern = pattern.Substring(1);
                }

                while (nodes.Count > 0)
                {
                    var node = nodes.Pop();

                    if (branches.ContainsKey(node)) // handle branching
                    {
                        result = branches[node];
                        branches.Remove(node);
                    }
                    result = result + node.Key;

                    if (node.IsFinal && result.Length >= originalPatternLength)
                    {
                        yield return result;
                    }

                    if (node._children == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(pattern))
                    {
                        // Current result must be saved for each branch
                        foreach (var c in node._children.OrderBy(x => x.Key).Select(x => x.Value).Reverse())
                        {
                            nodes.Push(c);
                            branches.Add(c, result);
                        }
                        continue;
                    }

                    if (node._children.TryGetValue(pattern[0], out TrieNode child))
                    {
                        nodes.Push(child);
                    }

                    pattern = pattern.Substring(1);
                }
            }

            public bool Remove(string wordToRemove)
            {
                var backTrace = new Stack<TrieNode>();
                var forward = new Stack<TrieNode>();

                var pattern = wordToRemove.Substring(1);
                string result = string.Empty;
                bool wasRemoved = false;

                forward.Push(this);
                backTrace.Push(this);

                while (forward.Count > 0)
                {
                    var node = forward.Pop();
                    result = result + node.Key;

                    if (node.IsFinal && result == wordToRemove)
                    {
                        node.IsFinal = false;
                        wasRemoved = true;
                        break;
                    }

                    if (node._children == null || string.IsNullOrEmpty(pattern))
                    {
                        break;
                    }

                    if (node._children.TryGetValue(pattern[0], out TrieNode child))
                    {
                        forward.Push(child);
                        backTrace.Push(child);
                    }

                    pattern = pattern.Substring(1);
                }

                TrieNode childToRemove = null;
                while (backTrace.Count > 0 && wasRemoved)
                {
                    var node = backTrace.Pop();
                    if (childToRemove != null)
                    {
                        node._children.Remove(childToRemove.Key[0]);
                    }

                    if (node._children?.Count > 0)
                    {
                        break;
                    }

                    childToRemove = node;
                }
                backTrace.Clear();

                return wasRemoved;
            }

            public override string ToString()
            {
                return $"{this.Key} {(this.IsFinal ? " is Final" : string.Empty)}";
            }
        }

        [StructLayout(layoutKind: LayoutKind.Sequential, Pack = 8)]
        private readonly struct HashContainer
        {
            public ulong Part1 { get; }
            public ulong Part2 { get; }
            public ulong Part3 { get; }
            public ulong Part4 { get; }

            public HashContainer(byte[] hash)
            {
                Part1 = ((ulong)hash[0] << 56) |
                        ((ulong)hash[1] << 48) |
                        ((ulong)hash[2] << 40) |
                        ((ulong)hash[3] << 32) |
                        ((ulong)hash[4] << 24) |
                        ((ulong)hash[5] << 16) |
                        ((ulong)hash[6] << 8) |
                        hash[7];
                Part2 = ((ulong)hash[8] << 56) |
                        ((ulong)hash[9] << 48) |
                        ((ulong)hash[10] << 40) |
                        ((ulong)hash[11] << 32) |
                        ((ulong)hash[12] << 24) |
                        ((ulong)hash[13] << 16) |
                        ((ulong)hash[14] << 8) |
                        hash[15];
                Part3 = ((ulong)hash[16] << 56) |
                        ((ulong)hash[17] << 48) |
                        ((ulong)hash[18] << 40) |
                        ((ulong)hash[19] << 32) |
                        ((ulong)hash[20] << 24) |
                        ((ulong)hash[21] << 16) |
                        ((ulong)hash[22] << 8) |
                        hash[23];
                Part4 = ((ulong)hash[24] << 56) |
                        ((ulong)hash[25] << 48) |
                        ((ulong)hash[26] << 40) |
                        ((ulong)hash[27] << 32) |
                        ((ulong)hash[28] << 24) |
                        ((ulong)hash[29] << 16) |
                        ((ulong)hash[30] << 8) |
                        hash[31];
            }
        }
    }
}
