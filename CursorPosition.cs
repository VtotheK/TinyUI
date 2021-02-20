using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    public class CursorPosition
    {
        private int _top;
        private int _left;

        public CursorPosition(int left, int top)
        {
            _top = top;
            _left = left;
        }

        public int Top { get => _top; set => _top = value; }
        public int Left { get => _left; set => _left = value; }
    }
}
