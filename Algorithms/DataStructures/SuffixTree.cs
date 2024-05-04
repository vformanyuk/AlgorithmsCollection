using Algorithms.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.DataStructures
{
    public class SuffixTree
    {
        private readonly Dictionary<char, SuffixTreeNode> _subSuffixTrees = new Dictionary<char, SuffixTreeNode>();
        private int _wordsCount = 0;

        public int WordsCount => _wordsCount;

        public bool Add(string word)
        {
            var firstChar = word[0];
            if (!_subSuffixTrees.ContainsKey(firstChar))
            {
                _subSuffixTrees.Add(firstChar, new SuffixTreeNode(word));
                _wordsCount++;
                return true;
            }
            bool addResult = _subSuffixTrees[firstChar].Add(word, 0);
            _wordsCount += addResult ? 1 : 0;
            return addResult;
        }

        public void Clear()
        {
            _subSuffixTrees.Clear();
        }

        public IEnumerable<string> Match(string pattern, bool caseSensitive)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return _subSuffixTrees.Keys.OrderBy(key => char.ToUpperInvariant(key))
                                           .Select(key => _subSuffixTrees[key].GetMatches(pattern, caseSensitive))
                                           .SelectMany(x => x);
            }

            IEnumerable<string> result = null;
            if (_subSuffixTrees.TryGetValue(pattern[0], out SuffixTreeNode subSuffixTree))
            {
                result = subSuffixTree.GetMatches(pattern, caseSensitive);
            }

            if (!caseSensitive)
            {
                char branchKey = Char.IsUpper(pattern[0]) ? Char.ToLower(pattern[0]) : Char.ToUpper(pattern[0]);
                if (_subSuffixTrees.TryGetValue(branchKey, out SuffixTreeNode branch))
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
            if(_subSuffixTrees.TryGetValue(key, out SuffixTreeNode node))
            {
                removeResult = node.Remove(word);

                if (node.IsEmptyNode())
                {
                    _subSuffixTrees.Remove(key);
                }

                _wordsCount -= removeResult ? 1 : 0;
            }
            return removeResult;
        }

        public void Merge(SuffixTree merge)
        {
            foreach (var node in merge._subSuffixTrees)
            {
                if (_subSuffixTrees.TryGetValue(node.Key, out SuffixTreeNode subNode))
                {
                    _wordsCount += subNode.Merge(node.Value);
                    SuffixTreeNode.VisitFinalWords(subNode, n => SuffixTreeNode.Compact(n));
                }
                else
                {
                    var clone = (SuffixTreeNode)node.Value.Clone();
                    _subSuffixTrees.Add(node.Key, clone);
                    SuffixTreeNode.VisitFinalWords(clone, _ => _wordsCount++);
                }
            }
        }

        private class SuffixTreeNode : ICloneable
        {
            public bool IsFinal { get; private set; }
            public char Key { get; private set; }
            public string Suffix { get; private set; }
            public bool HasSuffix => (Suffix?.Length ?? 0) > 1;

            private int _keyIndex;

            private SuffixTreeNode _child;
            private Dictionary<char, SuffixTreeNode> _children;

            private SuffixTreeNode _parentNode;

            public SuffixTreeNode(SuffixTreeNode parent, string suffix) 
            {
                _parentNode = parent;
                _keyIndex = parent is null ? 0 : parent._keyIndex + 1;

                IsFinal = true;
                Suffix = suffix;
                Key = suffix[_keyIndex];
            }
            public SuffixTreeNode(string suffix) : this(null, suffix)
            {
            }
            private SuffixTreeNode() { }

            public int Merge(SuffixTreeNode node)
            {
                int mergedWords = 0;
                void increseMergedCounter(SuffixTreeNode _) => mergedWords++;

                if (node.HasSuffix)
                {
                    bool added = Add(node.Suffix, node._keyIndex);
                    mergedWords += (added ? 1 : 0);
                }

                if (node.IsEmptyNode())
                {
                    return mergedWords;
                }

                IEnumerable<SuffixTreeNode> mergeChildren = Enumerable.Empty<SuffixTreeNode>();
                if (node._children != null)
                {
                    mergeChildren = node._children.OrderBy(x => char.ToUpperInvariant(x.Key)).Select(x => x.Value);
                }
                if (node._child != null)
                {
                    mergeChildren = mergeChildren.Chain(node._child.AsEnumerable());
                }

                foreach (var mergeChild in mergeChildren)
                {
                    if (IsEmptyNode())
                    {
                        _child = (SuffixTreeNode)mergeChild.Clone();
                        _child._parentNode = this;
                        _child._keyIndex = this._keyIndex + 1;
                        mergedWords++;
                        continue;
                    }

                    if (_child != null)
                    {
                        if (_child.Key == mergeChild.Key)
                        {
                            mergedWords += _child.Merge(mergeChild);
                        }
                        else
                        {
                            var clone = (SuffixTreeNode)mergeChild.Clone();
                            clone._parentNode = this;
                            clone._keyIndex = this._keyIndex + 1;
                            _children = new Dictionary<char, SuffixTreeNode>()
                            {
                                { _child.Key, _child },
                                { clone.Key, clone }
                            };
                            _child = null;
                            VisitFinalWords(clone, increseMergedCounter);
                        }
                        continue;
                    }

                    if (_children.TryGetValue(mergeChild.Key, out SuffixTreeNode child))
                    {
                        mergedWords += child.Merge(mergeChild);
                    }
                    else
                    {
                        var clone = (SuffixTreeNode)mergeChild.Clone();
                        clone._parentNode = this;
                        clone._keyIndex = this._keyIndex + 1;
                        _children.Add(clone.Key, clone);
                        VisitFinalWords(clone, increseMergedCounter);
                    }
                }

                return mergedWords;
            }

            public static void VisitFinalWords(SuffixTreeNode rootNode, Action<SuffixTreeNode> onFinalVisited)
            {
                Stack<SuffixTreeNode> subNodes = new Stack<SuffixTreeNode>();
                subNodes.Push(rootNode);

                while(subNodes.Count > 0)
                {
                    var node = subNodes.Pop();
                    if (node.IsFinal)
                    {
                        onFinalVisited(node);
                    }

                    if (node.IsEmptyNode())
                    {
                        continue;
                    }

                    if (node._child != null)
                    {
                        subNodes.Push(node._child);
                    }
                    else
                    {
                        foreach(var child in node._children.Values)
                        {
                            subNodes.Push(child);
                        }
                    }
                }
            }

            public bool Add(string word, int index)
            {
                if (Suffix.Length - _keyIndex > 1)
                {
                    AppendWord(Suffix, _keyIndex + 1); //key is not final anymore so it should be distributed further
                    IsFinal = false;
                    Suffix = string.Empty;
                }

                if (word.Length - index == 1)
                {
                    Suffix = word;
                    bool wasFinal = IsFinal;
                    IsFinal = true;
                    return wasFinal ^ IsFinal;
                }

                return AppendWord(word, index + 1);
            }

            private bool AppendWord(string word, int index)
            {
                if (_child is null && _children is null) // first child being added. It might be the only one
                {
                    _child = new SuffixTreeNode(this, word);
                    return true;
                }

                var key = word[index];
                if (_child != null && _child.Key == key)
                {
                    return _child.Add(word, index);
                }

                if (_children is null) // second child added, use dictionary from now
                {
                    _children = new Dictionary<char, SuffixTreeNode>
                    {
                        { _child.Key, _child }
                    };
                    _child = null;
                }

                if (_children.TryGetValue(key, out SuffixTreeNode node))
                {
                    return node.Add(word, index);
                }
                _children.Add(key, new SuffixTreeNode(this, word));
                return true;
            }

            private bool TryGetChild(char key, out SuffixTreeNode node)
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
                var nodes = new Stack<(int patternIndex, SuffixTreeNode node)>();
                var tails = new Stack<SuffixTreeNode>();

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

                    if (node.TryGetChild(key, out SuffixTreeNode child))
                    {
                        nodes.Push((patternIndex, child));
                        nodeAdded = true;
                    }

                    if (!caseSensitive)
                    {
                        char branchKey = Char.IsUpper(key) ? Char.ToLower(key) : Char.ToUpper(key);
                        if (node.TryGetChild(branchKey, out SuffixTreeNode branchChild))
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
                        yield return node.Suffix;
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

            public bool Remove(string wordToRemove)
            {
                int patternIndex = 1;
                SuffixTreeNode currentNode = this;
                bool wasRemoved = false;

                while (currentNode != null)
                {
                    int suffixLength = currentNode.HasSuffix ? currentNode.Suffix.Length : 1;

                    if (suffixLength > wordToRemove.Length)
                    {
                        return false;
                    }

                    if (currentNode.IsFinal && 
                        suffixLength == wordToRemove.Length && 
                        string.Compare(currentNode.Suffix, wordToRemove, false) == 0)
                    {
                        wasRemoved = true;
                        currentNode.IsFinal = false;
                        break;
                    }

                    char key = wordToRemove[patternIndex];
                    if (!currentNode.TryGetChild(key, out SuffixTreeNode childNode))
                    {
                        break;
                    }

                    currentNode = childNode;
                    patternIndex++;
                }

                if (wasRemoved && currentNode.IsEmptyNode())
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

                    if (parentNode != null && parentNode._child != null && parentNode._child.IsEmptyNode())
                    {
                        Compact(parentNode._child);
                    }
                }
                return wasRemoved;
            }

            public static void Compact(SuffixTreeNode compactingTail)
            {
                if (!compactingTail.IsEmptyNode() ||
                    compactingTail._parentNode is null ||
                    compactingTail._parentNode.IsFinal ||
                    compactingTail._parentNode._children != null)
                {
                    return;
                }

                string suffix = compactingTail.Suffix;
                int keyIndex = compactingTail._keyIndex;
                compactingTail.IsFinal = false;

                SuffixTreeNode previousNode = compactingTail;
                SuffixTreeNode currentNode = compactingTail._parentNode;
                while (currentNode != null && currentNode.RemoveChild(previousNode.Key))
                {
                    keyIndex--;

                    previousNode = currentNode;
                    currentNode = currentNode._parentNode;
                }

                if (currentNode is null) // we reached the top level node
                {
                    currentNode = previousNode;
                }

                currentNode.Add(suffix, keyIndex);
            }

            private bool RemoveChild(char key)
            {
                if (_child != null && _child.Key == key && !_child.IsFinal && _child.IsEmptyNode())
                {
                    _child._parentNode = null;
                    _child = null;
                    return true;
                }

                if (_children != null && _children.TryGetValue(key, out SuffixTreeNode child) && !child.IsFinal && child.IsEmptyNode())
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

            public object Clone()
            {
                var node = new SuffixTreeNode
                {
                    IsFinal = IsFinal,
                    Suffix = Suffix,
                    Key = Key,
                };
                node._keyIndex = _keyIndex;

                if (_child != null)
                {
                    node._child = (SuffixTreeNode)_child.Clone();
                }
                else if (_children != null)
                {
                    node._children = new Dictionary<char, SuffixTreeNode>();
                    foreach (var child in _children.Values)
                    {
                        var clone = (SuffixTreeNode)child.Clone();
                        node._children[clone.Key] = clone;
                    }
                }

                return node;
            }
        }
    }
}
