<p align="center">
    <img src="Sphere.png" width="200"/>
    <h1 align="center"><b>Sphere</b></h1>
</p>

Sphere is a compact language designed for both software and Operating System Development via direct Raw-Memory-Manipulation <br>
The language syntax is designed to balance readability and writability by minimizing the amount of characters required to write while maintaining the self-explanatory nature of the code and namings. <br><br>
This language mainly uses freestanding C code for its Runtime. The runtime executable is very CPU Architecture amd OS specific. Therefore, the runtime executable is [going to be] available across mutliple Architectures and OSs.
<br>Sphere-written software [will] compile(s) into Sphere Binary Files (or `.sbf`s).  <br>
<br>
<br>
<p align="center">
    <h1 align="center"><b>THIS PROJECT IS UNDER ALPHA DEVELOPMENT</b></h1>
</p>
<p align="center">Any feature may change at any given point in time.</p>
<br>
<p align="center">Reader discression is advised</p>
<br>
<br>
<br>

# Examples
```rust
main(): int {
    hello: string = "Hello,"
    world: string = "World!"
    outln hello world 
    outln $"{hello}{world}"
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
 - do `dotnet publish --self-contained` and wait for it to finish
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
| Instructions and Arguments                | Description                                                                                                   |
|-------------------------------------------|---------------------------------------------------------------------------------------------------------------|
| `mov <object> <int>`                      | Move the object by signed number of addresses                                                                 | 
| `incr <int>`                              | Increment current address by the given amount                                                                 |
| `decr <int>`                              | Decrement current address by the given amount                                                                 |
| `<identifier>(): <DataType> {}`           | Function with name as string with one argument                                                                |
| `out <arsg>`                              | Print all arguments before EOL without line break.                                                            |
| `outln <args>`                            | Print all arguments before EOL with line break.                                                               |
| `if <Condition> { <instructions> }`       | Executes Instructions when condition returns true                                                             |
| `elif <Condition> { <instructions> }`     | Executes Instructions when condition returns true when primary condition is not met                           |
| `else if  <Condition> { <instructions> }` | Executes Instructions when condition returns true when primary condition is not met                           |
| `else { <instructions> }`                 | Executes Instructions when none of the Conditions were met                                                    |
| `for <start> <end> <identifier?> {}`         | Iterates from start value to the end value while iterator is less than or equal to end . Optional: Identifier |


# Sphere Execution File
```
entry <FileID/FileName>

[dependency]
./deps/path/to/file.sbf
./deps/path/to/directory
./deps/path/to/directory/*
./deps/path/to/directory/*/.../
./deps/path/to/directory/...*/
./deps/path/to/directory/sub.../

[blacklist]

```
|    Syntax    | Description                                                                                            |
|--------------|--------------------------------------------------------------------------------------------------------|
|    `./*`     | All files and folders                                                                                  |
|  `./*/.../`  | All files, folders and subdirectories (default depth: 1)                                               |
|  `./...*/`   | recursive subdirectory traversal - `*` can be any number for max depth (default: unlimited)            |
| `./sub...*/` | recursive subdirectory traversal under `sub` folder (max depth is `*`- default: unlimited)             |
|  `./*...*/`  | Recursively all files, folders and subdirectories at any depth (max depth is `*`- default: unlimited)  |



[Learn more (coming soon)](https://github.com/NullifyDev/Sphere)
<br><br>

### Support
Any operating systems that support LLVM and `clang` will be immediately supported.

### Platform Support 
 - [ ] Freestanding
   - [ ] x86
   - [ ] arm
   - [ ] RISC-V  
   <br>
 - [ ] Windows
   - [ ] x86
   - [ ] arm
   - [ ] RISC-V
   <br>
 - [ ] Linux
   - [ ] x86
   - [ ] arm
   - [ ] RISC-V
   <br>
 - [ ] MacOS
   - [ ] x86
   - [ ] arm
   <br>
 - [ ] Android
 - [ ] iOS

### Indefinate Software
Here are some of the software that are under consideration of development using Sphere as the language
 - [ ] Petroglyph IDE
 - [ ] Boulder Package Manager
 - [ ] RollOS (Runtime w/ Userspace)
 - [ ] Asciigine (Console/Terminal based Game Engine with Ascii graphics)
