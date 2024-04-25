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

        public IEnumerable<string> Match(string pattern)
        {
            //if (string.IsNullOrEmpty(pattern))
            //{
            //    var result = new List<string>();
            //    foreach (var n in _subTries.OrderBy(x => x.Key).Select(x => x.Value))
            //    {
            //        result.AddRange(n.GetWords(pattern));
            //    }
            //    return result;
            //}
            var key = _isCaseSensetive ? pattern[0] : Char.ToUpper(pattern[0]);
            return _subTries[key].GetMatches(pattern);
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
            public char Key { get; private set; }
            public char CaseAwareKey { get; }
            public string Suffix { get; private set; }
            public bool HasSuffix => !string.IsNullOrEmpty(Suffix);

            private TrieNode _child;
            private Dictionary<char, TrieNode> _children;

            private readonly bool _isCaseSensetive;

            public TrieNode(string suffix, bool caseSensetive)
            {
                _isCaseSensetive = caseSensetive;
                CaseAwareKey = _isCaseSensetive ? suffix[0] : Char.ToUpper(suffix[0]);

                IsFinal = true;
                Suffix = suffix;
                Key = suffix[0];
            }

            public bool Add(string word, int index = 0)
            {
                if (HasSuffix)
                {
                    //if(string.Compare(Key, word, !_isCaseSensetive) == 0) return false;

                    if (Suffix.Length > 1)
                    {
                        AppendWord(Suffix, 1); //key is not final anymore so it should be distributed further
                        IsFinal = false;
                    }
                    Suffix = string.Empty;
                }

                if ((word.Length - index) == 1)
                {
                    Suffix = string.Empty;
                    bool wasFinal = IsFinal;
                    IsFinal = true;

                    return wasFinal ^ IsFinal;
                }

                return AppendWord(word, index + 1);
            }

            private bool TryGetChild(char key, out TrieNode node)
            {
                bool result = false;
                node = null;

                if (_child != null)
                {
                    node = _child.CaseAwareKey == key ? _child : null;
                    result = node != null;
                }
                else if (_children != null)
                {
                    result = _children.TryGetValue(key, out node);
                }

                return result;
            }

            private bool RemoveChild(char key)
            {
                bool result = false;
                if (_child != null && _child.CaseAwareKey == key)
                {
                    _child = null;
                    result = true;
                }
                else if (_children != null)
                {
                    result = _children.Remove(key);
                    if (_children.Count == 1)
                    {
                        _child = _children[_children.Keys.First()];
                        _children.Clear();
                        _children = null;
                    }
                }
                return result;
            }

            private bool IsEmptyNode()
            {
                return _child is null && (_children?.Count ?? 0) == 0;
            }

            private bool AppendWord(string word, int index)
            {
                if (_child is null && _children is null) // first child being added. It might be the only one
                {
                    _child = new TrieNode(word.Substring(index), _isCaseSensetive);
                    return true;
                }

                var key = _isCaseSensetive ? word[index] : Char.ToUpper(word[index]);
                if (_child != null && _child.CaseAwareKey == key)
                {
                    return _child.Add(word, index);
                }

                if (_children is null) // second child added, use dictionary from now
                {
                    _children = new Dictionary<char, TrieNode>
                    {
                        { _child.CaseAwareKey, _child }
                    };
                    _child = null;
                }

                if (_children.TryGetValue(key, out TrieNode node))
                {
                    return node.Add(word, index);
                }
                _children.Add(key, new TrieNode(word.Substring(index), _isCaseSensetive));
                return true;
            }

            public IEnumerable<string> GetMatches(string pattern)
            {
                var nodes = new Stack<(int patternIndex, TrieNode node)>();
                var tails = new Stack<TrieNode>();

                nodes.Push((1,this));

                while (nodes.Count > 0) //patternIndex < pattern.Length && 
                {
                    var (patternIndex, node) = nodes.Pop();

                    char key = pattern[patternIndex];

                    if (node.TryGetChild(key, out TrieNode child))
                    {
                        if (patternIndex + 1 < pattern.Length)
                        {
                            nodes.Push((patternIndex + 1, child));
                        }
                        else
                        {
                            tails.Push(child);
                        }
                    }

                    if (!_isCaseSensetive)
                    {
                        char branchKey = Char.IsUpper(key) ? Char.ToLower(key) : Char.ToUpper(key);
                        if (node.TryGetChild(branchKey, out TrieNode branchChild))
                        {
                            if (patternIndex + 1 < pattern.Length)
                            {
                                nodes.Push((patternIndex + 1, branchChild));
                            }
                            else
                            {
                                tails.Push(branchChild);
                            }
                        }
                    }
                }

                if (tails.Count == 0)
                {
                    return Enumerable.Empty<string>();
                }



                return null;
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
                    result += node.HasSuffix ? node.Suffix : node.Key.ToString();

                    if (node.IsFinal && result.Length >= originalPatternLength)
                    {
                        yield return result;
                    }

                    if (node.IsEmptyNode())
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(pattern))
                    {
                        // Current result must be saved for each branch
                        if (node._child != null)
                        {
                            nodes.Push(node._child);
                            branches.Add(node._child, result);
                        }
                        else
                        {
                            foreach (var c in node._children.OrderBy(x => x.Key).Select(x => x.Value).Reverse())
                            {
                                nodes.Push(c);
                                branches.Add(c, result);
                            }
                        }
                        continue;
                    }

                    char key = _isCaseSensetive ? pattern[0] : Char.ToUpper(pattern[0]);
                    if (node.TryGetChild(key, out TrieNode child))
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
                    result.Append(node.HasSuffix ? node.Suffix : node.Key.ToString());

                    if (node.IsFinal &&
                        result.Length == wordToRemove.Length &&
                        string.Compare(result.ToString(), wordToRemove, !_isCaseSensetive) == 0)
                    {
                        wasRemoved = true;
                        node.IsFinal = false;
                        if (IsEmptyNode())
                        {
                            removeCandidate = node;
                        }
                        
                        break;
                    }

                    char key = _isCaseSensetive ? wordToRemove[patternIndex] : Char.ToUpper(wordToRemove[patternIndex]);
                    if (!node.TryGetChild(key, out TrieNode child))
                    {
                        break;
                    }

                    forward.Push(child);
                    patternIndex++;
                }

                if (removeCandidate != null)
                {
                    forward.Pop(); // pop removed node
                }

                while (removeCandidate != null && forward.Count > 0)
                {
                    var node = forward.Pop();
                    node.RemoveChild(removeCandidate.CaseAwareKey);
                    
                    if (!node.IsFinal && node.IsEmptyNode())
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
