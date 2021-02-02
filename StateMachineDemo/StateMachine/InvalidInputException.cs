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
        public InvalidInputException(InputField field) : base($"Virheellinen teksti kentässä:{field.Label}.")
        {

        }
    }
}
