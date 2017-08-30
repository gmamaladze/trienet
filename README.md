# trienet
Migrated from https://trienet.codeplex.com/

# usage
```csharp
using Gma.DataStructures.StringSearch;
	
...

var trie = new SuffixTrie<int>(3);

trie.Add("hello", 1);
trie.Add("world", 2);
trie.Add("hell", 3);

var result = trie.Retrieve("hel");
```

# motivation
If you are implementing a modern user friendly peace of software you will very probably need something like this:

![](https://github.com/gmamaladze/trienet/master/img/trie-example.png?raw=true)

Or this:

![](https://github.com/gmamaladze/trienet/master/img/trie-example_2.png?raw=true)

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

## preformance

Important: all diagrams are given in logarithmic scale on x-axis.

To answer the question about when to use trie vs. linear search beter I’v experimeted with real data.
As you can see below using a trie data structure may already be reasonable after 10.000 records if you are expecting many queries on the same data set.

![](https://github.com/gmamaladze/trienet/master/img/trie-look-up1.png?raw=true)

Look-up times on patricia are slightly better, advantages of patricia bacame more noticable if you work with strings having many repeating parts, like quelified names of classes in sourcecode files, namespaces, variable names etc. So if you are indexing source code or something similar it makes sense to use patricia …

![](https://raw.githubusercontent.com/gmamaladze/trienet/master/img/trie-look-up2.png?raw=true)

… even if the build-up time of patricia is higher compared to the normal trie.

![](https://github.com/gmamaladze/trienet/blob/master/img/trie-build-up1.png?raw=true)

 

## demo app

The app demonstrates indexing of large text files and look-up inside them. I have experimented with huge texts containing millions of words. Indexing took usually only several seconds and the look-up delay was still unnoticable for the user.

![](https://github.com/gmamaladze/trienet/blob/master/img/trie-demo-app.png?raw=true)

