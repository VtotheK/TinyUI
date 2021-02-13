using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TinyUI
{
    enum NavigationStateEvent
    {
        Right,
        Left,
        Up,
        Down
    }
    enum ActionStateEvent
    {
        Enter,
        Return,
        Esc
    }

    class WindowManager
    {
        List<InputField> _allInputFields = new List<InputField>();
        List<ButtonField> _allButtonFields = new List<ButtonField>();
        Dictionary<NavigationStateChange, IUIElement> navigationStateTransitions = new Dictionary<NavigationStateChange, IUIElement>();
        Dictionary<ActionStateChange, Action> _actionStateTransitions = new Dictionary<ActionStateChange, Action>();
        private InputField _errorInputField;
        private IUIElement _currentElementPosition;
        readonly ConsoleColor _inputTextColor;
        readonly ConsoleColor _errorTextColor;
        private ConsoleColor _buttonHighLightFgColor;
        private ConsoleColor _buttonHighLightBgColor;
        #region Constructors
        public WindowManager(ConsoleColor textColor, ConsoleColor errorTextColor)
        {
            _inputTextColor = textColor;
            _errorTextColor = errorTextColor;
        }
        public WindowManager(InputField position, ConsoleColor textColor, ConsoleColor errorTextColor)
        {
            _currentElementPosition = position;
            _inputTextColor = textColor;
            _errorTextColor = errorTextColor;
        }
        #endregion

        public InputField ErrorInputField { get => _errorInputField; set => _errorInputField = value; }

        public ConsoleColor ButtonHighLightFgColor { get => _buttonHighLightFgColor; set => _buttonHighLightFgColor = value; }

        public ConsoleColor ButtonHighLightBgColor { get => _buttonHighLightBgColor; set => _buttonHighLightBgColor = value; }

        public void CreateNavigationStateTransition(IUIElement fromField,NavigationStateEvent stateEvent, IUIElement toField, bool bothWays = false)
        {
            try
            {
                navigationStateTransitions.Add(new NavigationStateChange(fromField.FieldName.GetHashCode(), stateEvent), toField);
                if (bothWays)
                {
                    var oppositeEvent = GetOppositeNavigationStateEvent(stateEvent);
                    CreateNavigationStateTransition(toField, oppositeEvent, fromField, false);
                }
            }
            catch(ArgumentException e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine("Navigationtransition already exists");
                return;
            }
        }
        public void CreateActionStateTransition(IUIElement fromElement,ActionStateEvent stateEvent, Action toAction)
        {
           _actionStateTransitions.Add(new ActionStateChange(fromElement.FieldName.GetHashCode(), stateEvent), toAction);
        }
        
        public InputField CreateInputField(string name, CursorPosition position, uint maxcharacters, InputType inputType, bool allowNullValues)
        {
            if(name == null || maxcharacters <= 0)
            {
                throw new ArgumentException("Trying to create inputfield with null parameters");
            }
            var field = new InputField(name, position, (int)maxcharacters, inputType, allowNullValues);
            _allInputFields.Add(field);
            return field;
        }

        public ButtonField CreateButtonField(string name, CursorPosition position, string buttonLabel)
        {
            if (name == null || name == string.Empty)
            {
                throw new ArgumentException("Trying to create buttonfield with null name");
            }
            var field = new ButtonField(name, position, buttonLabel);
            _allButtonFields.Add(field);
            return field;
        }

        public InputField CreateErrorMessageField(string name, CursorPosition position, uint maxcharacters, InputType inputType, bool allowNullValues)
        {
            if (name == null || maxcharacters <= 0)
            {
                throw new ArgumentException("Trying to create input field with wrong parameters");
            }
            var field = new InputField(name, position, (int)maxcharacters, inputType, allowNullValues);
            _errorInputField = field;
            return field;
        }
        public CursorPosition NavigationInput(NavigationStateEvent stateEvent)
        {
            IUIElement newPosition;
            if (!navigationStateTransitions.TryGetValue(new NavigationStateChange(_currentElementPosition.FieldName.GetHashCode(), stateEvent), out newPosition))
            {
                return null;
            }
            else
            {
                if (_currentElementPosition is ButtonField)
                {
                    var f = _currentElementPosition as ButtonField;
                    f.ChangeHighlight(ConsoleColor.White, ConsoleColor.Black);
                }
                else
                {
                    Console.CursorVisible = true;
                }
                _currentElementPosition = newPosition;
                if(_currentElementPosition is ButtonField)
                {
                    var f = _currentElementPosition as ButtonField;
                    f.ChangeHighlight(ConsoleColor.Black, ConsoleColor.White);
                    Console.CursorVisible = false;
                }
                else
                {  
                    Console.CursorVisible = true;
                }
                CursorPosition pos = _currentElementPosition.GetCursorPosition();
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                return pos;
            }
        }

        public Action ActionInput(ActionStateEvent stateEvent)
        {
            Action act;
            if (!_actionStateTransitions.TryGetValue(new ActionStateChange(_currentElementPosition.FieldName.GetHashCode(), stateEvent), out act))
            {
                return null;
            }
            return act;
        }
        public void SetCursorToUIElement(IUIElement element)
        {
            if(_currentElementPosition != null && _currentElementPosition is ButtonField)
            {
                var f = _currentElementPosition as ButtonField;
                f.ChangeHighlight(ConsoleColor.White, ConsoleColor.Black);
            }
            _currentElementPosition = element;
            if(_currentElementPosition is ButtonField)
            {
                var f = _currentElementPosition as ButtonField;
                f.ChangeHighlight(ConsoleColor.Black, ConsoleColor.White);
                Console.CursorVisible = false;
            }
            var pos = _currentElementPosition.GetCursorPosition();
            Console.SetCursorPosition(pos.Left, pos.Top);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public IUIElement GetCurrentUIElement()
        {
            return _currentElementPosition;
        }

        public void DrawUIElements()
        {
            foreach(var field in _allInputFields)
            {
                field.DrawField();
            }
            foreach(var field in _allButtonFields)
            {
                field.DrawField();
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public Dictionary<string,string> GetDataFromInputFields()
        {
            try
            {
                ValidateInputFields();
                var ret = new Dictionary<string, string>();
                for(int i = 0; i < _allInputFields.Count; ++i)
                {
                    ret[_allInputFields[i].FieldName] = _allInputFields[i].BufferText; 
                }
                return ret;
            }
            catch(InvalidInputException e)
            {
                PrintErrorMessage(e.InputFieldName);
                return null;
            }
        }

        public void ValidateInputFields()
        {
            for(int i = 0; i < _allInputFields.Count; ++i)
            {
                if(!_allInputFields[i].ValidateField())
                {
                    throw new InvalidInputException(_allInputFields[i]);
                }
            }
        }
        public void DeleteCharacter()
        {
            if(_currentElementPosition is InputField)
            {
                var field = (InputField) _currentElementPosition;
                if(field.DeleteChar())
                {
                    Console.Write("\b \b");
                }
            }
        }

        public void AddCharacter(ConsoleKeyInfo c)
        {
            if (_currentElementPosition is InputField && c.Key != ConsoleKey.Tab)
            {
                var field = (InputField)_currentElementPosition;
                if (field.AddCharToBuffer(c.KeyChar))
                {
                    Console.ForegroundColor = _inputTextColor;
                    Console.Write(c.KeyChar);
                }
            }
        }

        public void PrintErrorMessage(string fieldName)
        {
            if (_errorInputField != null)
            {
                if(_errorInputField.BufferText.Length > 0)
                {
                    _errorInputField.EmptyBuffer(true);
                }
                var currentElement = GetCurrentUIElement();
                SetCursorToUIElement(_errorInputField);
                Console.ForegroundColor = _errorTextColor;
                Console.BackgroundColor = ConsoleColor.Black;
                string errorMessage = $"Väärä syöte kentässä: {fieldName}";
                Console.Write(errorMessage);
                _errorInputField.SetBuffer(errorMessage);
                Console.ForegroundColor = _inputTextColor;
                Console.BackgroundColor = ConsoleColor.Black;
                SetCursorToUIElement(currentElement);
            }
        }

        private NavigationStateEvent GetOppositeNavigationStateEvent(NavigationStateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case NavigationStateEvent.Left:
                    return NavigationStateEvent.Right;
                case NavigationStateEvent.Right:
                    return NavigationStateEvent.Left;
                case NavigationStateEvent.Up:
                    return NavigationStateEvent.Down;
                case NavigationStateEvent.Down:
                    return NavigationStateEvent.Up;
                default:
                    throw new ArgumentException("Unknown NavigationStateEvent.");
            }
        }
    }
}
