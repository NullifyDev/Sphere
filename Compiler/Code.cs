using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Sphere;

public class Code
{
    public static List<Code> Codes = new();
    public string name;
    public string content;
    
    // Compiler-specific variables
    public static PlatformID? Platform = Environment.OSVersion.Platform;
    public static Architecture Archictecture = RuntimeInformation.OSArchitecture;
    public static string ProjectDir = Directory.GetCurrentDirectory();
    
    public Code(string name, string content)
    {
        this.name = name;
        this.content = content;
    }

    public static void Add(string name, string content) => Codes.Add(new Code(name, content));
    public static void Dump() {
        File.WriteAllText($"{Directory.GetCurrentDirectory()}/output/base.h", "#ifndef BASE_H\n#define BASE_H\n\n#include <stdio.h>\n\n#endif");
        foreach (var c in Codes)
        {
            Utils.Outln(c.content);
            File.WriteAllText($"{Directory.GetCurrentDirectory()}/output/{c.name.Remove(c.name.Length - 4, 4)}.c", $"#include \"{Directory.GetCurrentDirectory()}\\base.h\"\n\n" + c.content);
        }
    }
}

public enum TargetPlatform {
    Windows, Linux, MacOS, Freestanding
}

public enum TargetArch
{
    i386, x64
}