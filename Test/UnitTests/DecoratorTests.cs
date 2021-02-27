using System;
using System.Collections.Generic;
using NUnit.Framework;
using TinyUI;

namespace TinyUITests
{
    class DecoratorTests
    {
        [Test]
        public void DecoratorTest()
        {
            var decors = new ElementDecorators();
            Assert.IsInstanceOf<ElementDecorators>(decors);
            Assert.IsNull(decors.LeftDecorator);
            decors.LeftDecorator = '{';
            Assert.That(decors.LeftDecorator.GetType() == new char?('c').GetType());
        }
    }
}
