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

    public class WindowManager
    {
        List<InputField> _allInputFields = new List<InputField>();
        List<ButtonField> _allButtonFields = new List<ButtonField>();
        Dictionary<NavigationStateChange, IUIElement> navigationStateTransitions = new Dictionary<NavigationStateChange, IUIElement>();
        Dictionary<ActionStateChange, Action> _actionStateTransitions = new Dictionary<ActionStateChange, Action>();
        InputField _errorInputField;
        IUIElement _currentElementPosition;
        readonly ConsoleColor _inputTextColor;
        readonly ConsoleColor _errorTextColor;
        ConsoleColor _buttonHighLightFgColor;
        ConsoleColor _buttonHighLightBgColor;
        string _errorMessageBody;
        bool _terminated = false;
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
        public string ErrorMessageBody { get => _errorMessageBody; set => _errorMessageBody = value; }

        public void CreateNavigationStateTransition(IUIElement fromField,ConsoleKey key, IUIElement toField, bool bothWays = false)
        {
            if(bothWays && key != ConsoleKey.UpArrow && key != ConsoleKey.DownArrow && key != ConsoleKey.LeftArrow && key != ConsoleKey.RightArrow)
            {
                bothWays = false;
            }
            try
            {
                navigationStateTransitions.Add(new NavigationStateChange(fromField.FieldName.GetHashCode(), key), toField);
                if (bothWays)
                {
                    var oppositeEvent = GetOppositeNavigationStateEvent(key);
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
        public void CreateActionStateTransition(IUIElement fromElement, ConsoleKey stateEvent, Action toAction)
        {
            _actionStateTransitions.Add(new ActionStateChange(fromElement.FieldName.GetHashCode(), stateEvent), toAction);
            fromElement.ElementAction = toAction;
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
        public CursorPosition NavigationInput(ConsoleKey key)
        {
            IUIElement newPosition;
            if (!navigationStateTransitions.TryGetValue(new NavigationStateChange(_currentElementPosition.FieldName.GetHashCode(), key), out newPosition))
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

        private Action ActionInput(ConsoleKey stateEvent)
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
            else
            {
                Console.CursorVisible = true;
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
                _errorInputField.EmptyBuffer(true);
                for(int j = 0; j < _allInputFields.Count; ++j)
                {
                    _allInputFields[j].EmptyBuffer(true);
                }

                return ret;
            }
            catch(InvalidInputException e)
            {
                PrintErrorMessage(e.InputField);
                return null;
            }
        }

        private void ValidateInputFields()
        {
            IUIElement elemIter;
            for(int i = 0; i < _allInputFields.Count; ++i)
            {
                elemIter = _allInputFields[i]; 
                if(!_allInputFields[i].ValidateField())
                {
                    throw new InvalidInputException(_allInputFields[i]);
                }
            }
        }

        public void SetCurrentUIElement(IUIElement element)
        {
            _currentElementPosition = element;
        }
        private void DeleteCharacter()
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

        private void AddCharacter(ConsoleKeyInfo c)
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

        public void PrintErrorMessage(IUIElement errorField)
        {
            if (_errorInputField != null)
            {
                if(_errorInputField.BufferText.Length > 0)
                {
                    _errorInputField.EmptyBuffer(true);
                }
                //var currentElement = GetCurrentUIElement();
                SetCursorToUIElement(_errorInputField);
                Console.ForegroundColor = _errorTextColor;
                Console.BackgroundColor = ConsoleColor.Black;
                string errorMessage = $"{ErrorMessageBody} : {errorField.FieldName}";
                Console.Write(errorMessage);
                _errorInputField.SetBuffer(errorMessage,false);
                Console.ForegroundColor = _inputTextColor;
                Console.BackgroundColor = ConsoleColor.Black;
                SetCursorToUIElement(errorField);
            }
        }

        private ConsoleKey GetOppositeNavigationStateEvent(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    return ConsoleKey.RightArrow;
                case ConsoleKey.RightArrow:
                    return ConsoleKey.LeftArrow;
                case ConsoleKey.UpArrow:
                    return ConsoleKey.DownArrow;
                case ConsoleKey.DownArrow:
                    return ConsoleKey.UpArrow;
                default:
                    throw new ArgumentException("Unknown key provided for getting opposite navigation state event.");
            }
        }

///<summary>Terminate the internal WindowManager Init loop and return back to caller</summary>

        public void Terminate()
        {
            _terminated = true;
            Console.Clear();
        }

        public void Init()
        {
            DrawUIElements();
            SetCursorToUIElement(_currentElementPosition);
            while (!_terminated)
            {
                var key = Console.ReadKey(true);
                var coord = NavigationInput(key.Key);
                if(coord != null)
                    Console.SetCursorPosition(coord.Left, coord.Top);
                else if(coord == null)
                {
                    Action act = ActionInput(key.Key);
                    if (act != null)
                        act.Invoke();
                    else if(key.Key == ConsoleKey.Backspace)
                    {
                        DeleteCharacter();
                    }
                    else if (key.Key != ConsoleKey.Tab && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.RightArrow && key.Key != ConsoleKey.LeftArrow
                        && key.Key != ConsoleKey.UpArrow && key.Key != ConsoleKey.DownArrow)
                    {
                        AddCharacter(key);
                    }
                }
                
            }
        }

        public void AddDecors(ElementDecorators decor, params IUIElement[] elements)
        {
            foreach(IUIElement elem in elements)
            {
                elem.Decorators = decor;
            }
        }
    }
}
