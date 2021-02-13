# TinyUI
TinyUI is a small library to create buttons and input fields to .NET console applications

## Tutorial

Create empty .NET console project, clone this repository to the project folder. 

```git clone https://github.com/VtotheK/TinyUI```


Create a class called HelloWorldWindow and create a method called "CreateUI" inside our HelloWorldWindow class. This method will hold the inputfield and button declarations, as well as button press logic. Write `using TinyUI` in the namespace section.

Inside your main function, instansiate a HelloWorldWindow object, and make a call to the CreateUI method.

Next you need to instansiate a WindowManager object, either inside the CreateUI method or you can create one inside your HelloWorldWindow class constructor. This is the class that you will mostly deal when creating the UI.

WindowManager construtor takes two `ConsoleColor` parameters, one for the input text color, one for printing error messages to the user.

At this point our HelloWorldWindow class should look something like this:

![Initial class file](https://github.com/VtotheK/TinyUI/blob/master/Doc/CreateWindowManager.jpg)

