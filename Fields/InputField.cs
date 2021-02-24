using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    public enum InputType
    {
        Integer,
        UnsignedInteger,
        String,
        StringNoNumbers,
        StringNoSpecialCharacters,
        StringNoNumbersNoSpecialCharacters,
        Char,
        Float
    }

    public enum LabelPosition
    {
        Top,
        Left,
        Bottom,
        Right
    }

    public class InputField : IUIElement
    {
        private string _label;
        private CursorPosition _labelPosition;
        private CursorPosition _elementPosition;
        readonly string _fieldName;
        readonly int _maxchars;
        readonly InputType _inputType;
        readonly bool _nullValues;
        private StringBuilder _buffer;
        private ElementDecorators _decorators;
        public InputField(string fieldName, CursorPosition position, int maxchars, InputType type, bool nullValues)
        {
            _nullValues = nullValues;
            _fieldName = fieldName;
            _maxchars = maxchars;
            _inputType = type;
            _elementPosition = position;
            _buffer = new StringBuilder(maxchars);
        }

        public InputField(string fieldName,string label, CursorPosition position, int maxchars, InputType type, bool nullValues)
        {
            _nullValues = nullValues;
            _fieldName = fieldName;
            _maxchars = maxchars;
            _inputType = type;
            _elementPosition = position;
            _buffer = new StringBuilder(maxchars);
            Label = label; //need to set label cursorposition as well
        }


        public int BufferLength => _buffer.Length;
        public int Maxchars => _maxchars;
        public InputType InputType => _inputType;
        public string FieldName => _fieldName;
        public CursorPosition ElementPosition => _elementPosition;
        public CursorPosition LabelPosition => _labelPosition;
        public string BufferText { get => _buffer.ToString();}
        public string Label { get => _label;
            set {
                _label = value;
                if (_labelPosition is null)
                {
                    _labelPosition = new CursorPosition(_elementPosition.Left, _elementPosition.Top - 1);
                }
            } 
        }


        public bool NullValues => _nullValues;

        public ElementDecorators Decorators { get => _decorators;
            set { 
                _decorators = value;
                if (_decorators.TopDecorator != null && LabelPosition.Top - 1 >= 0)
                {
                    int top = LabelPosition.Top - 1;
                    int left = LabelPosition.Left;
                    _labelPosition = new CursorPosition(left, top);
                }
            }
        }

        public bool AddCharToBuffer(char character)
        {
            if (_buffer.Length < _maxchars)
            {
                _buffer.Append(character);
                return true;
            }
            return false;
        }

        public bool DeleteChar()
        {
            if (_buffer.Length > 0)
            {
                _buffer.Remove(_buffer.Length - 1, 1);
                return true;
            }
            return false;
        }

        public void EmptyBuffer(bool deleteVisuals)
        {
            if (deleteVisuals)
            {
                var cur = GetCursorPosition();
                int left = cur.Left;
                int top = cur.Top;
                for(int i = 0; i < _buffer.Length; ++i, --left)
                {
                    Console.SetCursorPosition(left, top);
                    Console.Write("\b \b");
                }
            }
            _buffer.Clear();
        }

        public CursorPosition GetCursorPosition()
        {
            return new CursorPosition(ElementPosition.Left + BufferText.Length, ElementPosition.Top);
        }

        public void SetBuffer(string s)
        {
            _buffer.Clear();
            _buffer.Append(s);
        }

        public bool ValidateField()
        {

            if (!NullValues && BufferLength <= 0)
            {
                return false;
            }
            else if(NullValues && BufferLength == 0)
            {
                return true;
            }

            else if(BufferLength > Maxchars)
            {
                return false;
            }

            else
            {
                string pattern = @"[!\#£¤$%€&/{(\[)\]=}?\\´`+´|_:^¨~*'"+'"'+"<>@§½]";
                switch (InputType)
                {
                    case InputType.StringNoNumbers:
                        if (Regex.Match(BufferText, "[0-9]", RegexOptions.IgnoreCase).Success)
                        {
                            return false;
                        }
                        return true;

                    case InputType.StringNoNumbersNoSpecialCharacters:
                        
                        if (Regex.Match(BufferText, "[0-9]", RegexOptions.IgnoreCase).Success
                            || Regex.Match(BufferText, pattern, RegexOptions.IgnoreCase).Success)
                        {
                            return false;
                        }
                        return true;
                    case InputType.Integer:
                        if (Regex.Match(BufferText,"[a-zA-Z äÄåÅöÖ]",RegexOptions.IgnoreCase).Success
                            || Regex.Match(BufferText,pattern,RegexOptions.IgnoreCase).Success)
                        {
                            return false;
                        }
                        return true;

                    case InputType.UnsignedInteger:

                        if (Regex.Match(BufferText, "[a-zA-Z äÄåÅöÖ]", RegexOptions.IgnoreCase).Success
                            || Regex.Match(BufferText, pattern, RegexOptions.IgnoreCase).Success
                            || BufferText[0] == '-')
                        {
                            return false;
                        }
                        return true;
                    case InputType.Char:
                        {
                            return BufferText.Length != 1;
                        }
                    default:
                        return false;
                }
            }
        }

        public void DrawField()
        {
            if (Label != null)
            {
                int mid = 0;
                /*
                if(1==2)
                {
                    mid = (Maxchars / 2) - (Label.Length / 2)-1; TODO Figure out how this could work
                }
                */
                Console.SetCursorPosition(LabelPosition.Left+mid, LabelPosition.Top);
                Console.Write(Label);
            }
            if(_decorators != null)
            {
                int topDecorLen = Maxchars;
                int left = ElementPosition.Left;
                if (_decorators.LeftDecorator != null && ElementPosition.Left - 1 >= 0)
                {
                    Console.SetCursorPosition(ElementPosition.Left - 1, ElementPosition.Top);
                    Console.Write(_decorators.LeftDecorator);
                }

                if(_decorators.RightDecorator != null && ElementPosition.Left + topDecorLen <= Console.WindowWidth)
                {
                    Console.SetCursorPosition(ElementPosition.Left + Maxchars, ElementPosition.Top);
                    Console.Write(_decorators.RightDecorator);
                }

                if(_decorators.TopDecorator != null && ElementPosition.Top - 1 > 0 && ElementPosition.Left + topDecorLen <= Console.WindowWidth)
                {
                    string decorStr = new String((char)_decorators.TopDecorator, topDecorLen);
                    Console.SetCursorPosition(left, ElementPosition.Top - 1);
                    Console.Write(decorStr);
                }

                if(_decorators.BottomDecorator != null && ElementPosition.Top + 1 <= Console.WindowHeight && ElementPosition.Left + topDecorLen <= Console.WindowWidth)
                {
                    string decorStr = new String((char)_decorators.BottomDecorator, topDecorLen);
                    Console.SetCursorPosition(left, ElementPosition.Top + 1);
                    Console.Write(decorStr);
                }
            }
        }
    }
}
