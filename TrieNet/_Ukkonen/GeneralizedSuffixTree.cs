using System;
using System.Collections.Generic;
using System.Linq;

namespace Gma.DataStructures.StringSearch
{

/**
 * A Generalized Suffix Tree, based on the Ukkonen's paper "On-line construction of suffix trees"
 * http://www.cs.helsinki.fi/u/ukkonen/SuffixT1withFigs.pdf
 *
 * Allows for fast storage and fast(er) retrieval by creating a tree-based index out of a set of strings.
 * Unlike common suffix trees, which are generally used to build an index out of one (very) long string,
 * a Generalized Suffix Tree can be used to build an index over many strings.
 *
 * Its main operations are put and search:
 * Put adds the given key to the index, allowing for later retrieval of the given value.
 * Search can be used to retrieve the set of all the values that were put in the index with keys that contain a given input.
 *
 * In particular, after put(K, V), search(H) will return a set containing V for any string H that is substring of K.
 *
 * The overall complexity of the retrieval operation (search) is O(m) where m is the length of the string to search within the index.
 *
 * Although the implementation is based on the original design by Ukkonen, there are a few aspects where it differs significantly.
 * 
 * The tree is composed of a set of nodes and labeled edges. The labels on the edges can have any length as long as it's greater than 0.
 * The only constraint is that no two edges going out from the same node will start with the same character.
 * 
 * Because of this, a given (startNode, stringSuffix) Tuple can denote a unique path within the tree, and it is the path (if any) that can be
 * composed by sequentially traversing all the edges (e1, e2, ...) starting from startNode such that (e1.label + e2.label + ...) is equal
 * to the stringSuffix.
 * See the search method for details.
 * 
 * The union of all the edge labels from the root to a given leaf node denotes the set of the strings explicitly contained within the GST.
 * In addition to those strings, there are a set of different strings that are implicitly contained within the GST, and it is composed of
 * the strings built by concatenating e1.label + e2.label + ... + $end, where e1, e2, ... is a proper path and $end is prefix of any of
 * the labels of the edges starting from the last node of the path.
 *
 * This kind of "implicit path" is important in the testAndSplit method.
 *  
 */
    internal class GeneralizedSuffixTree
    {

        //The index of the last item that was added to the GST
        private int _last;

        //The root of the suffix tree
        private readonly Node _root;

        //The last leaf that was added during the update operation
        private Node _activeLeaf;

        public GeneralizedSuffixTree()
        {
            _root = new Node();
            _activeLeaf = _root;
            _last = 0;
        }

        /**
         * Searches for the given word within the GST.
         *
         * Returns all the indexes for which the key contains the <tt>word</tt> that was
         * supplied as input.
         *
         * @param word the key to search for
         * @return the collection of indexes associated with the input <tt>word</tt>
         */
        public IEnumerable<int> Search(string word)
        {
            return Search(word, -1);
        }

        /**
         * Searches for the given word within the GST and returns at most the given number of matches.
         *
         * @param word the key to search for
         * @param results the max number of results to return
         * @return at most <tt>results</tt> values for the given word
         */
        public IEnumerable<int> Search(string word, int results)
        {
            var tmpNode = SearchNode(word);
            return tmpNode == null 
                ? Enumerable.Empty<int>() 
                : tmpNode.GetData(results);
        }

        /**
         * Searches for the given word within the GST and returns at most the given number of matches.
         *
         * @param word the key to search for
         * @param to the max number of results to return
         * @return at most <tt>results</tt> values for the given word
         * @see GeneralizedSuffixTree#ResultInfo
         */
        public ResultInfo SearchWithCount(string word, int to)
        {
            var tmpNode = SearchNode(word);
            return tmpNode == null 
                ? new ResultInfo(Enumerable.Empty<int>(), 0) 
                : new ResultInfo(tmpNode.GetData(to), tmpNode.GetResultCount());
        }


        private static bool RegionMatches(string first, int toffset, string second, int ooffset, int len)
        {
            for (var i = 0; i < len; i++)
            {
                var one = first[toffset + i];
                var two = second[ooffset + i];
                if (one != two) return false;
            }
            return true;
        }

        /**
         * Returns the tree node (if present) that corresponds to the given string.
         */
        private Node SearchNode(string word)
        {
            /*
             * Verifies if exists a path from the root to a node such that the concatenation
             * of all the labels on the path is a superstring of the given word.
             * If such a path is found, the last node on it is returned.
             */
            var currentNode = _root;

            for (var i = 0; i < word.Length; ++i)
            {
                var ch = word[i];
                // follow the edge corresponding to this char
                var currentEdge = currentNode.GetEdge(ch);
                if (null == currentEdge)
                {
                    // there is no edge starting with this char
                    return null;
                }
                var label = currentEdge.Label;
                var lenToMatch = Math.Min(word.Length - i, label.Length);

                if (!RegionMatches(word, i, label, 0, lenToMatch))
                {
                    // the label on the edge does not correspond to the one in the string to search
                    return null;
                }

                if (label.Length >= word.Length - i)
                {
                    return currentEdge.Target;
                }
                // advance to next node
                currentNode = currentEdge.Target;
                i += lenToMatch - 1;
            }

            return null;
        }

        /**
         * Adds the specified <tt>index</tt> to the GST under the given <tt>key</tt>.
         *
         * Entries must be inserted so that their indexes are in non-decreasing order,
         * otherwise an IllegalStateException will be raised.
         *
         * @param key the string key that will be added to the index
         * @param index the value that will be added to the index
         * @throws IllegalStateException if an invalid index is passed as input
         */
        public void Put(string key, int index)
        {
            if (index < _last)
            {
                throw new InvalidOperationException(
                    "The input index must not be less than any of the previously inserted ones. Got " + index +
                    ", expected at least " + _last);
            }
            else
            {
                _last = index;
            }

            // reset activeLeaf
            _activeLeaf = _root;

            var remainder = key;
            var s = _root;

            // proceed with tree construction (closely related to procedure in
            // Ukkonen's paper)
            var text = string.Empty;
            // iterate over the string, one char at a time
            for (var i = 0; i < remainder.Length; i++)
            {
                // line 6
                text += remainder[i];
                // use intern to make sure the resulting string is in the pool.
                //TODO Check if needed
                //text = text.Intern();

                // line 7: update the tree with the new transitions due to this new char
                var active = Update(s, text, remainder.Substring(i), index);
                // line 8: make sure the active Tuple is canonical
                active = Canonize(active.Item1, active.Item2);

                s = active.Item1;
                text = active.Item2;
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
         * and there is an edge g such that
         *     g.label = stringPart + rest
         * 
         * Then g will be split in two different edges, one having $end as label, and the other one
         * having rest as label.
         *
         * @param inputs the starting node
         * @param stringPart the string to search
         * @param t the following character
         * @param remainder the remainder of the string to add to the index
         * @param value the value to add to the index
         * @return a Tuple containing
         *                  true/false depending on whether (stringPart + t) is contained in the subtree starting in inputs
         *                  the last node that can be reached by following the path denoted by stringPart starting from inputs
         *         
         */
        private Tuple<bool, Node> TestAndSplit(Node inputs, string stringPart, char t, string remainder, int value)
        {
            // descend the tree as far as possible
            var ret = Canonize(inputs, stringPart);
            var s = ret.Item1;
            var str = ret.Item2;

            if (!string.Empty.Equals(str))
            {
                var g = s.GetEdge(str[0]);

                var label = g.Label;
                // must see whether "str" is substring of the label of an edge
                if (label.Length > str.Length && label[str.Length] == t)
                {
                    return new Tuple<bool, Node>(true, s);
                }
                // need to split the edge
                var newlabel = label.Substring(str.Length);
                //assert (label.startsWith(str));

                // build a new node
                var r = new Node();
                // build a new edge
                var newedge = new Edge(str, r);

                g.Label = newlabel;

                // link s -> r
                r.AddEdge(newlabel[0], g);
                s.AddEdge(str[0], newedge);

                return new Tuple<bool, Node>(false, r);
            }
            var e = s.GetEdge(t);
            if (null == e)
            {
                // if there is no t-transtion from s
                return new Tuple<Boolean, Node>(false, s);
            }
            if (remainder.Equals(e.Label))
            {
                // update payload of destination node
                e.Target.AddRef(value);
                return new Tuple<bool, Node>(true, s);
            }
            if (remainder.StartsWith(e.Label))
            {
                return new Tuple<bool, Node>(true, s);
            }
            if (!e.Label.StartsWith(remainder))
            {
                return new Tuple<bool, Node>(true, s);
            }
            // need to split as above
            var newNode = new Node();
            newNode.AddRef(value);

            var newEdge = new Edge(remainder, newNode);
            e.Label = e.Label.Substring(remainder.Length);
            newNode.AddEdge(e.Label[0], e);
            s.AddEdge(t, newEdge);
            return new Tuple<bool, Node>(false, s);
            // they are different words. No prefix. but they may still share some common substr
        }

        /**
         * Return a (Node, string) (n, remainder) Tuple such that n is a farthest descendant of
         * s (the input node) that can be reached by following a path of edges denoting
         * a prefix of inputstr and remainder will be string that must be
         * appended to the concatenation of labels from s to n to get inpustr.
         */
        private static Tuple<Node, string> Canonize(Node s, string inputstr)
        {

            if (string.Empty.Equals(inputstr))
            {
                return new Tuple<Node, string>(s, inputstr);
            }
            var currentNode = s;
            var str = inputstr;
            var g = s.GetEdge(str[0]);
            // descend the tree as long as a proper label is found
            while (g != null && str.StartsWith(g.Label))
            {
                str = str.Substring(g.Label.Length);
                currentNode = g.Target;
                if (str.Length > 0)
                {
                    g = currentNode.GetEdge(str[0]);
                }
            }

            return new Tuple<Node, string>(currentNode, str);
        }

        /**
         * Updates the tree starting from inputNode and by adding stringPart.
         * 
         * Returns a reference (Node, string) Tuple for the string that has been added so far.
         * This means:
         * - the Node will be the Node that can be reached by the longest path string (S1)
         *   that can be obtained by concatenating consecutive edges in the tree and
         *   that is a substring of the string added so far to the tree.
         * - the string will be the remainder that must be added to S1 to get the string
         *   added so far.
         * 
         * @param inputNode the node to start from
         * @param stringPart the string to add to the tree
         * @param rest the rest of the string
         * @param value the value to add to the index
         */
        private Tuple<Node, string> Update(Node inputNode, string stringPart, string rest, int value)
        {
            var s = inputNode;
            var tempstr = stringPart;
            var newChar = stringPart[stringPart.Length - 1];

            // line 1
            var oldroot = _root;

            // line 1b
            var ret = TestAndSplit(s, tempstr.Substring(0, tempstr.Length - 1), newChar, rest, value);

            var r = ret.Item2;
            var endpoint = ret.Item1;

            // line 2
            while (!endpoint)
            {
                // line 3
                var tempEdge = r.GetEdge(newChar);
                Node leaf;
                if (null != tempEdge)
                {
                    // such a node is already present. This is one of the main differences from Ukkonen's case:
                    // the tree can contain deeper nodes at this stage because different strings were added by previous iterations.
                    leaf = tempEdge.Target;
                }
                else
                {
                    // must build a new leaf
                    leaf = new Node();
                    leaf.AddRef(value);
                    Edge newedge = new Edge(rest, leaf);
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
                    // root node
                    //TODO Check why assert
                    //assert (root == s);
                    // this is a special case to handle what is referred to as node _|_ on the paper
                    tempstr = tempstr.Substring(1);
                }
                else
                {
                    var canret = Canonize(s.Suffix, SafeCutLastChar(tempstr));
                    s = canret.Item1;
                    // use intern to ensure that tempstr is a reference from the string pool
                    tempstr = (canret.Item2 + tempstr[tempstr.Length - 1]); //TODO .intern();
                }

                // line 7
                ret = TestAndSplit(s, SafeCutLastChar(tempstr), newChar, rest, value);
                r = ret.Item2;
                endpoint = ret.Item1;
            }

            // line 8
            if (oldroot != _root)
            {
                oldroot.Suffix = r;
            }

            return new Tuple<Node, string>(s, tempstr);
        }

        private static string SafeCutLastChar(string seq)
        {
            return seq.Length == 0 ? string.Empty : seq.Substring(0, seq.Length - 1);
        }

        public int ComputeCount()
        {
            return _root.ComputeAndCacheCount();
        }
    }
}