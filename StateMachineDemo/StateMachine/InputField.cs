using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    public class InputField
    {
        private string label;
        private CursorPosition _labelCoordinate;
        private CursorPosition _position;
        readonly string _fieldName;
        readonly int _maxchars;
        readonly Type _type;
        readonly bool _nullValues;
        private StringBuilder _buffer;

        public InputField(string fieldName, int left, int top, int maxchars, Type type,bool nullValues)
        {
            _nullValues = nullValues;
            _fieldName = fieldName;
            _maxchars = maxchars;
            _type = type;
            _position = new CursorPosition(left, top);
            _buffer = new StringBuilder(maxchars);
        }
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
        public int BufferLength => _buffer.Length;
        public int Maxchars => _maxchars;
        public Type Type => _type;
        public string FieldName => _fieldName;
        public CursorPosition Position => _position;
        public CursorPosition LabelCoordinate => _labelCoordinate;
        public string Label { get => label;
            set {
                label = value;
                if (_labelCoordinate is null)
                {
                    _labelCoordinate = new CursorPosition(_position.Left, _position.Top - 1);
                }
            } }

        public bool NullValues => _nullValues;
    }
}
