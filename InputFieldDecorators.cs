using System;
using System.Collections.Generic;
using System.Text;

namespace TinyUI
{
    public class InputFieldDecorators
    {
        private char _topDecorator;
        private char _bottomDecorator;
        private char _leftDecorator;
        private char _rightDecorator;

        public InputFieldDecorators()
        {

        }

        public InputFieldDecorators(char topDecorator, char bottomDecorator, char leftDecorator, char rightDecorator)
        {
            _topDecorator = topDecorator;
            _bottomDecorator = bottomDecorator;
            _leftDecorator = leftDecorator;
            _rightDecorator = rightDecorator;
        }

        public char TopDecorator { get => _topDecorator; set => _topDecorator = value; }
        public char BottomDecorator { get => _bottomDecorator; set => _bottomDecorator = value; }
        public char LeftDecorator { get => _leftDecorator; set => _leftDecorator = value; }
        public char RightDecorator { get => _rightDecorator; set => _rightDecorator = value; }
    }
}
