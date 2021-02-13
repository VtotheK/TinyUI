# TinyUI
TinyUI is a small library to create buttons and input fields to .NET console applications

## How to use the library

Create empty .NET console project, clone this repository to the project folder. 

```git clone https://github.com/VtotheK/TinyUI```

Let's create a class that will hold the code  UI logic and funtion declarations for the buttons.

```cs
HelloWorldWindow window = new HelloWorldWindow();
```

Let's create a method inside our HelloWorldWindow class that will hold the inputfield and button declarations, as well as button press logic. We will call this method CreateUI

At this point our HelloWorldWindow class should look something like this:

![Initial class file](https://github.com/VtotheK/TinyUI/blob/master/Doc/CreateWindowManager.jpg)
Format: ![Alt Text](url)
