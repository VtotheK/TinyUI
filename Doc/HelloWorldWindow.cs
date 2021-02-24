using System;
using TinyUI;

namespace MyApp
{
    class HelloWorldWindow
    {
        private WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
        private InputField firstNameField;
        private InputField ageField;
        public HelloWorldWindow() 
        {

        }

        private void SendMessage()
        {
            var dict = manager.GetDataFromInputFields();
            string name = dict[firstNameField.FieldName];
            string age = dict[ageField.FieldName];

            Console.SetCursorPosition(3, 6);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"SENT: Name:{name}, age:{age}!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void CreateUI()
        {
            firstNameField = manager.CreateInputField("FirstName", new CursorPosition(2, 2), 10, InputType.StringNoNumbersNoSpecialCharacters, false);
            ageField = manager.CreateInputField("Age", new CursorPosition(20, 2), 3, InputType.Integer, false);
            firstNameField.Label = "First name";
            ageField.Label = "Age";

            ElementDecorators decors = new ElementDecorators(null,null,'[',']');
            manager.AddDecors(decors, firstNameField, ageField);
            ButtonField sendButton = manager.CreateButtonField("sendButton", new CursorPosition(13, 5), "Send");

            manager.CreateNavigationStateTransition(firstNameField, ConsoleKey.RightArrow, ageField, true);
            manager.CreateNavigationStateTransition(firstNameField, ConsoleKey.DownArrow, sendButton, true);
            manager.CreateNavigationStateTransition(ageField, ConsoleKey.DownArrow, sendButton, false);

            manager.CreateActionStateTransition(sendButton, ConsoleKey.Enter, SendMessage);

            manager.SetCursorToUIElement(firstNameField);
            manager.Init();
        }
    }


    /*      
            manager.SetCursorToUIElement(firstName);
            manager.Init();
            
*/
}










//ButtonField sendButton = manager.CreateButtonField("SendButton", new CursorPosition()








