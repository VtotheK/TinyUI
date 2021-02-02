using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    interface IUIElement
    {
        public string FieldName { get; }
        public CursorPosition ElementPosition { get;  }
        public CursorPosition LabelPosition { get;  }
        public CursorPosition GetCursorPosition();
    }
}
