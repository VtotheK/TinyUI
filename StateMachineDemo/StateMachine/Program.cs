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
            WindowManager window = new WindowManager();
            var firstName = window.CreateInputField("FirstName", new CursorPosition(10, 10), 15, typeof(string), true);
            var lastName = window.CreateInputField("LastName", new CursorPosition(30, 10), 15, typeof(string), true);
            var age = window.CreateInputField("Age", new CursorPosition(50, 10), 3, typeof(int), true);
            var address = window.CreateInputField("Address", new CursorPosition(55, 10), 30, typeof(string), true);
            firstName.Label = "Etunimi";
            lastName.Label  = "Sukunimi";
            age.Label  = "Ikä";
            address.Label  = "Osoite";
            window.CreateStateTransitions(firstName, StateEvent.Right, lastName);
            window.CreateStateTransitions(firstName, StateEvent.Left, address);
            window.CreateStateTransitions(lastName, StateEvent.Left, firstName);
            window.CreateStateTransitions(lastName, StateEvent.Right, age);
            window.CreateStateTransitions(age, StateEvent.Left, lastName);
            window.CreateStateTransitions(age, StateEvent.Right, address);
            window.CreateStateTransitions(address, StateEvent.Left, age);
            window.CreateStateTransitions(address, StateEvent.Right, firstName);
            window.DrawInputFieldLabels();
            window.SetCursorToInputField(firstName);
            while (true)
            {
                var key = Console.ReadKey(true);
                
                switch (key.Key) {
                    case ConsoleKey.RightArrow:
                        {
                            var coord = window.UserInput(StateEvent.Right);
                            Console.SetCursorPosition(coord.Left, coord.Top);
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            var coord = window.UserInput(StateEvent.Left);
                            Console.SetCursorPosition(coord.Left, coord.Top);
                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            var field = window.GetInputField();
                            if (field.DeleteChar()) 
                                Console.Write("\b \b");
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            try
                            {
                                window.ValidateInput();
                            }
                            catch (Exception e){
                                
                            }
                            break;
                        }
                    default:
                        {
                            var field = window.GetInputField();
                            if (field.AddChar(key.KeyChar))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(key.KeyChar);
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            break;
                        }
                }
                    
            }
        }
    }
}




