using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
<<<<<<< HEAD
    interface IUIElement
    {
        public ElementDecorators Decorators { get; set; }
        public string FieldName { get; }
        public CursorPosition ElementPosition { get;  }
        public CursorPosition LabelPosition { get;  }
        public CursorPosition GetCursorPosition();
        public void DrawField();
=======
    public interface IUIElement
    {
        ElementDecorators Decorators { get; set; }
        string FieldName { get; }
        CursorPosition ElementPosition { get;  }
        CursorPosition LabelPosition { get;  }
        CursorPosition GetCursorPosition();
        void DrawField();
>>>>>>> 778089a80e5031955f69c2c202d8cc174709dd34
    }
}
