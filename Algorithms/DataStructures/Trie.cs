using Algorithms.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.DataStructures
{
    public class Trie
    {
        private readonly Dictionary<char, TrieNode> _subTries = new Dictionary<char, TrieNode>();
        private int _wordsCount = 0;

        public int WordsCount => _wordsCount;

        public bool Add(string word)
        {
            var firstChar = word[0];
            if (!_subTries.ContainsKey(firstChar))
            {
                _subTries.Add(firstChar, new TrieNode(word, word.Length));
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

        public IEnumerable<string> Get(string pattern, bool caseSensitive)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                var allContent = new List<string>();
                foreach (var n in _subTries.OrderBy(x => x.Key).Select(x => x.Value))
                {
                    allContent.AddRange(n.GetWords(pattern, caseSensitive));
                }
                return allContent;
            }

            IEnumerable<string> result = null;
            if (_subTries.TryGetValue(pattern[0], out TrieNode subTrie))
            {
                result = subTrie.GetWords(pattern, caseSensitive);
            }

            if (!caseSensitive)
            {
                char branchKey = Char.IsUpper(pattern[0]) ? Char.ToLower(pattern[0]) : Char.ToUpper(pattern[0]);
                if (_subTries.TryGetValue(branchKey, out TrieNode branch))
                {
                    result = result is null ? branch.GetWords(pattern, caseSensitive) : result.Chain(branch.GetWords(pattern, caseSensitive));
                }
            }

            return result ?? Enumerable.Empty<string>();
        }

        public IEnumerable<string> Match(string pattern, bool caseSensitive)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                var allContent = new List<string>();
                foreach (var n in _subTries.OrderBy(x => x.Key).Select(x => x.Value))
                {
                    allContent.AddRange(n.GetMatches(pattern, caseSensitive));
                }
                return allContent;
            }

            IEnumerable<string> result = null;
            if (_subTries.TryGetValue(pattern[0], out TrieNode subTrie))
            {
                result = subTrie.GetMatches(pattern, caseSensitive);
            }

            if (!caseSensitive)
            {
                char branchKey = Char.IsUpper(pattern[0]) ? Char.ToLower(pattern[0]) : Char.ToUpper(pattern[0]);
                if (_subTries.TryGetValue(branchKey, out TrieNode branch))
                {
                    result = result is null ? branch.GetMatches(pattern, caseSensitive) : result.Chain(branch.GetMatches(pattern, caseSensitive));
                }
            }

            return result ?? Enumerable.Empty<string>();
        }

        public bool Remove(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentNullException(nameof(word));
            }

            var key = word[0];
            bool removeResult = false;
            if(_subTries.TryGetValue(key, out TrieNode node))
            {
                removeResult = node.Remove(word);

                if (node.IsEmptyNode())
                {
                    _subTries.Remove(key);
                }

                _wordsCount -= removeResult ? 1 : 0;
            }
            return removeResult;
        }

        private class TrieNode
        {
            public bool IsFinal { get; private set; }
            public char Key { get; private set; }
            public string Suffix { get; private set; }
            public bool HasSuffix => !string.IsNullOrEmpty(Suffix);

            private int _wordTotalLength;
            private TrieNode _child;
            private Dictionary<char, TrieNode> _children;

            private TrieNode _parentNode;

            public TrieNode(TrieNode parent, string suffix, int wordTotalLength) 
            {
                _parentNode = parent;
                _wordTotalLength = wordTotalLength;

                IsFinal = true;
                Suffix = suffix;
                Key = suffix[0];
            }
            public TrieNode(string suffix, int wordTotalLength) : this(null, suffix, wordTotalLength)
            {
            }

            public bool Add(string word, int index = 0)
            {
                if (HasSuffix)
                {
                    if (Suffix.Length > 1)
                    {
                        AppendWord(Suffix, 1, _wordTotalLength); //key is not final anymore so it should be distributed further
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

                return AppendWord(word, index + 1, word.Length);
            }

            private bool AppendWord(string word, int index, int wordLength)
            {
                if (_child is null && _children is null) // first child being added. It might be the only one
                {
                    _child = new TrieNode(this, word.Substring(index), wordLength);
                    return true;
                }

                var key = word[index];
                if (_child != null && _child.Key == key)
                {
                    return _child.Add(word, index);
                }

                if (_children is null) // second child added, use dictionary from now
                {
                    _children = new Dictionary<char, TrieNode>
                    {
                        { _child.Key, _child }
                    };
                    _child = null;
                }

                if (_children.TryGetValue(key, out TrieNode node))
                {
                    return node.Add(word, index);
                }
                _children.Add(key, new TrieNode(this, word.Substring(index), wordLength));
                return true;
            }

            private bool TryGetChild(char key, out TrieNode node)
            {
                bool result = false;
                node = null;

                if (_child != null)
                {
                    node = _child.Key == key ? _child : null;
                    result = node != null;
                }
                else if (_children != null)
                {
                    result = _children.TryGetValue(key, out node);
                }

                return result;
            }

            internal bool IsEmptyNode()
            {
                return _child is null && (_children?.Count ?? 0) == 0;
            }

            public IEnumerable<string> GetMatches(string pattern, bool caseSensitive)
            {
                var nodes = new Stack<(int patternIndex, TrieNode node)>();
                var tails = new Stack<TrieNode>();

                nodes.Push((0,this));
                bool nodeAdded = false;

                while (nodes.Count > 0)
                {
                    var (patternIndex, node) = nodes.Pop();

                    patternIndex++;
                    if (patternIndex >= pattern.Length)
                    {
                        tails.Push(node);
                        continue;
                    }

                    char key = pattern[patternIndex];
                    nodeAdded = false;

                    if (node.TryGetChild(key, out TrieNode child))
                    {
                        nodes.Push((patternIndex, child));
                        nodeAdded = true;
                    }

                    if (!caseSensitive)
                    {
                        char branchKey = Char.IsUpper(key) ? Char.ToLower(key) : Char.ToUpper(key);
                        if (node.TryGetChild(branchKey, out TrieNode branchChild))
                        {
                            nodes.Push((patternIndex, branchChild));
                            nodeAdded = true;
                        }
                    }

                    if (!nodeAdded && node.IsFinal)
                    {
                        tails.Push(node);
                    }
                }

                if (tails.Count == 0)
                {
                    yield break;
                }

                while (tails.Count > 0)
                {
                    var node = tails.Pop();
                    if (node.IsFinal)
                    {
                        yield return ComposeWord(node);
                        if (node.IsEmptyNode())
                        {
                            continue;
                        }
                    }

                    if (node._child != null)
                    {
                        tails.Push(node._child);
                    }
                    else
                    {
                        foreach (var key in node._children.Keys.OrderByDescending(k => k))
                        {
                            tails.Push(node._children[key]);
                        }
                    }
                }
                yield break;
            }

            private string ComposeWord(TrieNode node)
            {
                if (!node.IsFinal)
                {
                    throw new InvalidOperationException();
                }

                char[] word = new char[node._wordTotalLength];
                int wordIndex = node._wordTotalLength - 1;

                if (node.HasSuffix)
                {
                    for (int i = node.Suffix.Length - 1; i >= 0; i--)
                    {
                        word[wordIndex] = node.Suffix[i];
                        wordIndex--;
                    }
                }
                else
                {
                    word[wordIndex] = node.Key;
                    wordIndex--;
                }

                TrieNode parent = node._parentNode;
                while (parent != null)
                {
                    word[wordIndex] = parent.Key;
                    wordIndex--;
                    parent = parent._parentNode;
                }
                return new string(word);
            }

            public IEnumerable<string> GetWords(string pattern, bool caseSensitive)
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

                    char key = caseSensitive ? pattern[0] : Char.ToUpper(pattern[0]);
                    if (node.TryGetChild(key, out TrieNode child))
                    {
                        nodes.Push(child);
                    }

                    pattern = pattern.Substring(1);
                }
            }

            public bool Remove(string wordToRemove)
            {
                int patternIndex = 1;
                char[] pattern = wordToRemove.ToCharArray();

                int resultIndex = 0;
                var result = new char[wordToRemove.Length];
                
                TrieNode currentNode = this;
                bool wasRemoved = false;
                bool needsCompacting = false;

                while (currentNode != null)
                {
                    int suffixLength = currentNode.HasSuffix ? currentNode.Suffix.Length : 1;

                    if (resultIndex + suffixLength > pattern.Length)
                    {
                        return false;
                    }

                    if (suffixLength > 1)
                    {
                        for (int i = 0; i < suffixLength; i++)
                        {
                            result[resultIndex] = currentNode.Suffix[i];
                            resultIndex++;
                        }
                    }
                    else
                    {
                        result[resultIndex] = currentNode.Key;
                        resultIndex++;
                    }

                    if (currentNode.IsFinal && resultIndex == pattern.Length && Enumerable.SequenceEqual(pattern, result))
                    {
                        wasRemoved = true;
                        currentNode.IsFinal = false;
                        if (currentNode.IsEmptyNode())
                        {
                            needsCompacting = true;
                        }
                        break;
                    }

                    if (patternIndex >= pattern.Length)
                    {
                        break;
                    }

                    char key = pattern[patternIndex];
                    if (!currentNode.TryGetChild(key, out TrieNode childNode))
                    {
                        break;
                    }

                    currentNode = childNode;
                    patternIndex++;
                }

                if (needsCompacting)
                {
                    char removeKey = currentNode.Key;
                    var parentNode = currentNode._parentNode;
                    while (parentNode != null)
                    {
                        bool removed = parentNode.RemoveChild(removeKey);
                        if (removed)
                        {
                            removeKey = parentNode.Key;
                            parentNode = parentNode._parentNode;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return wasRemoved;
            }

            private bool RemoveChild(char key)
            {
                if (_child != null && _child.Key == key && _child.IsEmptyNode())
                {
                    _child._parentNode = null;
                    _child = null;
                    return true;
                }

                if (_children != null && _children.TryGetValue(key, out TrieNode child) && child.IsEmptyNode())
                {
                    child._parentNode = null;
                    bool result = _children.Remove(key);
                    if (_children.Count == 1)
                    {
                        _child = _children[_children.Keys.First()];
                        _children.Clear();
                        _children = null;
                    }
                    return result;
                }

                return false;
            }

            public override string ToString()
            {
                return $"{this.Key}{(this.IsFinal ? " is Final" : string.Empty)}";
            }
        }
    }
}
