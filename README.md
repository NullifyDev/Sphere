<p align="center">
    <img src="Sphere.png" width="200"/>
    <h1 align="center"><b>Sphere</b></h1>
</p>
Sphere is a minimalistic language, designed to be simple to write, understand and utilize.
The language has a library named "High Level Macros" to further aid the programmer without manual memory manipulation.

### Official Modules
HLM (High Level Macros) - HLM is a macro collection that adds functions and keywords like `for` and `while` to the language to further ease the development for some peolpe, making Sphere feel like a high-level language, when in reality, it's very low-level. 


### Data Types
__Do Note:__ A name of an (official) external module will be in brackets `()` to indicate what module something exists in.


### Reference Operators
| Handle    | Description                   |
|-----------|-------------------------------|
| `@<var>`  | Address of the given variable |
| `$<var>`  | Value of the given variable   |

### Instructions

| Command and `<arguments>` | Description                                                             |
|---------------------------|-------------------------------------------------------------------------|
| `up <bin>`                | Move up by the given amount. Default Value: 1                           |
| `down <bin>`              | Move down by the given amount. Default Value: 1                         |
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
