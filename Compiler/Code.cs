using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Sphere.Compiler;
using static Sphere.Compiler.Config;
namespace Sphere;

public class Code
{
    public static List<Code> Codes = new();
    public List<AST.Pragma> Pragmas = new();
    public string Name;
    public string Content;

    public Code(List<AST.Pragma> pragmas, string name, string content)
    {
        this.Pragmas = pragmas;
        this.Name = name;
        this.Content = content;
    }

    public Code(string name, string content)
    {
        this.Name = name;
        this.Content = content;
    }

    public static void Add(List<AST.Pragma> pragma, string name, string content) {
        Codes.Add(new Code(pragma, name, content));
    }

    public static void Add(string name, string content) {
        Codes.Add(new Code(name, content));
    }

    public static void Dump()
    {
        File.WriteAllText($"{Config.ProjectDir}/base.h", "#ifndef BASE_H\n#define BASE_H\n\n#include <stdio.h>\n\n#endif");
        foreach (var c in Codes)
        {
            File.WriteAllText($"{Config.ProjectDir}/{c.Name.Remove(c.Name.Length - 4, 4)}.c", Config.Platform switch
            {
                PlatformID.Win32NT => "#include \"base.h\"\n\n",
                PlatformID.Unix => $"#include \"{Config.ProjectDir}/base.h\"\n\n",
                // PlatformID.MacOSX => "#ifndef BASE_H\n#define BASE_H\n\n#include <stdio.h>\n\n#endif",
                _ => "#ifndef BASE_H\n#define BASE_H\n\n#include <stdio.h>\n\n#endif"
            }  + c.Content);
        }
    }
}

public enum TargetPlatform

{
    Windows, Linux, MacOS, Freestanding
}

public enum TargetArch
{
    i386, x64
}