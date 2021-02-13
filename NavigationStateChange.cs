using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    struct NavigationStateChange
    {
        readonly int _hash;
        NavigationStateEvent _stateEvent;

        public NavigationStateChange(int hash, NavigationStateEvent stateEvent)
        {
            _hash = hash;
            _stateEvent = stateEvent;
        }

        public int FieldHash { get => _hash; }
        public NavigationStateEvent StateEvent { get => _stateEvent; set => _stateEvent = value; }
    }
}
