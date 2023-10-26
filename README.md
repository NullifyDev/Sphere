<img src="Qbe.png" style="align-items: center" width="150"/>

# Qbe
This is a minimalistic language, designed to be simple to understand and utilize.
The language has a library named "ExtraKeys" to further aid the programmer without touching any of the pointer things.

### Official Modules
ExtraKeys - ExtraKeys is a library that adds functions and keywords like `for` and `while` to the langugae to further ease the development of some, making Qbe feel like it is "High-Level" when it isnt.


### Data Types
__Do Note:__ A name of an (official) module will be in brackets `()` to indicate in what module that thing can be found.


### Reference Operators
| Handle    | Description                   |
|-----------|-------------------------------|
| `@<var>`  | Address of the given register |
| `$<var>`  | Value of the given register   |

### Instructions

| Command and `<arguments>` | Description                                                             |
|---------------------------|-------------------------------------------------------------------------|
| `up <bin>`                | Move up by the given amount. Default Value: 1                           |
| `down <bin>`              | Move down by the given amount. Default Value: 1                         |
| `incr <int>`              | Increment current address by the given amount. Default value: 1         |
| `decr <int>`              | Decrement current address by the given amount. Default value: 1         |
| `func identifier (any)`   | Function with name with any arguments and argument types                |


<br>

### Support
Due to the fact that this is a prototype, this project is written in C#, developed with .Net 6 (soon to be updated with .Net 7 on November 2022), All of the Desktop Operating Systems (and other OSs that have .Net Support) are supported automatically (compilation script coming soon).
- [x] Linux
- [x] Windows
- [x] MacOS
