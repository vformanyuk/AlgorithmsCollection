using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.DataStructures
{
    public class Trie
    {
        private readonly Dictionary<char, TrieNode> _subTries = new Dictionary<char, TrieNode>();
        private readonly bool _isCaseSensetive;
        private int _wordsCount = 0;

        public int WordsCount => _wordsCount;

        public Trie(bool caseSensetive = false) => _isCaseSensetive = caseSensetive;

        public bool Add(string word)
        {
            var firstChar = _isCaseSensetive ? word[0] : Char.ToUpper(word[0]);
            if (!_subTries.ContainsKey(firstChar))
            {
                _subTries.Add(firstChar, new TrieNode(word, _isCaseSensetive));
                _wordsCount++;
                return true;
            }
            bool addResult = _subTries[firstChar].Add(word);
            _wordsCount += addResult ? 1 : 0;
            return addResult;
        }

        public void Clear()
        {
            _subTries.Clear();
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

            var key = _isCaseSensetive ? pattern[0] : Char.ToUpper(pattern[0]);
            return _subTries[key].GetWords(pattern);
        }

        public bool Remove(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentNullException(nameof(word));
            }

            var key = _isCaseSensetive ? word[0] : Char.ToUpper(word[0]);
            bool removeResult = _subTries[key].Remove(word);
            _wordsCount -= removeResult ? 1 : 0;
            return removeResult;
        }

        private class TrieNode
        {
            public bool IsFinal { get; private set; }
            public string Key { get; private set; }

            private Dictionary<char, TrieNode> _children;

            private readonly bool _isCaseSensetive;

            public TrieNode(string word, bool caseSensetive)
            {
                Key = word;
                IsFinal = true;
                _isCaseSensetive = caseSensetive;
            }

            public bool Add(string word, int index = 0)
            {
                if (Char.ToUpper(word[index]) != Char.ToUpper(Key[0]))
                {
                    throw new KeyNotFoundException($"Incorrect key {word[index]}");
                }

                if (Key.Length > 1)
                {
                    if(string.Compare(Key, word, !_isCaseSensetive) == 0) return false;

                    AppendWord(Key, 1); //key is not final anymore so it should be distributed further
                    Key = Key[0].ToString();
                    IsFinal = false;
                }

                if ((word.Length - index) == 1)
                {
                    bool wasFinal = IsFinal;
                    IsFinal = true;
                    return wasFinal ^ IsFinal;
                }

                return AppendWord(word, index + 1);
            }

            private bool AppendWord(string word, int index)
            {
                if (_children == null)
                {
                    _children = new Dictionary<char, TrieNode>();
                }

                var key = _isCaseSensetive ? word[index] : Char.ToUpper(word[index]);
                if (_children.TryGetValue(key, out TrieNode node))
                {
                    return node.Add(word, index);
                }
                _children.Add(key, new TrieNode(word.Substring(index), _isCaseSensetive));
                return true;
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
                    result += node.Key;

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

                    char key = _isCaseSensetive ? pattern[0] : Char.ToUpper(pattern[0]);
                    if (node._children.TryGetValue(key, out TrieNode child))
                    {
                        nodes.Push(child);
                    }

                    pattern = pattern.Substring(1);
                }
            }

            public bool Remove(string wordToRemove)
            {
                var forward = new Stack<TrieNode>();

                int patternIndex = 1;
                StringBuilder result = new StringBuilder();
                TrieNode removeCandidate = null;
                bool wasRemoved = false;

                forward.Push(this);

                while (forward.Count > 0)
                {
                    var node = forward.Peek();
                    result.Append(node.Key);

                    if (node.IsFinal && 
                        result.Length == wordToRemove.Length && 
                        string.Compare(result.ToString(), wordToRemove, !_isCaseSensetive) == 0)
                    {
                        wasRemoved = true;
                        node.IsFinal = false;
                        if (node._children is null || node._children.Count == 0)
                        {
                            removeCandidate = node;
                        }
                        break;
                    }

                    if (node._children == null)
                    {
                        break;
                    }

                    char key = _isCaseSensetive ? wordToRemove[patternIndex] : Char.ToUpper(wordToRemove[patternIndex]);
                    if (node._children.TryGetValue(key, out TrieNode child))
                    {
                        forward.Push(child);
                    }

                    patternIndex++;
                }

                if (removeCandidate != null)
                {
                    forward.Pop(); // pop removed node
                }

                while (removeCandidate != null && forward.Count > 0)
                {
                    var node = forward.Pop();
                    char key = _isCaseSensetive ? removeCandidate.Key[0] : Char.ToUpper(removeCandidate.Key[0]);
                    node._children.Remove(key);
                    
                    if (!node.IsFinal && node._children.Count == 0)
                    {
                        removeCandidate = node;
                    }
                    else
                    {
                        removeCandidate = null;
                    }
                }

                return wasRemoved;
            }

            public override string ToString()
            {
                return $"{this.Key} {(this.IsFinal ? " is Final" : string.Empty)}";
            }
        }
    }
}
