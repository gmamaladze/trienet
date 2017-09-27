using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gma.DataStructures.StringSearch._Ukkonen;

namespace Gma.DataStructures.StringSearch._Ukkonen
{

/**
 * A specialized implementation of Map that uses native char types and sorted
 * arrays to keep minimize the memory footprint.
 * Implements only the operations that are needed within the suffix tree context.
 */
 class EdgeBag : Dictionary<char, Edge>
 {
  //TODO Optimize
 }
}