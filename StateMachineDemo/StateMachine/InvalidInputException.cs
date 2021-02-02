using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
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

        public string FieldName { get => _fieldName;}
    }
}
