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
    }
}