using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    class InvalidInputException : Exception
    {
        public InvalidInputException()
        {

        }
        public InvalidInputException(string fieldName) : base($"Invalid input at {fieldName}.")
        {

        }
    }
}
