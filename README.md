<p align="center">
    <img src="Sphere.png" width="200"/>
    <h1 align="center"><b>Sphere</b></h1>
</p>

Sphere is a compact language designed for both software and Operating System Development via direct Raw-Memory-Manipulation <br>
The language syntax is designed to balance readability and writability by minimizing the amount of characters required to write while maintaining the self-explanatory nature of the code and namings. <br><br>
This language mainly uses freestanding C/C++ code for its Runtime. The runtime executable is very CPU Architecture specific. Therefore, the runtime executable is [going to be] available across mutliple architectures such as x86, arm and RISC-V. 
<br>Sphere-written software [will] compile(s) into Sphere Binary Files (or `.sbf`s).  <br>
<br>
<br>
<p align="center">
    <h1 align="center"><b>THIS PROJECT IS UNDER ALPHA DEVELOPMENT</b></h1>
</p>
<p align="center">Any feature may change at any given point in time.</p>
<br>
<br>
<br>

# Examples
```ps1
main(): int {
    hello: string = "Hello,"
    world: string = "World!"
    outln hello world
    outln hello+world 
}

<# Output:
Hello, World!
Hello,World!
#>
```

# How to Compile
This project is written with the latest .Net release So install that before continuing. Not sure if you have it? check by doing `dotnet --version`.
```
 - Git Clone this repo with the `-b prototype` argument and head into the cloned project.
 - do `dotnet restore` to get all the possible dependencies installed
 - do `dotnet ppublish --self-contained` and wait for it to finish
 - now run your new executable by running `<exeNameWithoutDotExe> <SphereFile>`
```

### Identifier Prefixes
| Handle      | Description                                       |
|-------------|---------------------------------------------------|
| `$<object>` | Gets the information of the specified object      |
| `@<object>` | Gets or sets the address of the specified object. |
| `<object>`  | Gets or sets the value of the object              |

[Learn more (coming soon)](https://github.com/NullifyDev/Sphere)
<br><br>
### Instructions
| Instructions and Arguments                | Description                                                                         |
|-------------------------------------------|-------------------------------------------------------------------------------------|
| `mov <object> <int>`                      | Move the object by signed number of addresses                                       | 
| `incr <int>`                              | Increment current address by the given amount                                       |
| `decr <int>`                              | Decrement current address by the given amount                                       |
| `<identifier>(): <DataType> {}`           | Function with name as string with one argument                                      |
| `out <arsg>`                              | Print all arguments before EOL without line break.                                  |
| `outln <args>`                            | Print all arguments before EOL with line break.                                     |
| `if <Condition> { <instructions> }`       | Executes Instructions when condition returns true                                   |
| `elif <Condition> { <instructions> }`     | Executes Instructions when condition returns true when primary condition is not met |
| `else if  <Condition> { <instructions> }` | Executes Instructions when condition returns true when primary condition is not met |
| `else { <instructions> }`                 | Executes Instructions when none of the Conditions were met                           |
| `for <start> <end> <identifier>`          | 


[Learn more (coming soon)](https://github.com/NullifyDev/Sphere)
<br><br>

### Support
Any operating systems that support LLVM and `clang` will be immediately supported.

### CPU Architectures Implemented
 - [ ] x86
 - [ ] arm
 - [ ] RISC-V


### Indefinate Softawre
Here are some of the software that are under consideration of development using Sphere as the language
 - [ ] Petroglyph IDE
 - [ ] Boulder Package Manager
 - [ ] RollOS
 - [ ] Asciigine (Console/Terminal based Game Engine with Ascii graphics)
