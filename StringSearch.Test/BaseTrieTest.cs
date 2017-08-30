// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace Gma.DataStructures.StringSearch.Test
{
    [TestFixture]
    public abstract class BaseTrieTest
    {
        protected ITrie<int> Trie { get; private set; }

        [TestFixtureSetUp]
        public virtual void Setup()
        {
            Trie = CreateTrie();
            for (int i = 0; i < Words40.Length; i++)
            {
                Trie.Add(Words40[i], i);
            }
        }

        protected abstract ITrie<int> CreateTrie();

        public string[] Words40 = new[] {
                                            "daubreelite",
                                            "daubingly",
                                            "daubingly",
                                            "phycochromaceous",
                                            "phycochromaceae",
                                            "phycite",
                                            "athymic",
                                            "athwarthawse",
                                            "athrotaxis",
                                            "unaccorded",
                                            "unaccordant",
                                            "unaccord",
                                            "kokoona",
                                            "koko",
                                            "koklas",
                                            "s",
                                            "flexibilty",
                                            "flexanimous",
                                            "collochemistry",
                                            "collochemistry",
                                            "collocationable",
                                            "capomo",
                                            "capoc",
                                            "capoc",
                                            "ungivingness",
                                            "ungiveable",
                                            "ungive",
                                            "prestandard",
                                            "prestandard",
                                            "prestabilism",
                                            "megalocornea",
                                            "megalocephalia",
                                            "megalocephalia",
                                            "afaced",
                                            "aettekees",
                                            "aetites",
                                            "comolecule",
                                            "comodato",
                                            "comodato",
                                            "cognoscibility"
                                        };

        [TestCase("d", new[] { 0, 1, 2 })]
        [TestCase("da", new[] { 0, 1, 2 })]
        [TestCase("dau", new[] { 0, 1, 2 })]
        [TestCase("daub", new[] { 0, 1, 2 })]
        [TestCase("daubr", new[] { 0 })]
        [TestCase("daubre", new[] { 0 })]
        [TestCase("daubree", new[] { 0 })]
        [TestCase("daubreel", new[] { 0 })]
        [TestCase("daubreeli", new[] { 0 })]
        [TestCase("daubreelit", new[] { 0 })]
        [TestCase("daubreelite", new[] { 0 })]
        [TestCase("d", new[] { 0, 1, 2 })]
        [TestCase("da", new[] { 0, 1, 2 })]
        [TestCase("dau", new[] { 0, 1, 2 })]
        [TestCase("daub", new[] { 0, 1, 2 })]
        [TestCase("daubi", new[] { 1, 2 })]
        [TestCase("daubin", new[] { 1, 2 })]
        [TestCase("daubing", new[] { 1, 2 })]
        [TestCase("daubingl", new[] { 1, 2 })]
        [TestCase("daubingly", new[] { 1, 2 })]
        [TestCase("d", new[] { 0, 1, 2 })]
        [TestCase("da", new[] { 0, 1, 2 })]
        [TestCase("dau", new[] { 0, 1, 2 })]
        [TestCase("daub", new[] { 0, 1, 2 })]
        [TestCase("daubi", new[] { 1, 2 })]
        [TestCase("daubin", new[] { 1, 2 })]
        [TestCase("daubing", new[] { 1, 2 })]
        [TestCase("daubingl", new[] { 1, 2 })]
        [TestCase("daubingly", new[] { 1, 2 })]
        [TestCase("p", new[] { 3, 4, 5, 27, 28, 29 })]
        [TestCase("ph", new[] { 3, 4, 5 })]
        [TestCase("phy", new[] { 3, 4, 5 })]
        [TestCase("phyc", new[] { 3, 4, 5 })]
        [TestCase("phyco", new[] { 3, 4 })]
        [TestCase("phycoc", new[] { 3, 4 })]
        [TestCase("phycoch", new[] { 3, 4 })]
        [TestCase("phycochr", new[] { 3, 4 })]
        [TestCase("phycochro", new[] { 3, 4 })]
        [TestCase("phycochrom", new[] { 3, 4 })]
        [TestCase("phycochroma", new[] { 3, 4 })]
        [TestCase("phycochromac", new[] { 3, 4 })]
        [TestCase("phycochromace", new[] { 3, 4 })]
        [TestCase("phycochromaceo", new[] { 3 })]
        [TestCase("phycochromaceou", new[] { 3 })]
        [TestCase("phycochromaceous", new[] { 3 })]
        [TestCase("p", new[] { 3, 4, 5, 27, 28, 29 })]
        [TestCase("ph", new[] { 3, 4, 5 })]
        [TestCase("phy", new[] { 3, 4, 5 })]
        [TestCase("phyc", new[] { 3, 4, 5 })]
        [TestCase("phyco", new[] { 3, 4 })]
        [TestCase("phycoc", new[] { 3, 4 })]
        [TestCase("phycoch", new[] { 3, 4 })]
        [TestCase("phycochr", new[] { 3, 4 })]
        [TestCase("phycochro", new[] { 3, 4 })]
        [TestCase("phycochrom", new[] { 3, 4 })]
        [TestCase("phycochroma", new[] { 3, 4 })]
        [TestCase("phycochromac", new[] { 3, 4 })]
        [TestCase("phycochromace", new[] { 3, 4 })]
        [TestCase("phycochromacea", new[] { 4 })]
        [TestCase("phycochromaceae", new[] { 4 })]
        [TestCase("p", new[] { 3, 4, 5, 27, 28, 29 })]
        [TestCase("ph", new[] { 3, 4, 5 })]
        [TestCase("phy", new[] { 3, 4, 5 })]
        [TestCase("phyc", new[] { 3, 4, 5 })]
        [TestCase("phyci", new[] { 5 })]
        [TestCase("phycit", new[] { 5 })]
        [TestCase("phycite", new[] { 5 })]
        [TestCase("a", new[] { 6, 7, 8, 33, 34, 35 })]
        [TestCase("at", new[] { 6, 7, 8 })]
        [TestCase("ath", new[] { 6, 7, 8 })]
        [TestCase("athy", new[] { 6 })]
        [TestCase("athym", new[] { 6 })]
        [TestCase("athymi", new[] { 6 })]
        [TestCase("athymic", new[] { 6 })]
        [TestCase("a", new[] { 6, 7, 8, 33, 34, 35 })]
        [TestCase("at", new[] { 6, 7, 8 })]
        [TestCase("ath", new[] { 6, 7, 8 })]
        [TestCase("athw", new[] { 7 })]
        [TestCase("athwa", new[] { 7 })]
        [TestCase("athwar", new[] { 7 })]
        [TestCase("athwart", new[] { 7 })]
        [TestCase("athwarth", new[] { 7 })]
        [TestCase("athwartha", new[] { 7 })]
        [TestCase("athwarthaw", new[] { 7 })]
        [TestCase("athwarthaws", new[] { 7 })]
        [TestCase("athwarthawse", new[] { 7 })]
        [TestCase("a", new[] { 6, 7, 8, 33, 34, 35 })]
        [TestCase("at", new[] { 6, 7, 8 })]
        [TestCase("ath", new[] { 6, 7, 8 })]
        [TestCase("athr", new[] { 8 })]
        [TestCase("athro", new[] { 8 })]
        [TestCase("athrot", new[] { 8 })]
        [TestCase("athrota", new[] { 8 })]
        [TestCase("athrotax", new[] { 8 })]
        [TestCase("athrotaxi", new[] { 8 })]
        [TestCase("athrotaxis", new[] { 8 })]
        [TestCase("u", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("un", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("una", new[] { 9, 10, 11 })]
        [TestCase("unac", new[] { 9, 10, 11 })]
        [TestCase("unacc", new[] { 9, 10, 11 })]
        [TestCase("unacco", new[] { 9, 10, 11 })]
        [TestCase("unaccor", new[] { 9, 10, 11 })]
        [TestCase("unaccord", new[] { 9, 10, 11 })]
        [TestCase("unaccorde", new[] { 9 })]
        [TestCase("unaccorded", new[] { 9 })]
        [TestCase("u", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("un", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("una", new[] { 9, 10, 11 })]
        [TestCase("unac", new[] { 9, 10, 11 })]
        [TestCase("unacc", new[] { 9, 10, 11 })]
        [TestCase("unacco", new[] { 9, 10, 11 })]
        [TestCase("unaccor", new[] { 9, 10, 11 })]
        [TestCase("unaccord", new[] { 9, 10, 11 })]
        [TestCase("unaccorda", new[] { 10 })]
        [TestCase("unaccordan", new[] { 10 })]
        [TestCase("unaccordant", new[] { 10 })]
        [TestCase("u", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("un", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("una", new[] { 9, 10, 11 })]
        [TestCase("unac", new[] { 9, 10, 11 })]
        [TestCase("unacc", new[] { 9, 10, 11 })]
        [TestCase("unacco", new[] { 9, 10, 11 })]
        [TestCase("unaccor", new[] { 9, 10, 11 })]
        [TestCase("unaccord", new[] { 9, 10, 11 })]
        [TestCase("k", new[] { 12, 13, 14 })]
        [TestCase("ko", new[] { 12, 13, 14 })]
        [TestCase("kok", new[] { 12, 13, 14 })]
        [TestCase("koko", new[] { 12, 13 })]
        [TestCase("kokoo", new[] { 12 })]
        [TestCase("kokoon", new[] { 12 })]
        [TestCase("kokoona", new[] { 12 })]
        [TestCase("k", new[] { 12, 13, 14 })]
        [TestCase("ko", new[] { 12, 13, 14 })]
        [TestCase("kok", new[] { 12, 13, 14 })]
        [TestCase("koko", new[] { 12, 13 })]
        [TestCase("k", new[] { 12, 13, 14 })]
        [TestCase("ko", new[] { 12, 13, 14 })]
        [TestCase("kok", new[] { 12, 13, 14 })]
        [TestCase("kokl", new[] { 14 })]
        [TestCase("kokla", new[] { 14 })]
        [TestCase("koklas", new[] { 14 })]
        [TestCase("s", new[] { 15 })]
        [TestCase("f", new[] { 16, 17 })]
        [TestCase("fl", new[] { 16, 17 })]
        [TestCase("fle", new[] { 16, 17 })]
        [TestCase("flex", new[] { 16, 17 })]
        [TestCase("flexi", new[] { 16 })]
        [TestCase("flexib", new[] { 16 })]
        [TestCase("flexibi", new[] { 16 })]
        [TestCase("flexibil", new[] { 16 })]
        [TestCase("flexibilt", new[] { 16 })]
        [TestCase("flexibilty", new[] { 16 })]
        [TestCase("f", new[] { 16, 17 })]
        [TestCase("fl", new[] { 16, 17 })]
        [TestCase("fle", new[] { 16, 17 })]
        [TestCase("flex", new[] { 16, 17 })]
        [TestCase("flexa", new[] { 17 })]
        [TestCase("flexan", new[] { 17 })]
        [TestCase("flexani", new[] { 17 })]
        [TestCase("flexanim", new[] { 17 })]
        [TestCase("flexanimo", new[] { 17 })]
        [TestCase("flexanimou", new[] { 17 })]
        [TestCase("flexanimous", new[] { 17 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("co", new[] { 18, 19, 20, 36, 37, 38, 39 })]
        [TestCase("col", new[] { 18, 19, 20 })]
        [TestCase("coll", new[] { 18, 19, 20 })]
        [TestCase("collo", new[] { 18, 19, 20 })]
        [TestCase("colloc", new[] { 18, 19, 20 })]
        [TestCase("colloch", new[] { 18, 19 })]
        [TestCase("colloche", new[] { 18, 19 })]
        [TestCase("collochem", new[] { 18, 19 })]
        [TestCase("collochemi", new[] { 18, 19 })]
        [TestCase("collochemis", new[] { 18, 19 })]
        [TestCase("collochemist", new[] { 18, 19 })]
        [TestCase("collochemistr", new[] { 18, 19 })]
        [TestCase("collochemistry", new[] { 18, 19 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("co", new[] { 18, 19, 20, 36, 37, 38, 39 })]
        [TestCase("col", new[] { 18, 19, 20 })]
        [TestCase("coll", new[] { 18, 19, 20 })]
        [TestCase("collo", new[] { 18, 19, 20 })]
        [TestCase("colloc", new[] { 18, 19, 20 })]
        [TestCase("colloch", new[] { 18, 19 })]
        [TestCase("colloche", new[] { 18, 19 })]
        [TestCase("collochem", new[] { 18, 19 })]
        [TestCase("collochemi", new[] { 18, 19 })]
        [TestCase("collochemis", new[] { 18, 19 })]
        [TestCase("collochemist", new[] { 18, 19 })]
        [TestCase("collochemistr", new[] { 18, 19 })]
        [TestCase("collochemistry", new[] { 18, 19 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("co", new[] { 18, 19, 20, 36, 37, 38, 39 })]
        [TestCase("col", new[] { 18, 19, 20 })]
        [TestCase("coll", new[] { 18, 19, 20 })]
        [TestCase("collo", new[] { 18, 19, 20 })]
        [TestCase("colloc", new[] { 18, 19, 20 })]
        [TestCase("colloca", new[] { 20 })]
        [TestCase("collocat", new[] { 20 })]
        [TestCase("collocati", new[] { 20 })]
        [TestCase("collocatio", new[] { 20 })]
        [TestCase("collocation", new[] { 20 })]
        [TestCase("collocationa", new[] { 20 })]
        [TestCase("collocationab", new[] { 20 })]
        [TestCase("collocationabl", new[] { 20 })]
        [TestCase("collocationable", new[] { 20 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("ca", new[] { 21, 22, 23 })]
        [TestCase("cap", new[] { 21, 22, 23 })]
        [TestCase("capo", new[] { 21, 22, 23 })]
        [TestCase("capom", new[] { 21 })]
        [TestCase("capomo", new[] { 21 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("ca", new[] { 21, 22, 23 })]
        [TestCase("cap", new[] { 21, 22, 23 })]
        [TestCase("capo", new[] { 21, 22, 23 })]
        [TestCase("capoc", new[] { 22, 23 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("ca", new[] { 21, 22, 23 })]
        [TestCase("cap", new[] { 21, 22, 23 })]
        [TestCase("capo", new[] { 21, 22, 23 })]
        [TestCase("capoc", new[] { 22, 23 })]
        [TestCase("u", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("un", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("ung", new[] { 24, 25, 26 })]
        [TestCase("ungi", new[] { 24, 25, 26 })]
        [TestCase("ungiv", new[] { 24, 25, 26 })]
        [TestCase("ungivi", new[] { 24 })]
        [TestCase("ungivin", new[] { 24 })]
        [TestCase("ungiving", new[] { 24 })]
        [TestCase("ungivingn", new[] { 24 })]
        [TestCase("ungivingne", new[] { 24 })]
        [TestCase("ungivingnes", new[] { 24 })]
        [TestCase("ungivingness", new[] { 24 })]
        [TestCase("u", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("un", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("ung", new[] { 24, 25, 26 })]
        [TestCase("ungi", new[] { 24, 25, 26 })]
        [TestCase("ungiv", new[] { 24, 25, 26 })]
        [TestCase("ungive", new[] { 25, 26 })]
        [TestCase("ungivea", new[] { 25 })]
        [TestCase("ungiveab", new[] { 25 })]
        [TestCase("ungiveabl", new[] { 25 })]
        [TestCase("ungiveable", new[] { 25 })]
        [TestCase("u", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("un", new[] { 9, 10, 11, 24, 25, 26 })]
        [TestCase("ung", new[] { 24, 25, 26 })]
        [TestCase("ungi", new[] { 24, 25, 26 })]
        [TestCase("ungiv", new[] { 24, 25, 26 })]
        [TestCase("ungive", new[] { 25, 26 })]
        [TestCase("p", new[] { 3, 4, 5, 27, 28, 29 })]
        [TestCase("pr", new[] { 27, 28, 29 })]
        [TestCase("pre", new[] { 27, 28, 29 })]
        [TestCase("pres", new[] { 27, 28, 29 })]
        [TestCase("prest", new[] { 27, 28, 29 })]
        [TestCase("presta", new[] { 27, 28, 29 })]
        [TestCase("prestan", new[] { 27, 28 })]
        [TestCase("prestand", new[] { 27, 28 })]
        [TestCase("prestanda", new[] { 27, 28 })]
        [TestCase("prestandar", new[] { 27, 28 })]
        [TestCase("prestandard", new[] { 27, 28 })]
        [TestCase("p", new[] { 3, 4, 5, 27, 28, 29 })]
        [TestCase("pr", new[] { 27, 28, 29 })]
        [TestCase("pre", new[] { 27, 28, 29 })]
        [TestCase("pres", new[] { 27, 28, 29 })]
        [TestCase("prest", new[] { 27, 28, 29 })]
        [TestCase("presta", new[] { 27, 28, 29 })]
        [TestCase("prestan", new[] { 27, 28 })]
        [TestCase("prestand", new[] { 27, 28 })]
        [TestCase("prestanda", new[] { 27, 28 })]
        [TestCase("prestandar", new[] { 27, 28 })]
        [TestCase("prestandard", new[] { 27, 28 })]
        [TestCase("p", new[] { 3, 4, 5, 27, 28, 29 })]
        [TestCase("pr", new[] { 27, 28, 29 })]
        [TestCase("pre", new[] { 27, 28, 29 })]
        [TestCase("pres", new[] { 27, 28, 29 })]
        [TestCase("prest", new[] { 27, 28, 29 })]
        [TestCase("presta", new[] { 27, 28, 29 })]
        [TestCase("prestab", new[] { 29 })]
        [TestCase("prestabi", new[] { 29 })]
        [TestCase("prestabil", new[] { 29 })]
        [TestCase("prestabili", new[] { 29 })]
        [TestCase("prestabilis", new[] { 29 })]
        [TestCase("prestabilism", new[] { 29 })]
        [TestCase("m", new[] { 30, 31, 32 })]
        [TestCase("me", new[] { 30, 31, 32 })]
        [TestCase("meg", new[] { 30, 31, 32 })]
        [TestCase("mega", new[] { 30, 31, 32 })]
        [TestCase("megal", new[] { 30, 31, 32 })]
        [TestCase("megalo", new[] { 30, 31, 32 })]
        [TestCase("megaloc", new[] { 30, 31, 32 })]
        [TestCase("megaloco", new[] { 30 })]
        [TestCase("megalocor", new[] { 30 })]
        [TestCase("megalocorn", new[] { 30 })]
        [TestCase("megalocorne", new[] { 30 })]
        [TestCase("megalocornea", new[] { 30 })]
        [TestCase("m", new[] { 30, 31, 32 })]
        [TestCase("me", new[] { 30, 31, 32 })]
        [TestCase("meg", new[] { 30, 31, 32 })]
        [TestCase("mega", new[] { 30, 31, 32 })]
        [TestCase("megal", new[] { 30, 31, 32 })]
        [TestCase("megalo", new[] { 30, 31, 32 })]
        [TestCase("megaloc", new[] { 30, 31, 32 })]
        [TestCase("megaloce", new[] { 31, 32 })]
        [TestCase("megalocep", new[] { 31, 32 })]
        [TestCase("megaloceph", new[] { 31, 32 })]
        [TestCase("megalocepha", new[] { 31, 32 })]
        [TestCase("megalocephal", new[] { 31, 32 })]
        [TestCase("megalocephali", new[] { 31, 32 })]
        [TestCase("megalocephalia", new[] { 31, 32 })]
        [TestCase("m", new[] { 30, 31, 32 })]
        [TestCase("me", new[] { 30, 31, 32 })]
        [TestCase("meg", new[] { 30, 31, 32 })]
        [TestCase("mega", new[] { 30, 31, 32 })]
        [TestCase("megal", new[] { 30, 31, 32 })]
        [TestCase("megalo", new[] { 30, 31, 32 })]
        [TestCase("megaloc", new[] { 30, 31, 32 })]
        [TestCase("megaloce", new[] { 31, 32 })]
        [TestCase("megalocep", new[] { 31, 32 })]
        [TestCase("megaloceph", new[] { 31, 32 })]
        [TestCase("megalocepha", new[] { 31, 32 })]
        [TestCase("megalocephal", new[] { 31, 32 })]
        [TestCase("megalocephali", new[] { 31, 32 })]
        [TestCase("megalocephalia", new[] { 31, 32 })]
        [TestCase("a", new[] { 6, 7, 8, 33, 34, 35 })]
        [TestCase("af", new[] { 33 })]
        [TestCase("afa", new[] { 33 })]
        [TestCase("afac", new[] { 33 })]
        [TestCase("aface", new[] { 33 })]
        [TestCase("afaced", new[] { 33 })]
        [TestCase("a", new[] { 6, 7, 8, 33, 34, 35 })]
        [TestCase("ae", new[] { 34, 35 })]
        [TestCase("aet", new[] { 34, 35 })]
        [TestCase("aett", new[] { 34 })]
        [TestCase("aette", new[] { 34 })]
        [TestCase("aettek", new[] { 34 })]
        [TestCase("aetteke", new[] { 34 })]
        [TestCase("aettekee", new[] { 34 })]
        [TestCase("aettekees", new[] { 34 })]
        [TestCase("a", new[] { 6, 7, 8, 33, 34, 35 })]
        [TestCase("ae", new[] { 34, 35 })]
        [TestCase("aet", new[] { 34, 35 })]
        [TestCase("aeti", new[] { 35 })]
        [TestCase("aetit", new[] { 35 })]
        [TestCase("aetite", new[] { 35 })]
        [TestCase("aetites", new[] { 35 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("co", new[] { 18, 19, 20, 36, 37, 38, 39 })]
        [TestCase("com", new[] { 36, 37, 38 })]
        [TestCase("como", new[] { 36, 37, 38 })]
        [TestCase("comol", new[] { 36 })]
        [TestCase("comole", new[] { 36 })]
        [TestCase("comolec", new[] { 36 })]
        [TestCase("comolecu", new[] { 36 })]
        [TestCase("comolecul", new[] { 36 })]
        [TestCase("comolecule", new[] { 36 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("co", new[] { 18, 19, 20, 36, 37, 38, 39 })]
        [TestCase("com", new[] { 36, 37, 38 })]
        [TestCase("como", new[] { 36, 37, 38 })]
        [TestCase("comod", new[] { 37, 38 })]
        [TestCase("comoda", new[] { 37, 38 })]
        [TestCase("comodat", new[] { 37, 38 })]
        [TestCase("comodato", new[] { 37, 38 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("co", new[] { 18, 19, 20, 36, 37, 38, 39 })]
        [TestCase("com", new[] { 36, 37, 38 })]
        [TestCase("como", new[] { 36, 37, 38 })]
        [TestCase("comod", new[] { 37, 38 })]
        [TestCase("comoda", new[] { 37, 38 })]
        [TestCase("comodat", new[] { 37, 38 })]
        [TestCase("comodato", new[] { 37, 38 })]
        [TestCase("c", new[] { 18, 19, 20, 21, 22, 23, 36, 37, 38, 39 })]
        [TestCase("co", new[] { 18, 19, 20, 36, 37, 38, 39 })]
        [TestCase("cog", new[] { 39 })]
        [TestCase("cogn", new[] { 39 })]
        [TestCase("cogno", new[] { 39 })]
        [TestCase("cognos", new[] { 39 })]
        [TestCase("cognosc", new[] { 39 })]
        [TestCase("cognosci", new[] { 39 })]
        [TestCase("cognoscib", new[] { 39 })]
        [TestCase("cognoscibi", new[] { 39 })]
        [TestCase("cognoscibil", new[] { 39 })]
        [TestCase("cognoscibili", new[] { 39 })]
        [TestCase("cognoscibilit", new[] { 39 })]
        [TestCase("cognoscibility", new[] { 39 })]
        public void Test(string query, IEnumerable<int> expected)
        {
            IEnumerable<int> actual = Trie.Retrieve(query);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void ExhaustiveAddTimeMeasurement()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ITrie<int> trie = CreateTrie();
            foreach (var phrase in Words40)
            {
                trie.Add(phrase, phrase.GetHashCode());
            }

            stopwatch.Stop();
            Console.WriteLine("Time: {0} milliseconds", stopwatch.Elapsed.TotalMilliseconds);
        }

        public string[] LongPhrases40 = new[] {
                                                  "enterfeatnanocephalousssanapaiteadullamitesorchidologiessphenomandibularunremandedmeechersevererszamaforegamelaundsconelikesphalmaphylloptosiselvishvyproverbiologistdouanesdispensationalismapotypetearsheetsmesodisilicicnoncorruptnessheliotroperjinnywinkunrecurrent",
                                                  "scourwayproimmunityunstonetutoriatepreauditorydeaconaldishwipinginstillatorpardaosdespondentnessbourreaunonponderositysubconicquiniblemowtnonrecitationticchenreburnishnonexecutionsforfaulterflaughteringmausolespermysallokinesisescribientesbauckiehomopauseoverinfluence",
                                                  "noncontinuableprostascokemantelautomaticallydarnixsnowslipssubgoalcaddopostencephalondilutelyunrulablekilanalmacenistaangustateembryoctonychunneredlingismsoverdignifiedsparsonsitesubaqueansupercatholicallysprecounselmicrodistillationdisrestorecondensednesspredirectpinipicrin",
                                                  "unpredaciousnesschiarooscurossalfinrectigradeliverberryimpallupanayanodiflorousreadjourningrillettesaxeassoilzieingfruitwomanendonuclearunadventurousnesserinlacworksbandaiteparchableryukyuannondeformedanomitevolvocaceousreawaretiburtineunviolinedcuselitesawman",
                                                  "octavianporencephaliamirabilisesagrypnodefichtelitestylionindusiformacanthonhandspansunglospectrobolometerminisedananimadversivenesstrilliaceousoveytogetherhoodschoolgirlismnonmelodramaticallyssidiondawneredsubsphericpleuropedaloverlockingsamoritedendroclimatologiessyinstantiliquoracataposis",
                                                  "overdelicatenesskischensizeablenessseptomaxillarychumpishbelanderfallageantipragmaticismagranulocyticmechanalnoneasternepimyocardialbegoredretreatingnessfourquinecavilingnessradiestheticpolymixiidmelanotekitecacurunweepinghechtsactinostlimburgitesnonsubmergibilitysawbwaadddaunattractablewitherweightbacteriopurpurin",
                                                  "concatervateinogenesissgyrographembiotocidaeinguinodyniasfemorisrewishnonballotingoxidoreductionradiomuscularnoncurtailingsentogenousunrefundingrotavatorsuneathsflexibiltyunconsiderableszinkificationsnorseleruninstructiblesecchjuckholluschickslabellate",
                                                  "banintraligamentousargononunresumptivefinancistcentrarchidfumisteryserragemonseignevrunfoolishnessmahajunleatherfishesunfattenunsoothingsethylthioethanesuperrespectablesconsolanparcimoniesadenousblinterpedagogerybinoosphradiumconformatorsanisylflambageacquaintant",
                                                  "voicebandhuamuchilarticulabilitygiornatatelightlyingdealkylatepostbulbarosteotrophydiscommissioningcalombapoyntingvectionmacrosomiaimpolarilymetapoliticbiaswisebeedgedparafloccularitcheoglansolepieceladylintywhiteuncoherentlycoprophilismpiaclekarachivoglitespeculativismupbboreinterequinoctialbubbleless",
                                                  "sesoenteritismormaorshipmislyunpromiscuouslybescribblinghypopetalypennyfeenonobscurityhayliftpietosoessentializationflappetapparailsanticoagulatorprenominicaldrymouthspolymetamericmonariobelonosphaeritedwaiblesnonconsumptivelypapaiabeforenesscorkmakermacrosplanchnicundiagrammaticallymaxostoma",
                                                  "lasiocampidprunetincoventrybrigandishsuperacidityinterlucatenilometerveilednesslivishlyimprecatorilyrustfulmucroniformavelongeassociatorscleisteschylocystperithelialcapellaneprisiadkahypothetistgonotocontoariotomyssamidoazoattemperatorphthirusvellenagetriticalnessesthylose",
                                                  "unexcoriatedunimpressionabilityradiotropicbaronizedunfalcatedunretractedbayheadcoracocostaldovefloweroverbravenotopteridquadrinomicalsubdolouslyexhalatesceleratesperiareumcondensedlyoverpuissantlyhumanitymongermanucodemontrossaprilsilicoferruginousnpfxcaumstonetrebletreehyenanchinextraregularlybecommasemipathologically",
                                                  "sherryvalliesnonmonarchallysovereasinessmesocoelesubgenitalsouserworsementchondrectomydestinismavidyalysosomallyuncircumlocutoryboardysugescentpimolasciosophistarretezconscientisationleatmensanthroposociologistphyllobranchiatelonelihoodinteressortortilclintonchuradaunsatiabilityantitemperance",
                                                  "palimpsetsubmontagnebicornutemucoflocculentsallactiteparagonimiasisdreckiersubtotemunnormalnesssupersensitisercorruptednessluskschangarinemendableanthropoclimatologycobaltocyanicssacalinenondeficientcarcasslessfrustulumboliviansprepsychotictidelessnessrhyotaxiticundermuslinscurtaxepanlogist",
                                                  "precultureploughjoggersbodilizephysiologueerethizontidaehistoplasminlanchowsstatutumamphophilnondeprivationelectromotionspanpolismretrorenalpentadecagonmuscosenessarmoraciahippocastanaceousrecessorndebelelayshipmagnetolysisunhandselledfraudlessnessreevasionllerphytochemicalsbefan",
                                                  "caddiingunludicroussdiscanderingfindyssheemraadnonglucosidalbuggesssscotographyinfoldmentunpredictivelymullerianphoenicochroitearcanenessestoxiinfectiousayacahuitecesserscadastrationleucocytolysesperistrumoustextiferousunbemoanedcolloxylindevauntpergelisolcounterinteresthala",
                                                  "odoriphoreunfacetiousnessstauropegiadermatolysissbodypaintsalligatoridaeuntimorouslyjimsonpaintproofeylupwrapsunresignedlyprealludeunvisiblybulbotubermultititularunverbosenessgastrolatrouseelwrackstemplarlikenesssmysophiliaurushicqurangrasswidowhoodcyanuricheathenriess",
                                                  "weeshnonmischievousnesscitronciruscungeboiorthopyramidsourjackunsortcompanionizingesthesiometricinshiningchrysopeeanacrogynaeundestroyableproletarizationnunciustephromyeliticmevingpresubstitutiondecipiumunmachineableapomecometrysacraryprecanonicaltuboovarianbemonstersreedeoophoromalaciaselectrographitewaltrot",
                                                  "spacinessesfarmholdpyrocollodiontalterlamentationalattendresscakersleyinginterirrigationtransdermicaddlementsunshameablydihydrogensclairsentientdesonationunpiouslypteropogonscalfhoodduodramaswoolulosekenogeneticavailmentendotropictrymsfeebleheartedbounceablys",
                                                  "dispendiouslypostfeministsunicursalityflaggellaparavaginitistroussheteroxenousawardmentequangularmycoplasmatacaearomanipugliaflavorsomenesshemiteriaungraphicboltelnonextensionautocombustiblerhizomatictruismaticsketchabilitiestrilinoleateindazolecanotierscombercaryopterisesflattyfluoratesinecureshipcolocasia",
                                                  "cubiconetechnopsychologyprewarrantspostcommissureimpersonatrixsunoriginativewhafaboutfetoplacentaltridecenetransmigrationistslimnographmescalismsitalianoctachronousimproficiencypreeffectmyotrophyvaleraldehydesapskullsubjectileoligoprotheticdollfacebedotehydroscopistexpressorstowsenonswearermisregulating",
                                                  "palpigerousarchimperialistgangesbitterlessfrankfortsaikuchisemivitalquinquiliteralodorometerbandboxicaloverquicklybedinmargarateacetlaunderopinionmassednessunfrettyruntgenizingcyanophycinshadelessnessplaymongerstyrolstrophanhinlipopexiainterclericalfordablenessmakeshiftinesssfootpaddery",
                                                  "blatherydisunifysnonforeignesspurpartsulphocarbamideoutferrethardockcoevolvedcoevolvesnondemonstrativenessnonreparationpetrolizedunvitrescentunneareduncentralcleronomyroadstonebritishismdispassionedsundescriptivenesshydrogalvanicautoplasmotherapyhoordingredocketingunwreckedenfoldennonbankableprimevityunetymologically",
                                                  "paraplastinovermotorglaikitnesseskingrowmesiolingualthackoorcrossbeakoutpraisedsenaitecryometryherschelbutsudanseparatoriesdiscoplacentalianpreinsulatechoristryprincipestrichophytiaundislodgeableextratabularpreemployercouadiaphanyeventognathousintercirculatingscribbliestcobblerlessantrinsubministrantrockish",
                                                  "outfledattaccopremadnessempiriologicaluneradicateduntautologicallygantonoleocystmstantiliberalistvasemakingcocklighthydroxydehydrocorticosteroneanoscopetartagostackhousiaceousparanuclearterminizepurplinessorepearchfouetsinterdistinguishpanarchyjnanendriyaepirogeneticlateroversionidicantiphthisicalambulancingregidor",
                                                  "pyroxylenechararasseparatedlyanachronismaticalpicroerythrinfaninmesostomidnondespoticallygilgameshrecompetitionteredinidaebuteonineisosterismtranshumanizegasboatnoctambulesuperplausiblycossyritelingtowpalaebiologistschronosemicglossoplegiahypoalkalineendoaortitislushiercreammakermonosemicestocadawoilienocardia",
                                                  "pamplegiaepikeianonperceptiblesimmeringlyhydrocaulussuresbykathaguitermanitelamnectomyoutmalaproppedhemiramphinebeneplacitnonsilicatewelkeremovelesscorrelativismwitchbroomrisslebirkiestshemitepandariccroatiaphotoepinasticmacrosplanchnicornationinsensingsorroanoncumbrousparatitlesmahdism",
                                                  "strouthiocameliancyanochroianonimpeachablesubtrochantericbudgypailettepaininglypindanonexemptionmonoplasmaticbiliprasinnoncelestialshithertolyleneenrobementastronsmyrnioteingeniosepihyalcytococciseignioralcondiddlementditremidundermanagerkidgierpootersbetrunkdeparliamentvidkids",
                                                  "aponogetonaceousunconsultativesvirificshrinkergchileansoutshovingrhombiformtricompoundcapotenpachangaigniformdespairfulnesssprintlinebetafitelethargicalnesssericiculturescrassilingualnonadjacenciessketoketenevellincherheterizeelectrocataphoreticarchaeohippusalsweillfouetsundrossinessreduviidae",
                                                  "unsmirkingnonamotionlaryngismalnonsynodicallyleysingdamaskinesmookspondilbishoplessbritishersnoldaraireslaccicprivilegerswangynonsingularitiesnoucheopisthographicalsubtransverselyweirdlessnessubermenschsrehypothecateincorporealizepractitioneryuninucleatedinvertibratesafenerrelayer",
                                                  "calamaroidfarcemeatsubsulfatecolluncoredeemerbeslaveredunshammedprocellosedarksummonocondyliangollanspolarogramprepedunclebakeoutabnegativeoctachlorideundejectedopsyprionacelacinulasmudfatsammyblackbushrifledomautoserotherapypandeanredecisionconfricamentumsomaticovisceral",
                                                  "extumescenceintwinementarenginternuncewoohoodiscodactylouseccoproticophorichindustanduskeroverpublicizingumbrettranshumanizehornslatebelozengednonannexationhousefurnishingsdinnerlysylvestralrefordsupercrimescrotectomyimperatesgriddlerspostallantoicnonsuspensiveresalutationmainpinbradyseisms",
                                                  "nonactualnessegiptononcategoricalnesslecturessdeanthropomorphicsoverperchfibrinokinasebeentosophisticativegleicheniaceaeophiodontidaegroomishbescribblingsprecommunicationcataphrenicprecogitatingparochialitiesinfranchisethebsesquisquarezamindarieshospitaangelshipunderpriestprimegiltsanctcrotonbughuddroun",
                                                  "pseudolunulanonlyricalnessscatchiebeerhallspostclaviculariguassuunloathlyunallusivelyovercontributionsirventunprofoundsemianunmammaliannonderogativecryptovolcanismantisudoralpleasablenesssquilliannucleohistonenondisparaginguncallusedcageylysephyrulasaumurelencticalcivilizadeintramorainicfrontstall",
                                                  "epithalamitemplelikebiforinsalmanazarsblepharoncosissprediscountablecummockacheuleanunbanneredfleyednessdecohesionshirtlessnessamexpentadecahydratedunearthliestadetautophotoelectrictarantulatedtenderishtinynessantiasthmaticsunretrogradingporokeratosistaprootedskeraunophobiatarrietrollflowers",
                                                  "jettinglydessignmentsnontravelerpalatiumglucocorticorduninthronedtertiiagaricalesswaptreunenonruminationthyreoiditispimanunderhangespadongheleemsbicyclismmethylidynehomoclinalrosalgercorrealgobacknontrademadreporaldioptographunderbrewmennoniteunconceitedlys",
                                                  "intersolublesubtowergorgoniaprejudiciousnesslerizationbahanprelexicalcopertarentrayeusepseudobranchathoniteyakshadutchingcajanusginkgoalesabudefduflaparohysterectomyantidiphtheriainquietnessnonpuebloprereceiverglossemicintergonialnonplatitudinouslyphosphoreousdisauncountermandedabbycircuminsularbinotic",
                                                  "alumnalscalyclinonmetamorphosishemisaprophyticrewarehousenonsupporterurbanakylcrysticpreburnunsuperlativeinsectiferoussoldatfirstersalagounprobatedcytoblastemousdowagerismlymphorrheasubarticlestntsidioglossiaspottledbackspeiringlovesomenessspongiosityantigonorrhealextracalicular",
                                                  "corrigesalintataophyllogenoussprisaltrivantrenettespecificativelypaedotrophiesmillibarnmelolonthineplenartiesctenodontumbelliferoneregraduateunorganicallypelagraembootaunadmirablyfingerfishesrearraysinistruousresolicitationforeweighscomodatograviersseptenniadtwitchfireethicosocial",
                                                  "forepolingsemifeudalismunhumannesschaungedadvolutionwinterboundunneedfulnessserenditecanangapetrolintocodynamometerdisquietednesslachrymaeformpostzygapophysisverminlikehydagedolosunannihilatorymurlackschamberwomansuperunityscnidoscoluswiwimoorillsuncalkgattinehargeisanemoricole"
                                              };

    }
}