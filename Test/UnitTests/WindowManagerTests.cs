using NUnit.Framework;
using TinyUI;

namespace TinyUITests
{
    class WindowManagerTests
    {

        [Test]
        public void CurrentUiElement()
        {
            WindowManager manager = new WindowManager(System.ConsoleColor.White,System.ConsoleColor.Red);

            var firstField = manager.CreateInputField("firstField",new CursorPosition(10,10),10,InputType.String,true);
            var secondField = manager.CreateInputField("secondField",new CursorPosition(20,20),10,InputType.String,true);
            Assert.IsInstanceOf<InputField>(firstField);
            Assert.IsInstanceOf<InputField>(secondField);
            manager.CreateNavigationStateTransition(firstField, System.ConsoleKey.LeftArrow, secondField,true);
            manager.SetCurrentUIElement(firstField);
            Assert.AreSame(firstField, manager.GetCurrentUIElement());
        }

        [Test]
        public void CurrentElementTest()
        {
            WindowManager manager = new WindowManager(System.ConsoleColor.White, System.ConsoleColor.Red);
            var btn = manager.CreateButtonField("TestButton", new CursorPosition(2, 2), "TestLabel");
            Assert.IsInstanceOf<ButtonField>(btn);
            manager.SetCurrentUIElement(btn);

            var btnref = manager.GetCurrentUIElement();
            Assert.AreSame(btn, btnref);

            var otherbutton = manager.CreateButtonField("OtherTestButton", new CursorPosition(1, 1), "TestLabel");

            manager.SetCurrentUIElement(otherbutton);
            Assert.AreNotSame(otherbutton, btn);

            manager.SetCurrentUIElement(btn);
            Assert.AreSame(btn, btnref);
        }
    }
}
