using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using StateMachineDemo.StateMachine;

namespace StateMachineDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowLayout();
            WindowState window = new WindowState(CursorPosition.FirstName);
            var pos = window.GetCurrentPosition();
            Console.SetCursorPosition(pos.Left, pos.Top);
            while (true)
            {
                var key = Console.ReadKey(true);
                
                switch (key.Key) {
                    case ConsoleKey.RightArrow:
                        {
                            window.ChangeState(StateEvent.Right);
                            var coord = window.GetCurrentPosition();
                            Console.SetCursorPosition(coord.Left, coord.Top);
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            window.ChangeState(StateEvent.Left);
                            var coord = window.GetCurrentPosition();
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
                    default:
                        {
                            var field = window.GetInputField();
                            field.AddChar(key.KeyChar);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(key.KeyChar);
                            Console.ForegroundColor = ConsoleColor.White;
                            var coord = window.GetCurrentPosition();
                            Console.SetCursorPosition(coord.Left, coord.Top);
                            break;
                        }
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




