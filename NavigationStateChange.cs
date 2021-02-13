using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    struct NavigationStateChange
    {
        readonly int _hash;
        ConsoleKey _stateEvent;

        public NavigationStateChange(int hash, ConsoleKey key)
        {
            _hash = hash;
            _stateEvent = key;
        }

        public int FieldHash { get => _hash; }
        public ConsoleKey StateEvent { get => _stateEvent; set => _stateEvent = value; }
    }
}
