using System;
using System.Collections.Generic;
using NUnit.Framework;
using TinyUI;

namespace TinyUITests
{
    public class InputFieldTests
    {
        const string TESTUINT = "TestfieldUint";
        const string TESTINT = "TestfieldInt";
        const string TESTSPEC = "TestFieldSpec";
        const string TESTBUFFER = "Buffertest";
       
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void InputFieldBufferTests()
        {
            WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
            InputField letterBuffer = manager.CreateInputField(TESTBUFFER, new CursorPosition(20, 20), 20,InputType.StringNoNumbersNoSpecialCharacters, false);
            letterBuffer.SetBuffer("abcdefghiljklmnopqrs");
            Assert.True(letterBuffer.ValidateField());
            Assert.Greater(letterBuffer.BufferLength,10);
            letterBuffer.AddCharToBuffer('t');
            Assert.True(letterBuffer.ValidateField());
            letterBuffer.EmptyBuffer(false);
            Assert.False(letterBuffer.ValidateField());
            letterBuffer.SetBuffer("abcde");
            Assert.AreEqual(5, letterBuffer.BufferLength);
            Assert.True(letterBuffer.DeleteChar());
            Assert.AreEqual(4, letterBuffer.BufferLength);
            Assert.True(letterBuffer.ValidateField());
            letterBuffer.AddCharToBuffer('l');
            Assert.AreEqual(5, letterBuffer.BufferLength);
            Assert.True(letterBuffer.DeleteChar());
        }

        [Test]
        public void InputFieldValidationStringOnlyLetters()
        {
            WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
            InputField fieldNoSpec = manager.CreateInputField(TESTSPEC, new CursorPosition(10, 10),20,InputType.StringNoNumbersNoSpecialCharacters,false);
            Assert.IsEmpty(fieldNoSpec.BufferText);
            fieldNoSpec.SetBuffer("ab$");
            Assert.False(fieldNoSpec.ValidateField());
            fieldNoSpec.SetBuffer("4&");
            Assert.False(fieldNoSpec.ValidateField());
            fieldNoSpec.SetBuffer("testinen");
        }

        [Test]
        public void InputFieldValidationOnlyIntegerAndNull()
        {
            int temp;
            WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
            InputField fieldOnlyInt = manager.CreateInputField(TESTINT, new CursorPosition(10, 10),10,InputType.Integer, true);
            Assert.True(fieldOnlyInt.ValidateField());
            Assert.IsEmpty(fieldOnlyInt.BufferText);
            Assert.AreEqual(10, fieldOnlyInt.Maxchars);
            fieldOnlyInt.SetBuffer("123123123");
            Assert.GreaterOrEqual(fieldOnlyInt.BufferLength,9);
            Assert.True(Int32.TryParse(fieldOnlyInt.BufferText, out temp));
            Assert.True(fieldOnlyInt.ValidateField());
            fieldOnlyInt.AddCharToBuffer('c');
            Assert.False(fieldOnlyInt.ValidateField());
            Assert.False(Int32.TryParse(fieldOnlyInt.BufferText, out temp));
            Assert.True(fieldOnlyInt.DeleteChar());
            Assert.True(fieldOnlyInt.ValidateField());
            fieldOnlyInt.SetBuffer("-10");
            Assert.True(fieldOnlyInt.ValidateField());
        }
        [Test]
        public void InputFieldValidationUInt()
        {
            WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
            var uintField = manager.CreateInputField(TESTUINT, new CursorPosition(20, 10), 3,InputType.UnsignedInteger, true);
            Assert.IsInstanceOf<InputField>(uintField);
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

        [Test]
        public void GetAllInputs()
        {
            WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
            var testbuf = manager.CreateInputField(TESTBUFFER, new CursorPosition(1, 1), 10, InputType.String, true);
            var testuint = manager.CreateInputField(TESTUINT, new CursorPosition(1, 1), 10, InputType.String, true);
            var data = manager.GetDataFromInputFields();
            Assert.IsInstanceOf<Dictionary<string, string>>(data);
            Assert.Throws<KeyNotFoundException>( () => { var a = data["NoSuchKey"]; });
            Assert.DoesNotThrow( () => { var a = data[TESTBUFFER]; });
            Assert.DoesNotThrow( () => { var a = data[TESTUINT]; });
            Assert.IsInstanceOf<InputField>(testbuf);
            Assert.IsInstanceOf<InputField>(testuint);
        }
    }
}