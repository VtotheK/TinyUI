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
        private IUIElement _currenteElementPosition;
        Dictionary<StateChange, IUIElement> stateTransitions = new Dictionary<StateChange, IUIElement>();
        readonly ConsoleColor _textColor;
        readonly ConsoleColor _errorTextColor;

        #region Constructors
        public WindowManager(ConsoleColor textColor, ConsoleColor errorTextColor)
        {
            _textColor = textColor;
            _errorTextColor = errorTextColor;
        }
        public WindowManager(InputField position, ConsoleColor textColor, ConsoleColor errorTextColor)
        {
            _currenteElementPosition = position;
            _textColor = textColor;
            _errorTextColor = errorTextColor;
        }
        #endregion

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
            IUIElement newPosition;
            if (!stateTransitions.TryGetValue(new StateChange(_currenteElementPosition.FieldName.GetHashCode(), stateEvent), out newPosition))
            {
                throw new StateTransitionException(_currenteElementPosition.FieldName,stateEvent);
            }
            _currenteElementPosition = newPosition;
            CursorPosition pos = _currenteElementPosition.GetCursorPosition();
            return pos;
        }

        //Deprecated
        /*public CursorPosition GetCurrentPosition(InputField field)
        {
            int top = field.Position.Top;
            int left = field.Position.Left + field.BufferLength;
            return new CursorPosition(left, top);
        }*/ 

        public void SetCursorToInputField(IUIElement element)
        {
            _currenteElementPosition = element;
            var pos = _currenteElementPosition.GetCursorPosition();
            Console.SetCursorPosition(pos.Left, pos.Top);
        }

        public IUIElement GetCurrentUIElement()
        {
            return _currenteElementPosition;
        }

        public void DrawInputFieldLabels()
        {
            foreach(var field in _allFields)
            {
                if(field.Label != null)
                Console.SetCursorPosition(field.LabelPosition.Left, field.LabelPosition.Top);
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
                    _currenteElementPosition = _allFields[i];
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

        public void DeleteCharacter()
        {
            if(_currenteElementPosition is InputField)
            {
                var field = (InputField) _currenteElementPosition;
                if(field.DeleteChar())
                {
                    Console.Write("\b \b");
                }
            }
        }

        public void AddCharacter(char c)
        {
            if (_currenteElementPosition is InputField)
            {
                var field = (InputField)_currenteElementPosition;
                if (field.AddChar(c))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(c);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public void PrintErrorMessage()
        {
            Console.SetCursorPosition(_errorInputField.ElementPosition.Left, _errorInputField.ElementPosition.Top);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            //Console.Write(e.Message);
            //window.SetCursorToInputField(fieldWhereError);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
        }

    }
}
