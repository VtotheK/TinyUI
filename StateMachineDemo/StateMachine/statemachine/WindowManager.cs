using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo
{
    enum StateEvent
    {
        Right,
        Left,
        Up,
        Down,
        Return,
        Enter,
        Input
    }

    class WindowManager
    {
        List<InputField> _allFields = new List<InputField>();
        private InputField _position;
        Dictionary<StateChange, InputField> stateTransitions = new Dictionary<StateChange, InputField>();

        public WindowManager()
        {

        }
        public WindowManager(InputField position)
        {
            _position = position;
        }

        public void CreateStateTransitions(InputField fromField,StateEvent stateEvent, InputField toField)
        {
            stateTransitions.Add(new StateChange(fromField.FieldName.GetHashCode(), stateEvent), toField);
        }
        
        public InputField CreateInputField(string name, CursorPosition position, uint maxcharacters, Type inputType, bool allowNullValues)
        {
            if(name == null || maxcharacters <= 0 || inputType == null)
            {
                throw new ArgumentException("Trying to create input field with null parameters");
            }
            var field = new InputField(name, position.Left, position.Top, (int)maxcharacters, inputType, allowNullValues);
            _allFields.Add(field);
            return field;
        }

        public CursorPosition UserInput(StateEvent stateEvent)
        {
            InputField newPosition;
            if (!stateTransitions.TryGetValue(new StateChange(_position.FieldName.GetHashCode(), stateEvent), out newPosition))
            {
                throw new Exception("Awww shit, no transition for this state change!");
            }
            _position = newPosition;
            CursorPosition pos = GetCurrentPosition(_position);
            return pos;
        }

        public CursorPosition GetCurrentPosition(InputField field)
        {
            int top = field.Position.Top;
            int left = field.Position.Left + field.BufferLength;
            return new CursorPosition(left, top);
        }

        public void SetCursorToInputField(InputField field)
        {
            _position = field;
            Console.SetCursorPosition(field.Position.Left, field.Position.Top);
        }

        public InputField GetInputField()
        {
            return _position;
        }

        public void DrawInputFieldLabels()
        {
            foreach(var field in _allFields)
            {
                Console.SetCursorPosition(field.LabelCoordinate.Left, field.LabelCoordinate.Top);
                Console.Write(field.Label);
            }
        }

        public void ValidateInput()
        {
            for(int i = 0; i < _allFields.Count; ++i)
            {
                Debug.Write(_allFields[i].BufferLength+"\n");
                if(_allFields[i].NullValues && _allFields[i].BufferLength <= 0)
                {
                    throw new Exception("SHIIIT");
                }
            }
        }

    }
}
