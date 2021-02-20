using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    class InvalidInputException : Exception
    {
        string _fieldName;
        public InvalidInputException()
        {

        }
        public InvalidInputException(IUIElement field)
        {
            _fieldName = field.FieldName;
        }

        public string InputFieldName { get => _fieldName;}
    }
}
