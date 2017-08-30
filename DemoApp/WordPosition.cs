// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System.IO;

namespace Gma.DataStructures.StringSearch.DemoApp
{
    internal class WordPosition
    {
        private readonly long m_CharPosition;
        private readonly string m_FileName;

        public WordPosition(long charPosition, string fileName)
        {
            m_CharPosition = charPosition;
            m_FileName = fileName;
        }

        public string FileName
        {
            get { return m_FileName; }
        }

        public long CharPosition
        {
            get { return m_CharPosition; }
        }

        public override string ToString()
        {
            return
                string.Format(
                    "( Pos {0} ) {1}",
                    CharPosition,
                    Path.GetFileName(FileName));
        }
    }
}