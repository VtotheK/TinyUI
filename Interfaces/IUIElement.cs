using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    interface IUIElement
    {
        public ElementDecorators Decorators { get; set; }
        public string FieldName { get; }
        public CursorPosition ElementPosition { get;  }
        public CursorPosition LabelPosition { get;  }
        public CursorPosition GetCursorPosition();
        public void DrawField();
    }
}
