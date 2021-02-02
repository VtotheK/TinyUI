using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    class ButtonField : IUIElement
    {
        public string FieldName => throw new NotImplementedException();

        public CursorPosition ElementPosition => throw new NotImplementedException();

        public CursorPosition LabelPosition => throw new NotImplementedException();

        public CursorPosition GetCursorPosition()
        {
            throw new NotImplementedException();
        }
    }
}
