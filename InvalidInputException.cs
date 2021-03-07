using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    class InvalidInputException : Exception
    {
        string _fieldName;
        IUIElement _field;
        public InvalidInputException()
        {

        }
        public InvalidInputException(IUIElement field)
        {
            _fieldName = field.FieldName;
            _field = field;
        }

        public string InputFieldName { get => _fieldName;}
        public IUIElement InputField{ get => _field;}
    }
}
