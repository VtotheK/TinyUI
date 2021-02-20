using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    class StateTransitionException : Exception
    {

        public StateTransitionException(){

    }
        public StateTransitionException(string fieldNameFrom, NavigationStateEvent stateEvent) : base
            ($"Invalid state transition from {fieldNameFrom} with StateEvent:{stateEvent.ToString()}.")
        {

        }


    }
}
