using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Gma.DataStructures.StringSearch._Ukkonen;

namespace Gma.DataStructures.StringSearch._Ukkonen
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
 * Because of this, a given (startNode, stringSuffix) pair can denote a unique path within the tree, and it is the path (if any) that can be
 * composed by sequentially traversing all the edges (e1, e2, ...) starting from startNode such that (e1.label + e2.label + ...) is equal
 * to the stringSuffix.
 * See the search method for details.
 * 
 * The union of all the edge labels from the root to a given leaf node denotes the set of the strings explicitly contained within the GST.
 * In addition to those Strings, there are a set of different strings that are implicitly contained within the GST, and it is composed of
 * the strings built by concatenating e1.label + e2.label + ... + $end, where e1, e2, ... is a proper path and $end is prefix of any of
 * the labels of the edges starting from the last node of the path.
 *
 * This kind of "implicit path" is important in the testAndSplit method.
 *  
 */
    public class GeneralizedSuffixTree
    {

        /**
         * The index of the last item that was added to the GST
         */
        private int last;

        /**
         * The root of the suffix tree
         */
        private readonly Node root;

        /**
         * The last leaf that was added during the update operation
         */
        private Node activeLeaf;

        public GeneralizedSuffixTree()
        {
            root = new Node();
            activeLeaf = root;
            last = 0;
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
        public IEnumerable<int> search(string word)
        {
            return search(word, -1);
        }

        /**
         * Searches for the given word within the GST and returns at most the given number of matches.
         *
         * @param word the key to search for
         * @param results the max number of results to return
         * @return at most <tt>results</tt> values for the given word
         */
        public IEnumerable<int> search(string word, int results)
        {
            var tmpNode = searchNode(word);
            if (tmpNode == null)
            {
                return Enumerable.Empty<int>();
            }
            return tmpNode.getData(results);
        }

        /**
         * Searches for the given word within the GST and returns at most the given number of matches.
         *
         * @param word the key to search for
         * @param to the max number of results to return
         * @return at most <tt>results</tt> values for the given word
         * @see GeneralizedSuffixTree#ResultInfo
         */
        public ResultInfo searchWithCount(String word, int to)
        {
            Node tmpNode = searchNode(word);
            if (tmpNode == null)
            {
                return new ResultInfo(Enumerable.Empty<int>(), 0);
            }

            return new ResultInfo(tmpNode.getData(to), tmpNode.getResultCount());
        }


        private bool RegionMatches(string first, int toffset, string second, int ooffset, int len)
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
        private Node searchNode(String word)
        {
            /*
             * Verifies if exists a path from the root to a node such that the concatenation
             * of all the labels on the path is a superstring of the given word.
             * If such a path is found, the last node on it is returned.
             */
            Node currentNode = root;
            Edge currentEdge;

            for (int i = 0; i < word.Length; ++i)
            {
                char ch = word[i];
                // follow the edge corresponding to this char
                currentEdge = currentNode.getEdge(ch);
                if (null == currentEdge)
                {
                    // there is no edge starting with this char
                    return null;
                }
                else
                {
                    String label = currentEdge.getLabel();
                    int lenToMatch = Math.Min(word.Length - i, label.Length);

                    if (!RegionMatches(word, i, label, 0, lenToMatch))
                    {
                        // the label on the edge does not correspond to the one in the string to search
                        return null;
                    }

                    if (label.Length >= word.Length - i)
                    {
                        return currentEdge.getDest();
                    }
                    else
                    {
                        // advance to next node
                        currentNode = currentEdge.getDest();
                        i += lenToMatch - 1;
                    }
                }
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
        public void put(String key, int index)
        {
            if (index < last)
            {
                throw new InvalidOperationException(
                    "The input index must not be less than any of the previously inserted ones. Got " + index +
                    ", expected at least " + last);
            }
            else
            {
                last = index;
            }

            // reset activeLeaf
            activeLeaf = root;

            String remainder = key;
            Node s = root;

            // proceed with tree construction (closely related to procedure in
            // Ukkonen's paper)
            String text = "";
            // iterate over the string, one char at a time
            for (int i = 0; i < remainder.Length; i++)
            {
                // line 6
                text += remainder[i];
                // use intern to make sure the resulting string is in the pool.
                //TODO Check if needed
                //text = text.Intern();

                // line 7: update the tree with the new transitions due to this new char
                Pair<Node, String> active = update(s, text, remainder.Substring(i), index);
                // line 8: make sure the active pair is canonical
                active = canonize(active.getFirst(), active.getSecond());

                s = active.getFirst();
                text = active.getSecond();
            }

            // add leaf suffix link, is necessary
            if (null == activeLeaf.getSuffix() && activeLeaf != root && activeLeaf != s)
            {
                activeLeaf.setSuffix(s);
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
         * @return a pair containing
         *                  true/false depending on whether (stringPart + t) is contained in the subtree starting in inputs
         *                  the last node that can be reached by following the path denoted by stringPart starting from inputs
         *         
         */
        private Pair<Boolean, Node> testAndSplit(Node inputs, String stringPart, char t, String remainder, int value)
        {
            // descend the tree as far as possible
            Pair<Node, String> ret = canonize(inputs, stringPart);
            Node s = ret.getFirst();
            String str = ret.getSecond();

            if (!"".Equals(str))
            {
                Edge g = s.getEdge(str[0]);

                String label = g.getLabel();
                // must see whether "str" is substring of the label of an edge
                if (label.Length > str.Length && label[str.Length] == t)
                {
                    return new Pair<Boolean, Node>(true, s);
                }
                else
                {
                    // need to split the edge
                    String newlabel = label.Substring(str.Length);
                    //assert (label.startsWith(str));

                    // build a new node
                    Node r = new Node();
                    // build a new edge
                    Edge newedge = new Edge(str, r);

                    g.setLabel(newlabel);

                    // link s -> r
                    r.addEdge(newlabel[0], g);
                    s.addEdge(str[0], newedge);

                    return new Pair<Boolean, Node>(false, r);
                }

            }
            else
            {
                Edge e = s.getEdge(t);
                if (null == e)
                {
                    // if there is no t-transtion from s
                    return new Pair<Boolean, Node>(false, s);
                }
                else
                {
                    if (remainder.Equals(e.getLabel()))
                    {
                        // update payload of destination node
                        e.getDest().addRef(value);
                        return new Pair<Boolean, Node>(true, s);
                    }
                    else if (remainder.StartsWith(e.getLabel()))
                    {
                        return new Pair<Boolean, Node>(true, s);
                    }
                    else if (e.getLabel().StartsWith(remainder))
                    {
                        // need to split as above
                        Node newNode = new Node();
                        newNode.addRef(value);

                        Edge newEdge = new Edge(remainder, newNode);

                        e.setLabel(e.getLabel().Substring(remainder.Length));

                        newNode.addEdge(e.getLabel()[0], e);

                        s.addEdge(t, newEdge);

                        return new Pair<Boolean, Node>(false, s);
                    }
                    else
                    {
                        // they are different words. No prefix. but they may still share some common substr
                        return new Pair<Boolean, Node>(true, s);
                    }
                }
            }

        }

        /**
         * Return a (Node, String) (n, remainder) pair such that n is a farthest descendant of
         * s (the input node) that can be reached by following a path of edges denoting
         * a prefix of inputstr and remainder will be string that must be
         * appended to the concatenation of labels from s to n to get inpustr.
         */
        private Pair<Node, String> canonize(Node s, String inputstr)
        {

            if ("".Equals(inputstr))
            {
                return new Pair<Node, String>(s, inputstr);
            }
            else
            {
                Node currentNode = s;
                String str = inputstr;
                Edge g = s.getEdge(str[0]);
                // descend the tree as long as a proper label is found
                while (g != null && str.StartsWith(g.getLabel()))
                {
                    str = str.Substring(g.getLabel().Length);
                    currentNode = g.getDest();
                    if (str.Length > 0)
                    {
                        g = currentNode.getEdge(str[0]);
                    }
                }

                return new Pair<Node, String>(currentNode, str);
            }
        }

        /**
         * Updates the tree starting from inputNode and by adding stringPart.
         * 
         * Returns a reference (Node, String) pair for the string that has been added so far.
         * This means:
         * - the Node will be the Node that can be reached by the longest path string (S1)
         *   that can be obtained by concatenating consecutive edges in the tree and
         *   that is a substring of the string added so far to the tree.
         * - the String will be the remainder that must be added to S1 to get the string
         *   added so far.
         * 
         * @param inputNode the node to start from
         * @param stringPart the string to add to the tree
         * @param rest the rest of the string
         * @param value the value to add to the index
         */
        private Pair<Node, String> update(Node inputNode, String stringPart, String rest, int value)
        {
            Node s = inputNode;
            String tempstr = stringPart;
            char newChar = stringPart[stringPart.Length - 1];

            // line 1
            Node oldroot = root;

            // line 1b
            Pair<Boolean, Node> ret = testAndSplit(s, tempstr.Substring(0, tempstr.Length - 1), newChar, rest, value);

            Node r = ret.getSecond();
            bool endpoint = ret.getFirst();

            Node leaf;
            // line 2
            while (!endpoint)
            {
                // line 3
                Edge tempEdge = r.getEdge(newChar);
                if (null != tempEdge)
                {
                    // such a node is already present. This is one of the main differences from Ukkonen's case:
                    // the tree can contain deeper nodes at this stage because different strings were added by previous iterations.
                    leaf = tempEdge.getDest();
                }
                else
                {
                    // must build a new leaf
                    leaf = new Node();
                    leaf.addRef(value);
                    Edge newedge = new Edge(rest, leaf);
                    r.addEdge(newChar, newedge);
                }

                // update suffix link for newly created leaf
                if (activeLeaf != root)
                {
                    activeLeaf.setSuffix(leaf);
                }
                activeLeaf = leaf;

                // line 4
                if (oldroot != root)
                {
                    oldroot.setSuffix(r);
                }

                // line 5
                oldroot = r;

                // line 6
                if (null == s.getSuffix())
                {
                    // root node
                    //TODO Check why assert
                    //assert (root == s);
                    // this is a special case to handle what is referred to as node _|_ on the paper
                    tempstr = tempstr.Substring(1);
                }
                else
                {
                    Pair<Node, String> canret = canonize(s.getSuffix(), safeCutLastChar(tempstr));
                    s = canret.getFirst();
                    // use intern to ensure that tempstr is a reference from the string pool
                    tempstr = (canret.getSecond() + tempstr[tempstr.Length - 1]); //TODO .intern();
                }

                // line 7
                ret = testAndSplit(s, safeCutLastChar(tempstr), newChar, rest, value);
                r = ret.getSecond();
                endpoint = ret.getFirst();

            }

            // line 8
            if (oldroot != root)
            {
                oldroot.setSuffix(r);
            }
            oldroot = root;

            return new Pair<Node, String>(s, tempstr);
        }

        Node getRoot()
        {
            return root;
        }

        private String safeCutLastChar(String seq)
        {
            if (seq.Length == 0)
            {
                return "";
            }
            return seq.Substring(0, seq.Length - 1);
        }

        public int computeCount()
        {
            return root.computeAndCacheCount();
        }

        /**
         * An utility object, used to store the data returned by the GeneralizedSuffixTree GeneralizedSuffixTree.searchWithCount method.
         * It contains a collection of results and the total number of results present in the GST.
         * @see GeneralizedSuffixTree#searchWithCount(java.lang.String, int) 
         */
        public class ResultInfo
        {

            /**
             * The total number of results present in the database
             */
            public int totalResults;

            /**
             * The collection of (some) results present in the GST
             */
            public IEnumerable<int> results;

            public ResultInfo(IEnumerable<int> results, int totalResults)
            {
                this.totalResults = totalResults;
                this.results = results;
            }
        }

        /**
         * A private class used to return a tuples of two elements
         */
        private class Pair<A, B> : Tuple<A, B>
        {

            public A getFirst()
            {
                return this.Item1;
            }

            public B getSecond()
            {
                return this.Item2;
            }

            public Pair(A item1, B item2) : base(item1, item2)
            {
            }
        }
    }
}