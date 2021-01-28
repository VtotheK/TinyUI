using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace StateMachine
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
        Next,
        Previous,
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
            _firstName = new InputField(10,10,15,typeof(string));
            _lastName = new InputField(30,10,15,typeof(string));
            _age = new InputField(50,10,15,typeof(int));
            _address = new InputField(55,10,15,typeof(string));
            CreateStateTransitions();
        }

        private void CreateStateTransitions()
        {
            stateTransitions.Add(new StateChange(CursorPosition.FirstName, StateEvent.Next), CursorPosition.LastName);
            stateTransitions.Add(new StateChange(CursorPosition.FirstName, StateEvent.Previous), CursorPosition.Address);
            stateTransitions.Add(new StateChange(CursorPosition.LastName, StateEvent.Next), CursorPosition.Age);
            stateTransitions.Add(new StateChange(CursorPosition.LastName, StateEvent.Previous), CursorPosition.FirstName);
            stateTransitions.Add(new StateChange(CursorPosition.Age, StateEvent.Next), CursorPosition.Address);
            stateTransitions.Add(new StateChange(CursorPosition.Age, StateEvent.Previous), CursorPosition.LastName);
            stateTransitions.Add(new StateChange(CursorPosition.Address, StateEvent.Next), CursorPosition.FirstName);
            stateTransitions.Add(new StateChange(CursorPosition.Address, StateEvent.Previous), CursorPosition.LastName);
        }

        public InputField ChangeState(StateEvent stateEvent)
        {
            CursorPosition newPosition;
            if(!stateTransitions.TryGetValue(new StateChange(_position,stateEvent),out newPosition)){
                throw new Exception("Awww shit, no transition for this state change!");
            }
            _position = newPosition;
            return GetInputField();
        }

        public InputField GetCurrentPosition()
        {
            return GetInputField();
        }

        private InputField GetInputField()
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
    class StateMachine
    {
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            WindowLayout();
            WindowState window = new WindowState(CursorPosition.FirstName);
            var pos = window.GetCurrentPosition();
            Console.SetCursorPosition(pos.Top, pos.Left);
            while (true)
            {
                var key = Console.ReadKey(true);
                
                switch (key.Key) {
                    case ConsoleKey.RightArrow:
                        {
                            var field = window.ChangeState(StateEvent.Next);
                            Console.SetCursorPosition(field.Top, field.Left);
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            var field = window.ChangeState(StateEvent.Previous);
                            Console.SetCursorPosition(field.Top, field.Left);
                            break;
                        }
                    case ConsoleKey.Backspace:
                        Console.Write("\b \b");
                        break;
                    default:
                        sb.Append(key.KeyChar);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(key.KeyChar);
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
                    
            }
        }

        static void WindowLayout()
        {
            Console.SetCursorPosition(10, 9);
            Console.Write("First name");
            Console.SetCursorPosition(30,9);
            Console.Write("Last name");
            Console.SetCursorPosition(50, 9);
            Console.Write("Age");
            Console.SetCursorPosition(55, 9);
            Console.Write("Address");
        }
    }
}


struct InputField
{
    readonly int _maxchars;
    readonly Type _type;
    readonly int _top;
    readonly int _left;
    public InputField(int top, int left,int maxchars, Type type)
    {
        _maxchars = maxchars ;
        _type = type;
        _left = left;
        _top = top;
    }

    public int Maxchars => _maxchars;
    public Type Type => _type;
    public int Top => _top;
    public int Left => _left;
}