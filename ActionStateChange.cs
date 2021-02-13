using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    struct ActionStateChange
    {
        readonly int _hash;
        private ActionStateEvent _stateEvent;

        public ActionStateChange(int hash, ActionStateEvent stateEvent)
        {
            _hash = hash;
            _stateEvent = stateEvent;
        }

        public int Hash => _hash;

        public ActionStateEvent StateEvent { get => _stateEvent; set => _stateEvent = value; }
    }
}
