# TinyUI
TinyUI is a small library to create buttons and input fields to .NET console applications, and navigate between the fields with keyboard. You can filter the input data and get multiple user inputs with one static console screen.

This library was created as a student project.

## Tutorial

Create empty .NET console project, clone this repository to the project folder. 

```git clone https://github.com/VtotheK/TinyUI```


Create a class called HelloWorldWindow and create a method called "CreateUI" inside our HelloWorldWindow class. This method will hold the inputfield and button declarations, as well as button press logic. Write `using TinyUI` in the namespace section.

Inside your main function, instansiate a HelloWorldWindow object, and make a call to the CreateUI method.

Next you need to instansiate a WindowManager object. You can do it inside the CreateUI method, or you can create one inside your HelloWorldWindow class constructor, or just create a field when the HelloWorldWindow class is instansiated. 

WindowManager construtor takes two `ConsoleColor` parameters, first for the input text color, second for printing error messages to the user.

At this point our HelloWorldWindow class should look something like this:
```cs
using System;
using TinyUI;

namespace MyApp
{
    class HelloWorldWindow
    {
        private WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
        public HelloWorldWindow() 
        {

        }

        public void CreateUI()
        {
            
        }
    }
}
```

Using our WindowManager object, let's create two input fields, one for getting user's first name, and second to get user's age. Let's set them as class fields so we can reference the inputfields at runtime and hookup function calls to them:

### Inputfield
```cs
namespace MyApp
{
    class HelloWorldWindow
    {
        private WindowManager manager = new WindowManager(ConsoleColor.White, ConsoleColor.Red);
        private InputField firstNameField;
        private InputField ageField;
        public HelloWorldWindow() 
        {

        }

        public void CreateUI()
        {
            firstNameField = manager.CreateInputField("FirstName", new CursorPosition(2, 2), 10, InputType.StringNoNumbersNoSpecialCharacters, false);
            ageField = manager.CreateInputField("Age", new CursorPosition(20, 2), 3, InputType.Integer, false);
            firstNameField.Label = "First name";
            ageField.Label = "Age";
        }
    }
}
```
WindowManger has a method `CreateInputField` for creating the input fields. This method takes the following parameters:

`string name` Identifier for the input field, so use **unique names** for and between the input and button fields. This property will also be used later to read the input data from the user.

`Cursorposition position` Encapsulates the left and top positions where the inputfield will be drawn in the console. Cursonposition class constructor takes two arguments, _left_ and _top_. These will be the coordinates where the input field will **start**.

`uint maxcharacters`Maximum amount of characters the input field will take. 

`InputType inputType` Determines what the field will accept as input. Only numbers, strings with no number or special characters, string with no numbers but special characters are allowed etc.

`bool allowNullValues` When the inputfield(s) will be validated, do you allow this field to be empty? 

CreateInputField method returns **Inputfield** type object that was created inside WindowManager. Everytime you need to reference the inputfield you just created, you need use this object.

If you want to give a label for this input field, InputField class has property **Label**.

### Button field

```cs
public void CreateUI()
{
    firstNameField = manager.CreateInputField("FirstName", new CursorPosition(2, 2), 10, InputType.StringNoNumbersNoSpecialCharacters, false);
    ageField = manager.CreateInputField("Age", new CursorPosition(20, 2), 3, InputType.Integer, false);

    firstNameField.Label = "First name";
    ageField.Label = "Age";

    ButtonField sendButton = manager.CreateButtonField("sendButton", new CursorPosition(13, 5), "Send");
}
```

Let's create one button which will act as a trigger to read the data from the input fields.

Like with the input field, there is a method for creating button fields inside the WindowManager class. CreateButtonField method takes the following argumenets:

`string name` Identifier for the button field, used to identify the button field, so use **unique names** for and between the input and button fields.

`Cursorposition position` Look @ creation of input field.

`string buttonLabel` The label for the button. This the text that will be highlighted/de-highlighted when activating/de-activating the button.

### Navigation between the fields
Once you have declared the fields you need, it is time to create the movement logic between the input fields. This library only support movement by keyboard, no mouse. Let's create a movement logic between the FirstName and Age fields.

Use the `CreateNavigationTransition` method to save the wanted movement logic between the fields. This method saves the transitions into dictionary, which will be used by the internal statemachine to determine where to move the cursor at runtime.

```cs
public void CreateUI()
{
    firstNameField = manager.CreateInputField("FirstName", new CursorPosition(2, 2), 10, InputType.StringNoNumbersNoSpecialCharacters, false);
    ageField = manager.CreateInputField("Age", new CursorPosition(20, 2), 3, InputType.Integer, false);

    firstNameField.Label = "First name";
    ageField.Label = "Age";

    ButtonField sendButton = manager.CreateButtonField("sendButton", new CursorPosition(13, 5), "Send");

    manager.CreateNavigationStateTransition(firstName, ConsoleKey.RightArrow, age, true);
}
```

`CreateNavigationTransition` method takes the following arguments:

`IUIElement fromField` Any object that implements the `IUIElement` interface. The field from which you want the transition to be made to some other field.

`ConsoleKey key` The keyboard key, that you want to bind as a trigger for executing the transition.

`IUIElement toField` Any object that implements the `IUIElement` interface. The target field where you want to end up when the trigger is executed.

`bool bothWays` Do you want to create back-and-forth transition between the objects? See picture below.

![bothWays argument](https://github.com/VtotheK/TinyUI/blob/master/Doc/NavigationStateTransitionBothWays.jpg)

Let's create the rest of the navigation transitions to complete the navigation in our UI.

```cs
public void CreateUI()
{
    firstNameField = manager.CreateInputField("FirstName", new CursorPosition(2, 2), 10, InputType.StringNoNumbersNoSpecialCharacters, false);
    ageField = manager.CreateInputField("Age", new CursorPosition(20, 2), 3, InputType.Integer, false);
    firstNameField.Label = "First name";
    ageField.Label = "Age";

    ButtonField sendButton = manager.CreateButtonField("sendButton", new CursorPosition(13, 5), "Send");

    manager.CreateNavigationStateTransition(firstName, ConsoleKey.RightArrow, age, true);
    manager.CreateNavigationStateTransition(firstName, ConsoleKey.DownArrow, sendButton, true);
    manager.CreateNavigationStateTransition(age, ConsoleKey.DownArrow, sendButton, false);

    manager.SetCursorToUIElement(firstName);
    manager.Init();
}
```
At this point our navigation diagram looks like this

![Navigation diagram](https://github.com/VtotheK/TinyUI/blob/master/Doc/NavigationDiagram.jpg)

You also need to call `SetCursorToUIElement` to specify the starting point where the cursor will be. This method accepts object as argument that implements `IUIElement` interface.  
`Init` function draws the UI elements and starts to take input from the user.

Small demo what we have done so far

![giphy demo gif](https://media0.giphy.com/media/AdEwiBNCMoSI96zVR8/giphy.gif)

### Button invoke function call

To bind a function to button press event, use WindowManagers `CreateActionStateTransition` function.
```cs
public void CreateUI()
{
    .
    .
    .
    manager.CreateActionStateTransition(sendButton, ConsoleKey.Enter, SendMessage);

    manager.SetCursorToUIElement(firstNameField);
    manager.Init();
}
```

CreateActionStateTransition takes the following parameters

`IUIElement fromElement` the sender element of which the function call should be invoked from.

`ConsoleKey stateEvent` ConsoleKey enum, the wanted keybinding to trigger the action

`Action toAction` The target function, which will be called.

As you can see, in this example i have created a SendMessage function to `HelloWorldWindow` class as an example. 

```cs
private void SendMessage()
{
    var dict = manager.GetDataFromInputFields();
    string name = dict[firstNameField.FieldName];
    string age = dict[ageField.FieldName];

    Console.SetCursorPosition(3, 6);
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write($"SENT: Name:{name}, age:{age}!");
    Console.ForegroundColor = ConsoleColor.White;
}
```

This function calls `WindowManager.GetDataFromInputFields` function, which returns a `Dictionary<string,string>` object which will contain the data from the inputfield(s) . The __key__ for the dictionary is the InputField.FieldName property, and the __value__ for the key is the inputted data by the user. Small demo below.

![giphy demo gif](https://media4.giphy.com/media/T96oiRNyoISklxRRbQ/giphy.gif)

Let's add a small finishing touch by adding decorator around our inputfields by instasiating `ElementDecorators` object and passing into constructor the chars that we want to decorate our field with. Then pass that object to `WindowManager.AddDecors` along with the fields you want to add them to.
```cs
public void CreateUI()
{
    ...
    ElementDecorators decors = new ElementDecorators(null,null,'[',']');
    manager.AddDecors(decors, firstNameField, ageField);
    
    ButtonField sendButton = manager.CreateButtonField("sendButton", new CursorPosition(13, 5), "Send");
    ...
    manager.SetCursorToUIElement(firstNameField);
    manager.Init();
}
```

And we end up with.

![giphy demo gif](https://media1.giphy.com/media/3O6EHAb59Xe2gUC6xY/giphy.gif)
