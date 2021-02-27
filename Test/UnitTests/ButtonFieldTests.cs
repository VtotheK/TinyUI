using System;
using System.Collections.Generic;
using NUnit.Framework;
using TinyUI;

namespace TinyUITests
{
    public delegate void TestDelegate();
    class ButtonFieldTests
    {
        public void ActionTest()
        {

        }

        [Test]
        public void ActionInvokeTest()
        {
            WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
            var button = manager.CreateButtonField("TestButton", new CursorPosition(2, 2), "MyButton");
            Assert.IsInstanceOf<ButtonField>(button);
            Assert.IsInstanceOf<IUIElement>(button);
            manager.CreateActionStateTransition(button, ConsoleKey.Enter, this.ActionTest);
            TestDelegate dele = this.ActionTest;
            Assert.True(dele.Method == button.ElementAction.Method);
        }
    }
}
