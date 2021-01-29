using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    struct CursorCoordinate
    {
        private int _top;
        private int _left;

        public CursorCoordinate(int left, int top)
        {
            _top = top;
            _left = left;
        }

        public int Top { get => _top; set => _top = value; }
        public int Left { get => _left; set => _left = value; }
    }
}
