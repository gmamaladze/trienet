// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

namespace Gma.DataStructures.StringSearch
{
    public struct SplitResult
    {
        private readonly StringPartition m_Head;
        private readonly StringPartition m_Rest;

        public SplitResult(StringPartition head, StringPartition rest)
        {
            m_Head = head;
            m_Rest = rest;
        }

        public StringPartition Rest
        {
            get { return m_Rest; }
        }

        public StringPartition Head
        {
            get { return m_Head; }
        }

        public bool Equals(SplitResult other)
        {
            return m_Head == other.m_Head && m_Rest == other.m_Rest;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SplitResult && Equals((SplitResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (m_Head.GetHashCode()*397) ^ m_Rest.GetHashCode();
            }
        }

        public static bool operator ==(SplitResult left, SplitResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SplitResult left, SplitResult right)
        {
            return !(left == right);
        }
    }
}