// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Gma.DataStructures.StringSearch
{
    [Serializable]
    [DebuggerDisplay(
        "{m_Origin[m_StartIndex..m_EndIndex]}"
        )]
    public struct StringSlice<K> : IEnumerable<K> where K : IEquatable<K>
    {
        private readonly ReadOnlyMemory<K> m_Origin;
        private int m_StartIndex;
        private int m_EndIndex;

        public StringSlice(ReadOnlyMemory<K> origin)
            : this(origin, 0, origin.Length)
        {
        }

        public StringSlice(ReadOnlyMemory<K> origin, int startIndex)
            : this(origin, startIndex, origin.Length - startIndex)
        {
        }

        public StringSlice(ReadOnlyMemory<K> origin, int startIndex, int partitionLength)
        {
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex), "The value must be non negative.");
            if (partitionLength < 0)
                throw new ArgumentOutOfRangeException(nameof(partitionLength), "The value must be non negative.");
            m_Origin = origin;
            m_StartIndex = startIndex;
            m_EndIndex = Math.Min(origin.Length, startIndex + partitionLength);
        }

        public K this[int index]
        {
            get { return m_Origin.Span[m_StartIndex + index]; }
        }

        public int StartIndex {
            set {
                m_StartIndex = Math.Max(0, Math.Min(value, m_EndIndex));
            }
            get { return m_StartIndex; }
        }

        public int EndIndex {
            set {
                m_EndIndex = Math.Max(StartIndex, Math.Min(value, m_Origin.Length));
            }
            get { return m_EndIndex; }
        }

        public int Length
        {
            get { return m_EndIndex - m_StartIndex; }
        }

        #region IEnumerable<char> Members

        public IEnumerator<K> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public bool Equals(StringSlice<K> other)
        {
            return m_Origin.Equals(other.m_Origin) &&
                m_EndIndex == other.m_EndIndex &&
                m_StartIndex == other.m_StartIndex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is StringSlice<K> sli && Equals(sli);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = m_Origin.GetHashCode();
                hashCode = (hashCode*397) ^ m_EndIndex;
                hashCode = (hashCode*397) ^ m_StartIndex;
                return hashCode;
            }
        }

        public StringSlice<K> Slice(int startIndex) {
            return Slice(startIndex, Length - startIndex);
        }

        public StringSlice<K> Slice(int startIndex, int count) {
            return new StringSlice<K>(m_Origin, m_StartIndex + startIndex, Math.Min(count, Length - startIndex));
        }

        public bool StartsWith(StringSlice<K> other) {
            return StartsWith(other.AsSpan());
        }

        public bool StartsWith(ReadOnlySpan<K> other) {
            if (Length < other.Length) {
                return false;
            }

            for (int i = 0; i < other.Length; i++) {
                if (!this[i].Equals(other[i])) {
                    return false;
                }
            }
            return true;
        }

        public ReadOnlyMemory<K> AsMemory() {
            return m_Origin.Slice(m_StartIndex, Length);
        }

        public ReadOnlySpan<K> AsSpan() {
            return m_Origin.Span.Slice(m_StartIndex, Length);
        }

        public (StringSlice<K>, StringSlice<K>) Split(int splitAt)
        {
            var head = new StringSlice<K>(m_Origin, m_StartIndex, splitAt);
            var rest = new StringSlice<K>(m_Origin, m_StartIndex + splitAt, Length - splitAt);
            return (head, rest);
        }

        public static bool operator ==(StringSlice<K> left, StringSlice<K> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringSlice<K> left, StringSlice<K> right)
        {
            return !(left == right);
        }
    }
}