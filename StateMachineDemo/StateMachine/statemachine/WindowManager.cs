using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
        private InputField _errorInputField;
        private InputField _position;
        Dictionary<StateChange, InputField> stateTransitions = new Dictionary<StateChange, InputField>();

        public WindowManager()
        {

        }
        public WindowManager(InputField position)
        {
            _position = position;
        }

        public InputField ErrorInputField { get => _errorInputField; set => _errorInputField = value; }

        public void CreateStateTransition(InputField fromField,StateEvent stateEvent, InputField toField)
        {
            stateTransitions.Add(new StateChange(fromField.FieldName.GetHashCode(), stateEvent), toField);
        }
        
        public InputField CreateInputField(string name, CursorPosition position, uint maxcharacters, InputType inputType, bool allowNullValues)
        {
            if(name == null || maxcharacters <= 0)
            {
                throw new ArgumentException("Trying to create input field with null parameters");
            }
            var field = new InputField(name, position.Left, position.Top, (int)maxcharacters, inputType, allowNullValues);
            _allFields.Add(field);
            return field;
        }


        public InputField CreateErrorMessageField(string name, CursorPosition position, uint maxcharacters, InputType inputType, bool allowNullValues)
        {
            if (name == null || maxcharacters <= 0)
            {
                throw new ArgumentException("Trying to create input field with wrong parameters");
            }
            var field = new InputField(name, position.Left, position.Top, (int)maxcharacters, inputType, allowNullValues);
            _errorInputField = field;
            return field;
        }
        public CursorPosition UserInput(StateEvent stateEvent)
        {
            InputField newPosition;
            if (!stateTransitions.TryGetValue(new StateChange(_position.FieldName.GetHashCode(), stateEvent), out newPosition))
            {
                throw new StateTransitionException(_position.FieldName,stateEvent);
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
            Console.SetCursorPosition(field.Position.Left + field.BufferLength, field.Position.Top);
        }

        public InputField GetInputField()
        {
            return _position;
        }

        public void DrawInputFieldLabels()
        {
            foreach(var field in _allFields)
            {
                if(field.Label != null)
                Console.SetCursorPosition(field.LabelCoordinate.Left, field.LabelCoordinate.Top);
                Console.Write(field.Label);
            }
        }

        public void ValidateInput()
        {
            for(int i = 0; i < _allFields.Count; ++i)
            {
                Debug.Write(_allFields[i].BufferLength+"\n");
                if(!_allFields[i].NullValues && _allFields[i].BufferLength <= 0)
                {
                    _position = _allFields[i];
                    throw new InvalidInputException(_allFields[i]);
                }
                else
                {
                    switch (_allFields[i].InputType)
                    {
                        case InputType.String:
                            if (Regex.Match(_allFields[i].Buffer, @"[0-9]@+$", RegexOptions.IgnoreCase).Success)
                            {
                                throw new InvalidInputException(_allFields[i]);
                            }
                            break;

                        case InputType.Integer:
                            int temp;
                            if(!Int32.TryParse(_allFields[i].Buffer,out temp))
                            {
                                throw new InvalidInputException(_allFields[i]);
                            }
                            break;

                        case InputType.UnsignedInteger:
                            uint utemp;
                            if(!UInt32.TryParse(_allFields[i].Buffer,out utemp))
                            {
                                throw new InvalidInputException(_allFields[i]);
                            }
                            break;
                        default:
                            Debug.Write("Invalid datatype defined");
                            break;

                    }
                }
            }
        }

        public void PrintErrorMessage()
        {
            Console.SetCursorPosition(_errorInputField.Position.Left, _errorInputField.Position.Top);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            //Console.Write(e.Message);
            //window.SetCursorToInputField(fieldWhereError);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
        }

    }
}
