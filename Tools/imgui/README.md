# Heaven Gate 

## Intro

It`s time to describe our new story editor written by c++, easy to use and friendly to developers. This document will show the framework of Heaven Gate, its data struct and usage. If you are not a developer. then you should directly jump to the last part, about how to use this editor.

## The Framework of Heaven Gate

Heaven Gate is base three-part: model, view and controller. But these three names is come from the software engineering area, in Heaven Gate, we will call them data, window and controller for a better semantically understand.

### Data (Model)

Now we have two types of data, Story Json and Table Json.

#### Story Json

Story Json is a list of Story Node, every Node owns a type to show who it is. Story word is a core node, basically contains all important Story content.

Every Story Json is inherited from Story Node, a Story Node only contains a member Node Type.

When a Story Json will be saved in a file, it will serialize to an array of the JSON object, all type names are lower case.

Now in Heaven Gate, it will generate a singleton class called Story Json Manager to keep a global Story Json. It reduces the situation that every window keeps a reference of Story Josn and needs to check whether it is nullptr always.

#### Story Table

Story Table a template class to save a table type data. A Story Table is an array of Story Row, it is also a template class.

A Story row can save a character array. If you need to save another type of data in a column, you need to create a special table or write a method that can transform a special type to a character array. 

Like Story Json Manager, now we also have a Table Json Manager to keep all type Table Json.

### Window (View)

Now we have a window center to initialize all windows and singleton class. 

All window classes are base on Base Window, what you only need to do is write a class inherits Base Class and override UpdateWindow and UpdateMenu.

### Controller

Basically they are different type singleton managers, like Story File Manager is used to control write and read of Story Json and Story Table.

## Data Structure

## Usage