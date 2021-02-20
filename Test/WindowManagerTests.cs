using NUnit.Framework;
using TinyUI;

namespace TinyUITests
{
    class WindowManagerTests
    {

        [Test]
        public void TransitionTest()
        {
            WindowManager manager = new WindowManager(System.ConsoleColor.White,System.ConsoleColor.Red);

            var firstField = manager.CreateInputField("firstField",new CursorPosition(10,10),10,InputType.String,true);
            var secondField = manager.CreateInputField("secondField",new CursorPosition(20,20),10,InputType.String,true);
            Assert.IsInstanceOf<InputField>(firstField);
            Assert.IsInstanceOf<InputField>(secondField);
            manager.CreateNavigationStateTransition(firstField, System.ConsoleKey.LeftArrow, secondField,true);
            manager.SetCursorToUIElement(firstField);
            manager.NavigationInput(System.ConsoleKey.LeftArrow);
            Assert.AreSame(secondField, manager.GetCurrentUIElement());
            manager.NavigationInput(System.ConsoleKey.RightArrow);
            Assert.AreSame(firstField, manager.GetCurrentUIElement());
        }
    }
}
