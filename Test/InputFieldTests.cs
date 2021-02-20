using NUnit.Framework;
using TinyUI;

namespace TinyUITests
{
    public class InputFieldTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InputFieldBufferTests()
        {
            InputField buffer = new InputField("Buffertest", new CursorPosition(20, 20), 20,InputType.StringNoNumbersNoSpecialCharacters, false);
            buffer.SetBuffer("abcdefghiljklmnopqrs");
            Assert.True(buffer.ValidateField());
            buffer.AddCharToBuffer('t');
            Assert.True(buffer.ValidateField());
            buffer.EmptyBuffer(false);
            Assert.False(buffer.ValidateField());
            buffer.SetBuffer("abcde");
            Assert.AreEqual(5, buffer.BufferLength);
            Assert.True(buffer.DeleteChar());
            Assert.AreEqual(4, buffer.BufferLength);
            Assert.True(buffer.ValidateField());
            buffer.AddCharToBuffer('l');
            Assert.AreEqual(5, buffer.BufferLength);
            Assert.True(buffer.DeleteChar());
        }

        [Test]
        public void InputFieldValidationStringOnlyLetters()
        {
            InputField fieldNoSpec = new InputField("TestFieldSpec", new CursorPosition(10, 10),20,InputType.StringNoNumbersNoSpecialCharacters,false);
            Assert.IsEmpty(fieldNoSpec.BufferText);
            fieldNoSpec.SetBuffer("ab$");
            Assert.False(fieldNoSpec.ValidateField());
            fieldNoSpec.SetBuffer("4&");
            Assert.False(fieldNoSpec.ValidateField());
        }

        [Test]
        public void InputFieldValidationOnlyIntegerAndNull()
        {
            InputField fieldOnlyInt = new InputField("TestfieldInt", new CursorPosition(10, 10),10,InputType.Integer, true);
            Assert.True(fieldOnlyInt.ValidateField());
            Assert.IsEmpty(fieldOnlyInt.BufferText);
            Assert.AreEqual(10, fieldOnlyInt.Maxchars);
            fieldOnlyInt.SetBuffer("123123123");
            Assert.True(fieldOnlyInt.ValidateField());
            fieldOnlyInt.AddCharToBuffer('c');
            Assert.False(fieldOnlyInt.ValidateField());
            Assert.True(fieldOnlyInt.DeleteChar());
            Assert.True(fieldOnlyInt.ValidateField());
            fieldOnlyInt.SetBuffer("-10");
            Assert.True(fieldOnlyInt.ValidateField());
        }
        [Test]
        public void InputFieldValidationUInt()
        {
            InputField uintField = new InputField("TestfieldUint", new CursorPosition(20, 10), 3,InputType.UnsignedInteger, true);
            Assert.IsEmpty(uintField.BufferText);
            uintField.SetBuffer("-2");
            Assert.False(uintField.ValidateField());
            uintField.SetBuffer("abc");
            Assert.False(uintField.ValidateField());
            uintField.SetBuffer("10");
            Assert.True(uintField.ValidateField());
            uintField.SetBuffer("1%");
            Assert.False(uintField.ValidateField());


        }
    }
}