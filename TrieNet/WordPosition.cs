// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php

namespace Gma.DataStructures.StringSearch {
    public struct WordPosition<T> {
        private readonly int m_CharPosition;
        private readonly T m_Value;

        public WordPosition(int charPosition, T value) {
            m_CharPosition = charPosition;
            m_Value = value;
        }

        public T Value {
            get { return m_Value; }
        }

        public int CharPosition {
            get { return m_CharPosition; }
        }

        public override string ToString() {
            return
                string.Format(
                    "( Pos {0} ) {1}",
                    CharPosition,
                    Value);
        }
    }
}