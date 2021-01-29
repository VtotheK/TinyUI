using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    public struct InputField
    {
        readonly string _fieldName;
        readonly int _maxchars;
        readonly Type _type;
        readonly int _top;
        readonly int _left;
        private StringBuilder _buffer;
        public InputField(string fieldName, int left, int top, int maxchars, Type type)
        {
            _fieldName = fieldName;
            _maxchars = maxchars;
            _type = type;
            _left = left;
            _top = top;
            _buffer = new StringBuilder(maxchars);
        }
        public int BufferLength()
        {
            return _buffer.Length;
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

        public int Maxchars => _maxchars;
        public Type Type => _type;
        public int Top => _top;
        public int Left => _left;

        public string FieldName => _fieldName;
    }
}
