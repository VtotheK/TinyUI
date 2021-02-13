using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    class ButtonField : IUIElement
    {
        readonly string _fieldName;
        private CursorPosition _elementPosition;
        private CursorPosition _labelPosition;
        private string _label;

        public ButtonField(string fieldName, CursorPosition position, string label)
        {
            _elementPosition = position;
            _fieldName = fieldName;
            _label = label;
            _labelPosition = position;
        }

        public string FieldName => _fieldName;
        public CursorPosition ElementPosition => _elementPosition;
        public CursorPosition LabelPosition => _labelPosition;
        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                if (_labelPosition is null)
                {
                    _labelPosition = new CursorPosition(_elementPosition.Left, _elementPosition.Top - 1);
                }
            }
        }

        public CursorPosition GetCursorPosition()
        {
            return _elementPosition;
        }

        public void ChangeHighlight(ConsoleColor fgColor, ConsoleColor bgColor)
        {
            var curFgColor = Console.ForegroundColor;
            var curBgColor = Console.BackgroundColor;
            Console.SetCursorPosition(_elementPosition.Left,_elementPosition.Top);
            Console.ForegroundColor = fgColor;
            Console.BackgroundColor = bgColor;
            Console.Write(_label);
            Console.ForegroundColor = curBgColor;
            Console.ForegroundColor = curBgColor;
        }

        public void DrawField()
        {
            if (Label != null)
            {
                Console.SetCursorPosition(LabelPosition.Left, LabelPosition.Top);
                Console.Write(Label);
            }
        }
    }
}
