# TinyUI
TinyUI is a small library to create buttons and input fields to .NET console applications, and navigate between the fields with keyboard, no support for mouse navigation **currently**.

This library was created as a student project.

## Tutorial

Create empty .NET console project, clone this repository to the project folder. 

```git clone https://github.com/VtotheK/TinyUI```


Create a class called HelloWorldWindow and create a method called "CreateUI" inside our HelloWorldWindow class. This method will hold the inputfield and button declarations, as well as button press logic. Write `using TinyUI` in the namespace section.

Inside your main function, instansiate a HelloWorldWindow object, and make a call to the CreateUI method.

Next you need to instansiate a WindowManager object, either inside the CreateUI method or you can create one inside your HelloWorldWindow class constructor, or just create a field once the HelloWorldWindow is instansiated. In the following picture the last option was used WindowManager is the class that you will mostly deal when creating the UI.

WindowManager construtor takes two `ConsoleColor` parameters, first for the input text color, second for printing error messages to the user.

At this point our HelloWorldWindow class should look something like this:

![Initial class file](https://github.com/VtotheK/TinyUI/blob/master/Doc/CreateWindowManager.jpg)

Using our WindowManager object, let's create two input fields, one for getting our first name, and second to get our age:

###Inputfield creation

![Input field creation](https://github.com/VtotheK/TinyUI/blob/master/Doc/InputFields.jpg)

CreasteInputField takes the following parameters:

`string name` The name of the input field. Identifier for the input field, used to identify the input field, so use **unique names** for and between the input and button fields. This property will also be used later to read the input data from the user.

`Cursorposition position` Encapsulates the left and top positions where the inputfield will be drawn in the console. Cursonposition class constructor takes two arguments, _left_ and _top_. These will be the coordinates where the input field will **start**.

`uint maxcharacters`Maximum amount of characters the input field will take. 

`InputType inputType` Determines what the field will accept as input. Only numbers, strings with no number or special characters, string with no numbers but special characters are allowed etc.

`bool allowNullValues` When the inputfield(s) will be validated, do you allow this field to be empty? 

CreateInputField method returns **Inputfield** type object that was created inside WindowManager. Everytime you need to reference the inputfield you just created, you will use this object.
If you want to give a label for this input field, InputField class has property **Label**.

### Button field creation
Let's create one button which will act as trigger to try validate the input fields and get the inputted data.

![Input field creation](https://github.com/VtotheK/TinyUI/blob/master/Doc/CreateButtonField.jpg)

Buttonfield class constructor takes the following parameters:

`string name` The name of the button field. Identifier for the button field, used to identify the button field, so use **unique names** for and between the input and button fields.

`Cursorposition position` Look @ creation of input field.

`string buttonLabel` The label for the button. This the text that will be highlighted/de-highlighted when moving between the fields.

### Movement between the fields
Once you have declared the fields you need, it is time to create the movement logic between the input fields. This library only support movement by keyboard, no mouse. Let's create a movement logic between the FirstName and Age fields.


