using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo.StateMachine
{
    enum CursorPosition
    {
        FirstName,
        LastName,
        Age,
        Address
    }

    enum StateEvent
    {
        Right,
        Left,
        Input
    }

    struct StateChange
    {
        CursorPosition _position;
        StateEvent _stateEvent;

        public StateChange(CursorPosition position, StateEvent stateEvent)
        {
            _position = position;
            _stateEvent = stateEvent;
        }

        public CursorPosition Position { get => _position; set => _position = value; }
        public StateEvent StateEvent { get => _stateEvent; set => _stateEvent = value; }
    }
    class WindowState
    {
        List<InputField> _allFields = new List<InputField>();
        private CursorPosition _position;
        private StateEvent _stateEvent;
        readonly InputField _firstName;
        readonly InputField _lastName;
        readonly InputField _age;
        readonly InputField _address;
        Dictionary<StateChange, CursorPosition> stateTransitions = new Dictionary<StateChange, CursorPosition>();
        public WindowState(CursorPosition position)
        {
            _position = position;
            _firstName = new InputField("FirstName",10, 10, 15, typeof(string));
            _lastName = new InputField("LastName",30, 10, 15, typeof(string));
            _age = new InputField("Age",50, 10, 3, typeof(int));
            _address = new InputField("Address",55, 10, 30, typeof(string));
            CreateStateTransitions();
        }

        private void CreateStateTransitions()
        {
            stateTransitions.Add(new StateChange(CursorPosition.FirstName, StateEvent.Right), CursorPosition.LastName);
            stateTransitions.Add(new StateChange(CursorPosition.FirstName, StateEvent.Left), CursorPosition.Address);
            stateTransitions.Add(new StateChange(CursorPosition.LastName, StateEvent.Right), CursorPosition.Age);
            stateTransitions.Add(new StateChange(CursorPosition.LastName, StateEvent.Left), CursorPosition.FirstName);
            stateTransitions.Add(new StateChange(CursorPosition.Age, StateEvent.Right), CursorPosition.Address);
            stateTransitions.Add(new StateChange(CursorPosition.Age, StateEvent.Left), CursorPosition.LastName);
            stateTransitions.Add(new StateChange(CursorPosition.Address, StateEvent.Right), CursorPosition.FirstName);
            stateTransitions.Add(new StateChange(CursorPosition.Address, StateEvent.Left), CursorPosition.Age);
        }

        public void ChangeState(StateEvent stateEvent)
        {
            CursorPosition newPosition;
            if (!stateTransitions.TryGetValue(new StateChange(_position, stateEvent), out newPosition))
            {
                throw new Exception("Awww shit, no transition for this state change!");
            }
            _position = newPosition;
        }

        public CursorCoordinate GetCurrentPosition()
        {
            var field = GetInputField();
            int top = field.Top;
            int left = field.Left + field.BufferLength();
            return new CursorCoordinate(left, top);
        }

        public InputField GetInputField()
        {
            switch (_position)
            {
                case CursorPosition.FirstName:
                    return _firstName;
                case CursorPosition.LastName:
                    return _lastName;
                case CursorPosition.Age:
                    return _age;
                case CursorPosition.Address:
                    return _address;
                default:
                    throw new Exception("No input field for this position, what in the fuck?");
            }
        }
    }
}
