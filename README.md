# TinyUI
TinyUI is a small library to create buttons and input fields to .NET console applications

## Tutorial

Create empty .NET console project, clone this repository to the project folder. 

```git clone https://github.com/VtotheK/TinyUI```


Create a class called HelloWorldWindow and create a method called "CreateUI" inside our HelloWorldWindow class. This method will hold the inputfield and button declarations, as well as button press logic. Write `using TinyUI` in the namespace section.

Inside your main function, instansiate a HelloWorldWindow object, and make a call to the CreateUI method.

Next you need to instansiate a WindowManager object, either inside the CreateUI method or you can create one inside your HelloWorldWindow class constructor. WindowManager is the class that you will mostly deal when creating the UI.

WindowManager construtor takes two `ConsoleColor` parameters, one for the input text color, one for printing error messages to the user.

At this point our HelloWorldWindow class should look something like this:

![Initial class file](https://github.com/VtotheK/TinyUI/blob/master/Doc/CreateWindowManager.jpg)

Using our WindowManager object, let's create two input fields, one for getting our first name, and second to get our age:

![Input field creation](https://github.com/VtotheK/TinyUI/blob/master/Doc/InputFields.jpg)

Inputfield creation takes the following parameters:

```cs 
string name
```
The name of the input field. Identifier for the input field, used to identify the inputfield so **use unique names**. Will also be used later to read the input data from the user.

```cs 
Cursorposition position
```
Encapsulates the left and top positions where the inputfield will be drawn. Cursonposition class constructor takes two arguments, _left_ and _top_. These will be the coordinates where the input field will **start**.

```cs 
uint maxcharacters
```
Maximum amount of characters the input field will take. 

```cs 
InputType inputType
```
Determines what the field will accept as input. Only numbers, strings with no number or special characters etc.

```cs 
bool allowNullValues
```
When the inputfield(s) will be validated, do you allow this field to be empty? 
