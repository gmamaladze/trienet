[![Build status](https://ci.appveyor.com/api/projects/status/4ruj2ijq2uc0pu2m/branch/master?svg=true)](https://ci.appveyor.com/project/gmamaladze/trienet/branch/master) [![NuGet version](https://badge.fury.io/nu/TrieNet.svg)](https://badge.fury.io/nu/TrieNet)

![TrieNet - The library provides .NET Data Structures for Prefix String Search and Substring (Infix) Search to Implement Auto-completion and Intelli-sense.](/img/trienet.png)

# usage

<pre>
  nuget install TrieNet
</pre>


```csharp
using Gma.DataStructures.StringSearch;
	
...

var trie = new UkkonenTrie<int>(3);
//var trie = new SuffixTrie<int>(3);

trie.Add("hello", 1);
trie.Add("world", 2);
trie.Add("hell", 3);

var result = trie.Retrieve("hel");
```

# updates

Added `UkkonenTrie<T>` which is a trie implementation using [Ukkonen's algorithm](https://en.wikipedia.org/wiki/Ukkonen%27s_algorithm).
Finally I managed to port (largely rewritten) a java implementation of [Generalized Suffix Tree using Ukkonen's algorithm](https://github.com/abahgat/suffixtree) by [Alessandro Bahgat](https://github.com/abahgat) (THANKS!). 

I have not made all measurements yet, but it occurs to have significatly imroved build-up and look-up times. 

# trienet

you liked it, you find it useful

![](/img/reviews.png)

so I migrated it from dying https://trienet.codeplex.com/ 

<pre>
  nuget install TrieNet
</pre>

and created a [NuGet package](https://www.nuget.org/packages/TrieNet/).


# motivation
If you are implementing a modern user friendly peace of software you will very probably need something like this:

![](/img/trie-example.png)

Or this:

![](/img/trie-example_2.png)

I have seen manyquestions about an efficient way of implementing a (prefix or infix) search over a key value pairs where keys are strings (for instance see:http://stackoverflow.com/questions/10472881/search-liststring-for-string-startswith).

So it depends:

* If your data source is aSQL or some other indexed database holdig your data it makes sense to utilize it’s search capabilities and issue a query to find maching records.

* If you have a small ammount of data, a linear scan will be probably the most efficient.

 
```csharp
IEnumerable> keyValuePairs;
...
var result = keyValuePairs.Select(pair => pair.Key.Contains(searchString));
``` 
 

* If you are seraching in a large set of key value records you may need a special data structure to perform your seach efficiently.


# trie

There is a family of data structures reffered as Trie. In this post I want to focus on a c# implementations and usage of Trie data structures. If you want to find out more about the theory behind the data structure itself Google will be probably your best friend. In fact most of popular books on data structures and algorithms describe tries (see.: Advanced Data Structures by Peter Brass)

## implementation

The only working .NET implementation I found so far was this one:http://geekyisawesome.blogspot.de/2010/07/c-trie.html

Having some concerns about interface usability, implementation details and performance I have decided to implement it from scratch.

My small library contains a bunch of trie data structures all having the same interface:


```csharp
public interface ITrie
{
  IEnumerable Retrieve(string query);
  void Add(string key, TValue value);
}
```

Class|Description  
-----|-------------
`Trie` | the simple trie, allows only prefix search, like `.Where(s => s.StartsWith(searchString))`
`SuffixTrie` | allows also infix search, like `.Where(s => s.Contains(searchString))`
`PatriciaTrie` | compressed trie, more compact, a bit more efficient during look-up, but a quite slower durig build-up.
`SuffixPatriciaTrie` | the same as PatriciaTrie, also enabling infix search.
`ParallelTrie` | very primitively implemented parallel data structure which allows adding data and retriving results from different threads simultaneusly.

## performance

Important: all diagrams are given in logarithmic scale on x-axis.

To answer the question about when to use trie vs. linear search beter I’v experimeted with real data.
As you can see below using a trie data structure may already be reasonable after 10.000 records if you are expecting many queries on the same data set.

![](/img/trie-look-up1.png)

Look-up times on patricia are slightly better, advantages of patricia bacame more noticable if you work with strings having many repeating parts, like quelified names of classes in sourcecode files, namespaces, variable names etc. So if you are indexing source code or something similar it makes sense to use patricia …

![](/img/trie-look-up2.png)

… even if the build-up time of patricia is higher compared to the normal trie.

![](/img/trie-build-up1.png)

 

## demo app

The app demonstrates indexing of large text files and look-up inside them. I have experimented with huge texts containing millions of words. Indexing took usually only several seconds and the look-up delay was still unnoticable for the user.

![](/img/trie-demo-app.png)

