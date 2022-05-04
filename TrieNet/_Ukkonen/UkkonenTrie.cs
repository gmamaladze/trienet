using System;
using System.Collections.Generic;
using System.Linq;

namespace Gma.DataStructures.StringSearch
{
    public class UkkonenTrie<K, T> : IGenericSuffixTrie<K, T> where K : IEquatable<K>
    {
        protected readonly int _minSuffixLength;

        //The root of the suffix tree
        private readonly Node<K, WordPosition<T>> _root;

        //The last leaf that was added during the update operation
        private Node<K, WordPosition<T>> _activeLeaf;

        public UkkonenTrie() : this(0)
        {
        }

        public UkkonenTrie(int minSuffixLength) 
        {
            _minSuffixLength = minSuffixLength;
            _root = new Node<K, WordPosition<T>>();
            _activeLeaf = _root;
        }

        public long Size {
            get {
                return _root.Size();
            }
        }

        public IEnumerable<T> Retrieve(ReadOnlySpan<K> word)
        {
            return RetrieveSubstrings(word).Select(o => o.Value).Distinct();
        }

        public IEnumerable<WordPosition<T>> RetrieveSubstrings(ReadOnlySpan<K> word)
        {
            if (word.Length < _minSuffixLength) return Enumerable.Empty<WordPosition<T>>();
            var tmpNode = SearchNode(word);
            return tmpNode == null 
                ? Enumerable.Empty<WordPosition<T>>() 
                : tmpNode.GetData();
        }


        private static bool RegionMatches(ReadOnlySpan<K> first, int toffset, ReadOnlySpan<K> second, int ooffset, int len)
        {
            for (var i = 0; i < len; i++)
            {
                var one = first[toffset + i];
                var two = second[ooffset + i];
                if (!one.Equals(two)) return false;
            }
            return true;
        }

        /**
         * Returns the tree NodeA<T> (if present) that corresponds to the given string.
         */
        private Node<K, WordPosition<T>> SearchNode(ReadOnlySpan<K> word)
        {
            /*
             * Verifies if exists a path from the root to a NodeA<T> such that the concatenation
             * of all the labels on the path is a superstring of the given word.
             * If such a path is found, the last NodeA<T> on it is returned.
             */
            var currentNode = _root;

            for (var i = 0; i < word.Length; ++i)
            {
                var ch = word[i];
                // follow the EdgeA<T> corresponding to this char
                var currentEdge = currentNode.GetEdge(ch);
                if (null == currentEdge)
                {
                    // there is no EdgeA<T> starting with this char
                    return null;
                }
                var label = currentEdge.Label.Span;
                var lenToMatch = Math.Min(word.Length - i, label.Length);

                if (!RegionMatches(word, i, label, 0, lenToMatch))
                {
                    // the label on the EdgeA<T> does not correspond to the one in the string to search
                    return null;
                }

                if (label.Length >= word.Length - i)
                {
                    return currentEdge.Target;
                }
                // advance to next NodeA<T>
                currentNode = currentEdge.Target;
                i += lenToMatch - 1;
            }

            return null;
        }

        public void Add(ReadOnlyMemory<K> key, T value)
        {
            // reset activeLeaf
            _activeLeaf = _root;

            var remainder = key;
            var s = _root;
            var size = key.Length;
            var offset = 0;

            // proceed with tree construction (closely related to procedure in
            // Ukkonen's paper)
            var text = new StringSlice<K>(key, 0, 0);
            // iterate over the string, one char at a time
            for (var i = 0; i < remainder.Length; i++)
            {
                // line 6
                text.EndIndex += 1;

                // line 7: update the tree with the new transitions due to this new char
                (s, text, offset) = Update(s, text, remainder.Slice(i), value, size, offset);
                // line 8: make sure the active Tuple is canonical
                (s, text, offset) = Canonize(s, text, offset);
            }

            // add leaf suffix link, is necessary
            if (null == _activeLeaf.Suffix && _activeLeaf != _root && _activeLeaf != s)
            {
                _activeLeaf.Suffix = s;
            }

        }

        /**
         * Tests whether the string stringPart + t is contained in the subtree that has inputs as root.
         * If that's not the case, and there exists a path of edges e1, e2, ... such that
         *     e1.label + e2.label + ... + $end = stringPart
         * and there is an EdgeA<T> g such that
         *     g.label = stringPart + rest
         * 
         * Then g will be split in two different edges, one having $end as label, and the other one
         * having rest as label.
         *
         * @param inputs the starting NodeA<T>
         * @param stringPart the string to search
         * @param t the following character
         * @param remainder the remainder of the string to add to the index
         * @param value the value to add to the index
         * @return a Tuple containing
         *                  true/false depending on whether (stringPart + t) is contained in the subtree starting in inputs
         *                  the last NodeA<T> that can be reached by following the path denoted by stringPart starting from inputs
         *         
         */
        private static (bool, Node<K, WordPosition<T>>, int) TestAndSplit(Node<K, WordPosition<T>> inputs, StringSlice<K> stringPart, K t, ReadOnlyMemory<K> remainder, T value, int size, int offset)
        {
            // descend the tree as far as possible
            (var s, var str, offset) = Canonize(inputs, stringPart, offset);

            if (str.Length > 0)
            {
                var g = s.GetEdge(str[0]);

                var label = g.Label;
                // must see whether "str" is substring of the label of an EdgeA<T>
                if (label.Length > str.Length && label.Span[str.Length].Equals(t))
                {
                    return (true, s, offset);
                }
                // need to split the EdgeA<T>
                var newlabel = label.Slice(str.Length);
                //assert (label.startsWith(str));

                // build a new NodeA<T>
                var r = new Node<K, WordPosition<T>>();
                // build a new EdgeA<T>
                var newedge = new Edge<K, WordPosition<T>>(str.AsMemory(), r);

                g.Label = newlabel;

                // link s -> r
                r.AddEdge(newlabel.Span[0], g);
                s.AddEdge(str[0], newedge);

                return (false, r, offset + str.Length);
            }
            var e = s.GetEdge(t);
            if (null == e)
            {
                // if there is no t-transtion from s
                return (false, s, offset);
            }
            var eLabelSpan = e.Label.Span;
            var remainderSpan = remainder.Span;
            if (remainderSpan.SequenceEqual(eLabelSpan))
            {
                // update payload of destination NodeA<T>
                e.Target.AddRef(new WordPosition<T>(size - offset - remainder.Length, value));
                return (true, s, offset);
            }
            if (remainderSpan.StartsWith(eLabelSpan))
            {
                return (true, s, offset);
            }
            if (!eLabelSpan.StartsWith(remainderSpan))
            {
                return (true, s, offset);
            }
            // need to split as above
            var newNode = new Node<K, WordPosition<T>>();
            newNode.AddRef(new WordPosition<T>(size - offset - remainder.Length, value));

            var newEdge = new Edge<K, WordPosition<T>>(remainder, newNode);
            e.Label = e.Label.Slice(remainder.Length);
            newNode.AddEdge(e.Label.Span[0], e);
            s.AddEdge(t, newEdge);
            return (false, s, offset);
            // they are different words. No prefix. but they may still share some common substr
        }

        /**
         * Return a (NodeA<T>, string) (n, remainder) Tuple such that n is a farthest descendant of
         * s (the input NodeA<T>) that can be reached by following a path of edges denoting
         * a prefix of inputstr and remainder will be string that must be
         * appended to the concatenation of labels from s to n to get inpustr.
         */
        private static (Node<K, WordPosition<T>>, StringSlice<K>, int) Canonize(Node<K, WordPosition<T>> s, StringSlice<K> inputstr, int offset)
        {

            if (inputstr.Length == 0)
            {
                return (s, inputstr, offset);
            }
            var currentNode = s;
            var g = s.GetEdge(inputstr[0]);
            // descend the tree as long as a proper label is found
            while (g != null && inputstr.Length >= g.Label.Length && inputstr.StartsWith(g.Label.Span))
            {
                inputstr.StartIndex += g.Label.Length;
                offset += g.Label.Length;
                currentNode = g.Target;
                if (inputstr.Length > 0)
                {
                    g = currentNode.GetEdge(inputstr[0]);
                }
            }

            return (currentNode, inputstr, offset);
        }

        /**
         * Updates the tree starting from inputNode and by adding stringPart.
         * 
         * Returns a reference (NodeA<T>, string) Tuple for the string that has been added so far.
         * This means:
         * - the NodeA<T> will be the NodeA<T> that can be reached by the longest path string (S1)
         *   that can be obtained by concatenating consecutive edges in the tree and
         *   that is a substring of the string added so far to the tree.
         * - the string will be the remainder that must be added to S1 to get the string
         *   added so far.
         * 
         * @param inputNode the NodeA<T> to start from
         * @param stringPart the string to add to the tree
         * @param rest the rest of the string
         * @param value the value to add to the index
         */
        private (Node<K, WordPosition<T>>, StringSlice<K>, int) Update(Node<K, WordPosition<T>> inputNode, StringSlice<K> stringPart, ReadOnlyMemory<K> rest, T value, int size, int offset)
        {
            var s = inputNode;
            var tempstr = stringPart;
            var newChar = stringPart[^1];

            // line 1
            var oldroot = _root;

            // line 1b
            var (endpoint, r, offset2) = TestAndSplit(s, tempstr.Slice(0, tempstr.Length - 1), newChar, rest, value, size, offset);

            // line 2
            while (!endpoint)
            {
                // line 3
                var tempEdge = r.GetEdge(newChar);
                Node<K, WordPosition<T>> leaf;
                if (null != tempEdge)
                {
                    // such a NodeA<T> is already present. This is one of the main differences from Ukkonen's case:
                    // the tree can contain deeper nodes at this stage because different strings were added by previous iterations.
                    leaf = tempEdge.Target;
                }
                else
                {
                    // must build a new leaf
                    leaf = new Node<K, WordPosition<T>>();
                    leaf.AddRef(new WordPosition<T>(size - offset2 - rest.Length, value));
                    var newedge = new Edge<K, WordPosition<T>>(rest, leaf);
                    r.AddEdge(newChar, newedge);
                }

                // update suffix link for newly created leaf
                if (_activeLeaf != _root)
                {
                    _activeLeaf.Suffix = leaf;
                }
                _activeLeaf = leaf;

                // line 4
                if (oldroot != _root)
                {
                    oldroot.Suffix = r;
                }

                // line 5
                oldroot = r;

                // line 6
                if (null == s.Suffix)
                {
                    // root NodeA<T>
                    // this is a special case to handle what is referred to as NodeA<T> _|_ on the paper
                    tempstr = tempstr.Slice(1, tempstr.Length - 1);
                }
                else
                {
                    tempstr.EndIndex -= 1;
                    (s, tempstr, offset) = Canonize(s.Suffix, tempstr, offset - 1);
                    tempstr.EndIndex += 1;
                }

                // line 7
                (endpoint, r, offset2) = TestAndSplit(s, SafeCutLastChar(tempstr), newChar, rest, value, size, offset);
            }

            // line 8
            if (oldroot != _root)
            {
                oldroot.Suffix = r;
            }

            return (s, tempstr, offset);
        }

        private static StringSlice<K> SafeCutLastChar(StringSlice<K> seq)
        {
            return seq.Length == 0 ? new StringSlice<K>() : seq.Slice(0, seq.Length - 1);
        }
    }
}