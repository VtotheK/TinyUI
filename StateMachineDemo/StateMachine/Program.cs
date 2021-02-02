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
            var ERROR_FIELD =   window.CreateErrorMessageField("Errorfield", new CursorPosition(20, 20),15, InputType.String,false);
            var firstName =     window.CreateInputField("FirstName", new CursorPosition(0, 10), 15, InputType.String, false);
            var lastName =      window.CreateInputField("LastName", new CursorPosition(20, 10), 15, InputType.String, false);
            var age =           window.CreateInputField("Age", new CursorPosition(40, 10), 3, InputType.UnsignedInteger, false);
            var address =       window.CreateInputField("Address", new CursorPosition(45, 10), 30, InputType.String, false);
            firstName.Label = "Etunimi";
            lastName.Label  = "Sukunimi";
            age.Label  = "Ikä";
            address.Label  = "Osoite";
            window.CreateStateTransition(firstName, StateEvent.Right, lastName);
            window.CreateStateTransition(firstName, StateEvent.Left, address);
            window.CreateStateTransition(lastName, StateEvent.Left, firstName);
            window.CreateStateTransition(lastName, StateEvent.Right, age);
            window.CreateStateTransition(age, StateEvent.Left, lastName);
            window.CreateStateTransition(age, StateEvent.Right, address);
            window.CreateStateTransition(address, StateEvent.Left, age);
            window.CreateStateTransition(address, StateEvent.Right, firstName);
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
                            window.DeleteCharacter();
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            try
                            {
                                window.ValidateInput();
                            }
                            catch (InvalidInputException e){
                                break;
                                /*var fieldWhereError = window.GetCurrentUIElement();
                                Console.SetCursorPosition(ERROR_FIELD.Position.Left, ERROR_FIELD.Position.Top);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.Write(e.Message);
                                window.SetCursorToInputField(fieldWhereError);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.BackgroundColor = ConsoleColor.Black;*/
                            }
                            break;
                        }
                    default:
                        {
                            window.AddCharacter(key.KeyChar);
                            /*if (field.AddChar(key.KeyChar))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(key.KeyChar);
                                Console.ForegroundColor = ConsoleColor.White;
                            }*/
                            break;
                        }
                }
                    
            }
        }
    }
}




