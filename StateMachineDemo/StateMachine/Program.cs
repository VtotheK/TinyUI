using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using StateMachineDemo;

namespace StateMachineDemo
{
    class Program
    {
        
        static void Main(string[] args)
        {
            WindowManager window = new WindowManager(ConsoleColor.White,ConsoleColor.Red);
            Console.SetWindowSize(60, 40);
            var ERROR_FIELD =   window.CreateErrorMessageField("Errorfield", new CursorPosition(0, 15),15, InputType.String,false);
            var firstName =     window.CreateInputField("Etunimi", new CursorPosition(0, 10), 15, InputType.String, false);
            var lastName =      window.CreateInputField("Sukunimi", new CursorPosition(20, 10), 15, InputType.String, false);
            var age =           window.CreateInputField("Ikä", new CursorPosition(40, 10), 3, InputType.UnsignedInteger, false);
            var address =       window.CreateInputField("Osoite", new CursorPosition(45, 10), 30, InputType.String, false);
            firstName.Label = "Etunimi";
            lastName.Label  = "Sukunimi";
            age.Label  = "Ikä";
            address.Label  = "Osoite";
            window.CreateNavigationStateTransition(firstName, NavigationStateEvent.Right, lastName);
            window.CreateNavigationStateTransition(firstName, NavigationStateEvent.Left, address);
            window.CreateNavigationStateTransition(lastName, NavigationStateEvent.Left, firstName);
            window.CreateNavigationStateTransition(lastName, NavigationStateEvent.Right, age);
            window.CreateNavigationStateTransition(age, NavigationStateEvent.Left, lastName);
            window.CreateNavigationStateTransition(age, NavigationStateEvent.Right, address);
            window.CreateNavigationStateTransition(address, NavigationStateEvent.Left, age);
            window.CreateNavigationStateTransition(address, NavigationStateEvent.Right, firstName);
            window.CreateActionStateTransition(address, ActionStateEvent.Enter, window.Test);
            window.DrawInputFieldLabels();
            window.SetCursorToUIElement(firstName);
            while (true)
            {
                
                var key = Console.ReadKey(true);
                
                switch (key.Key) {
                    case ConsoleKey.RightArrow:
                        {
                            var coord = window.UserInput(NavigationStateEvent.Right);
                            Console.SetCursorPosition(coord.Left, coord.Top);
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            var coord = window.UserInput(NavigationStateEvent.Left);
                            Console.SetCursorPosition(coord.Left, coord.Top);
                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            window.DeleteCharacter();
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            try
                            {
                                window.ValidateInput();
                            }
                            catch (InvalidInputException e) { 
                                window.PrintErrorMessage(e.FieldName);
                            }
                            break;
                        }
                    default:
                        {
                            window.AddCharacter(key.KeyChar);
                            break;
                        }
                }
                    
            }
        }
    }
}




