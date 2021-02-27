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
<<<<<<< HEAD
=======
        Action ElementAction { get; set; }    //TODO after project is over, change to internal set and upgrade this project to c#8.0 or beyond    
>>>>>>> 29c1da967679369a91b8bcff3d2fecea64af9a96
        CursorPosition GetCursorPosition();
        void DrawField();
    }
}
