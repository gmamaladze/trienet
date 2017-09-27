using System;
using System.Collections.Generic;
using System.Text;

namespace Gma.DataStructures.StringSearch._Ukkonen
{

public class Utils {
    
    /**
     * Normalize an input string
     * 
     * @param in the input string to normalize
     * @return <tt>in</tt> all lower-case, without any non alphanumeric character
     */
    public static String normalize(String input) {
        StringBuilder output = new StringBuilder();
        String l = input.ToLower();
        for (int i = 0; i < l.Length; ++i) {
            char c = l[i];
            if (c >= 'a' && c <= 'z' || c >= '0' && c <= '9') {
                output.Append(c);
            }
        }
        return output.ToString();
    }

    /**
     * Computes the set of all the substrings contained within the <tt>str</tt>
     * 
     * It is fairly inefficient, but it is used just in tests ;)
     * @param str the string to compute substrings of
     * @return the set of all possible substrings of str
     */
    public static ISet<String> getSubstrings(String str) {
        ISet<String> ret = new HashSet<String>();
        // compute all substrings
        for (int len = 1; len <= str.Length; ++len) {
            for (int start = 0; start + len <= str.Length; ++start) {
                String itstr = str.Substring(start, start + len);
                ret.Add(itstr);
            }
        }

        return ret;
    }
}
}