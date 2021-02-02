using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    public enum InputType
    {
        Integer,
        UnsignedInteger,
        String
    }

    public class InputField : IUIElement
    {
        private string label;
        private CursorPosition _labelPosition;
        private CursorPosition _elementposition;
        readonly string _fieldName;
        readonly int _maxchars;
        readonly InputType _inputType;
        readonly bool _nullValues;
        private StringBuilder _buffer;

        public InputField(string fieldName, int left, int top, int maxchars, InputType type,bool nullValues)
        {
            _nullValues = nullValues;
            _fieldName = fieldName;
            _maxchars = maxchars;
            _inputType = type;
            _elementposition = new CursorPosition(left, top);
            _buffer = new StringBuilder(maxchars);
        }

        public int BufferLength => _buffer.Length;
        public int Maxchars => _maxchars;
        public InputType InputType => _inputType;
        public string FieldName => _fieldName;
        public CursorPosition ElementPosition => _elementposition;
        public CursorPosition LabelPosition => _labelPosition;
        public string BufferText { get => _buffer.ToString();}
        public string Label { get => label;
            set {
                label = value;
                if (_labelPosition is null)
                {
                    _labelPosition = new CursorPosition(_elementposition.Left, _elementposition.Top - 1);
                }
            } }

        public bool NullValues => _nullValues;

        public bool AddChar(char character)
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
                foreach(char c in BufferText)
                {
                    Console.SetCursorPosition(left, top);
                    Console.Write("\b \b");
                    --left;
                }
                _buffer.Clear();
            }
        }

        public CursorPosition GetCursorPosition()
        {
            return new CursorPosition(ElementPosition.Left + BufferText.Length, ElementPosition.Top);
        }

        public void OverrideBuffer(string s)
        {
            _buffer.Clear();
            _buffer.Append(s);
        }
    }
}
