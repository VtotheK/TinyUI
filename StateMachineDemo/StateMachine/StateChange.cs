using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    struct StateChange
    {
        readonly int _hash;
        StateEvent _stateEvent;

        public StateChange(int hash, StateEvent stateEvent)
        {
            _hash = hash;
            _stateEvent = stateEvent;
        }

        public int FieldHash { get => _hash; }
        public StateEvent StateEvent { get => _stateEvent; set => _stateEvent = value; }
    }
}
