using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    struct ActionStateChange
    {
        readonly int _hash;
        private ConsoleKey _stateEvent;

        public ActionStateChange(int hash, ConsoleKey stateEvent)
        {
            _hash = hash;
            _stateEvent = stateEvent;
        }

        public int Hash => _hash;

        public ConsoleKey StateEvent { get => _stateEvent; set => _stateEvent = value; }
    }
}
