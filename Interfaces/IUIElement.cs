using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{

    public interface IUIElement
    {
        ElementDecorators Decorators { get; set; }
        string FieldName { get; }
        CursorPosition ElementPosition { get;  }
        CursorPosition LabelPosition { get;  }
        Action ElementAction { get; set; }    //TODO after project is over, change to internal set and upgrade this project to c#8.0 or beyond    
        CursorPosition GetCursorPosition();
        void DrawField();
    }
}
