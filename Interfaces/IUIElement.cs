using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    interface IUIElement
    {
        ElementDecorators Decorators { get; set; }
        string FieldName { get; }
        CursorPosition ElementPosition { get;  }
        CursorPosition LabelPosition { get;  }
        CursorPosition GetCursorPosition();
        void DrawField();
    }
}
