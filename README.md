<p align="center">
    <img src="Sphere.png" width="200"/>
    <h1 align="center"><b>Sphere</b></h1>
</p>

Sphere is a minimalistic language designed for both software and Operating System Development. <br>
The language syntax is designed to balance readability and writability by minimizing the amount of characters required to write while maintaining the self-explanatory nature of the code and namings. <br><br>
This language uses Freestanding-compatible C/C++ code to minimize the amount of refactoring reuqired to get Sphere-written projects to run on both with and without an environment.<br>
<br>
<br>

### Identifier Prefixes
| Handle      | Description                                       |
|-------------|---------------------------------------------------|
| `$<object>` | Gets or sets the value of the specified object    |
| `@<object>` | Gets or sets the address of the specified object. |

[Learn more (coming soon)](https://github.com/NullifyDev/Sphere)
<br><br>
### Instructions
| Instructions and Arguments          | Description                                                             |
|-------------------------------------|-------------------------------------------------------------------------|
| `mov <object> <int>`                | Move the object by signed number of addresses                           | 
| `incr <int>`                        | Increment current address by the given amount                           |
| `decr <int>`                        | Decrement current address by the given amount                           |
| `<string>(): <DataType>`            | Function with name as string with one argument                          |
| `out <arsg>`                        | Print all arguments before EOL without line break.                      |
| `outln <args>`                      | Print all arguments before EOL with line break.                         |
| `if <Condition> { <instructions> }` | Executes Instructions when condition returns true                       |

[Learn more (coming soon)](https://github.com/NullifyDev/Sphere)
<br><br>

### Support
Any operating systems that support LLVM and `clang` will be immediately supported.

### Indefinate Softawre
Here are some of the software that are under consideration of development using Sphere as the language
 - [ ] Petroglyph IDE
 - [ ] Boulder Package Manager
 - [ ] RollOS
 - [ ] Asciigine (Console/Terminal based Game Engine with Ascii graphics)
