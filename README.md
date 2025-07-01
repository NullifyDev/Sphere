<p align="center">
    <img src="Sphere.png" width="200"/>
    <h1 align="center"><b>Sphere</b></h1>
</p>

Sphere is a compact language designed with balance between memory control of C and memory safety via runtime-immutability first.<br>
<br>
**The Syntax** is designed to balance reading and writing by minimizing the amount of characters required to write while maintaining the self-explanatory nature of the code and namings. This is where you can do anything as the sky is the limit.<br>
<br>
**The Compilation** is deisgned to modify the code into a more memory-safe and efficient code while maintaining the end result by mitigating potential risks of memory corruption, to then convert into custom bytecode for the runtime. This is stricter and more limited as to what is possible, so it will try to replicate the exact behavior while making the bytecode safer to run.<br>
<br>
**The Runtime** is written in C and is designed to be freestanding with security mechanism for any undefined or uncoverred cases, conditions or behaviors. Due to this concept, the runtime is the __strictest__ part of Sphere as the possibilities are most limited here. This level requires stability and security. Which means all pointers stay on the object they were created on, never refer to anything that isnt of its type, never create nor change the type of the pointer, etc. With all that being said, the runtime has its own safety mechanisms in place to ensure that where most softawre crash, Sphere will try to recover and continue to roll on.<br>
<br>
This language uses "freestanding C"-written runtime. Therefore, it is very CPU Architecture and OS specific (List of curently supported CPU Architectures and OSs  are below).<br>
<br>
<br>
<br>
Sphere-written software [will] compile(s) into `.sbf` files.<br>
<br>
<br>
<p align="center">
    <h1 align="center"><b>THIS PROJECT IS UNDER ALPHA DEVELOPMENT</b></h1></p>
<p align="center">Any feature may change at any given point in time<br>Reader discression is advised</p><br>
<br>
<br>

# Example
```rust
main(): int {
    hello: string = "hello world"
    user: string = "nullifydev"
    outln "[terminal]: " hello
    outln "[$user]: give me $$5" // you can get "$" by doing "$$"

    /* placing objects or expressions in "(" and ")" concatenates
       the end result of the object or expression */
    outln "[terminal]: ok $(user)!" 
    ret 0
}

<# Output:
[terminal]: hello world
[nullifydev]: give me $5
[terminal]: ok nullifydev!

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


# Sphere Binary Executable file (SBE)
`sbe` files are human-readable link and execution files, planned to have their own conditions and conditional statements (coming soon).

Here is how .sef files will work
```
entry <FileIdOrFileName>

[dependency]
./deps/path/to/file.sbf
./deps/path/to/directory
./deps/path/to/directory/*
./deps/path/to/directory/*/.../
./deps/path/to/directory/...*/
./deps/path/to/directory/sub.../

[blacklist]
# to prevent loading

[whitelist]
# to allow loading
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
Any operating systems supported by LLVM and `clang` will immediately be supported.

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
   - [ ] x86 (old devices)
   - [ ] arm
   <br>
 - [ ] Android (ARM)
 - [ ] iOS (ARM)

### Indefinate Software
Here are some of the software that are under consideration of development using Sphere as the source code language
 - [ ] Petroglyph IDE
 - [ ] Boulder Package Manager
 - [ ] RollOS (Runtime w/ Userspace)
 - [ ] Asciigine (Console/Terminal based Game Engine with Ascii graphics)


## Where to find me
To keep yourself up to date with my language, please head over to [r/ProgrammingLanguages](https://discord.gg/MWjdg6GXRn) discord server within the `#sphere` channel under `PROJECTS Q-Y` Category<br>
I also have a [Discord community server](https://discord.gg/FQuqQTXhhm)
