<p align="center">
    <img src="Sphere.png" width="200"/>
    <h1 align="center"><b>Sphere</b></h1>
</p>
Sphere is a minimalistic language, designed to be simple to write, understand and utilize.
The language has a library named "High Level Macros" to further aid the programmer without manual memory manipulation.

### Example
```php
# this is a single-line comment

# create label str that sets the string's chars into their own addresses.
# and set the first to the 0th address (0th is the length. But the 1st char
# is the 1st (0x1) from 0th (0x0))
str = "Hello World!" @ 0x0

# starting function (otherwise, it will check for a function call within the file outside of all scopes)
main(): int {
    outln $str    # Print the values (from first to last address)
    outln str     # Print the variable-properties (label/label-properties)
}

Output:
 > Hello, World!
 > 
   Name: "str"
   Type: string
   Address: 0x0 (0)
   Length: 0xE (14)
```
### Official Modules
HLM (High Level Macros) - HLM is a macro collection that adds functions and keywords like `for` and `while` to the language to further ease the development for some peolpe, making Sphere feel like a high-level language, when in reality, it's very low-level. 


### Data Types
__Do Note:__ A name of an (official) external module will be in brackets `()` to indicate what module something exists in.


### Reference Operators
| Handle    | Description                   |
|-----------|---------------------------------------------------------------------------------------------------------------|
| `@<var>`  | Address/position of the specified Label *¹                                                                      |
| `$<var>`  | Value of the specified Lbael from its position to the length from said position.                                |
| `<var>`   | Returns/creates specified label about a segment of memory along side of how to treat it (labels can overlap |

### Instructions

| Command and `<arguments>` | Description                                                             |
|---------------------------|-------------------------------------------------------------------------|
| `up   <int>`              | Move up by the given amount. Default Value: 1                           |
| `down <int>`              | Move down by the given amount. Default Value: 1                         |
| `incr <int>`              | Increment current address by the given amount. Default value: 1         |
| `decr <int>`              | Decrement current address by the given amount. Default value: 1         |
| `func identifier (any)`   | Function with name with any arguments and argument types                |

<br>

## Support
This project is cross-platform. This being that the designed support of this language is x86 and ARM native. If the language lives a long life, it may indefinately receive RISC-V and/or CISC architecture support.  

### x86 Architecture
- [x] Linux
- [x] Windows
- [x] MacOS

### ARM Architecture
 - [ ] Android
 - [ ] iOS

## Indefinate Sphere FOSS 
 - IDE (SphereIDE)
 - VM (VirtualGlobe)
 - OS (RollOS)

<br>
<br>
<br>
``
¹ - In the language, these variable looking things are named as "Labels" because they label a certain segment of the memory, allowing the user and the computer to understand what and how to treat them.
```