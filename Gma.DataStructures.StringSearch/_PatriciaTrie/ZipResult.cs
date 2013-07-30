// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System.Diagnostics;

namespace Gma.DataStructures.StringSearch
{
    [DebuggerDisplay("Head: '{CommonHead}', This: '{ThisRest}', Other: '{OtherRest}', Kind: {MatchKind}")]
    public struct ZipResult
    {
        private readonly StringPartition m_CommonHead;
        private readonly StringPartition m_OtherRest;
        private readonly StringPartition m_ThisRest;

        public ZipResult(StringPartition commonHead, StringPartition thisRest, StringPartition otherRest)
        {
            m_CommonHead = commonHead;
            m_ThisRest = thisRest;
            m_OtherRest = otherRest;
        }

        public MatchKind MatchKind
        {
            get
            {
                return m_ThisRest.Length == 0
                           ? (m_OtherRest.Length == 0
                                  ? MatchKind.ExactMatch
                                  : MatchKind.IsContained)
                           : (m_OtherRest.Length == 0
                                  ? MatchKind.Contains
                                  : MatchKind.Partial);
            }
        }

        public StringPartition OtherRest
        {
            get { return m_OtherRest; }
        }

        public StringPartition ThisRest
        {
            get { return m_ThisRest; }
        }

        public StringPartition CommonHead
        {
            get { return m_CommonHead; }
        }
    }
}