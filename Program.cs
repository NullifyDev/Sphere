using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static Sphere.Utils;
using static Sphere.AST;
using Sphere.Compiler;
using System.Net;

namespace Sphere;

public class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        Console.OutputEncoding = Encoding.Unicode;
        
        if (args.Length <= 0) {
            Utils.Outln("Incorrect usage - no files provided. ");
            Utils.Outln("Correct usage: 'sphere run filename.spr'");
        }
        List<string> files = new();
        string instruction = "";
        string executable = "program";
        for(int i = 0; i < args.Length; i++) {
            if (args[i] == "run") {
                instruction = "run";
                continue;
            }

            if (args[i].StartsWith("dir="))
            {
                Config.ProjectDir = args[i].Remove(0, 4).StartsWith("./") ? $"{Directory.GetCurrentDirectory()}/{args[i].Remove(0, 6)}" : $"{args[i].Remove(0, 4)}";
                continue;
            }
            if (args[i].StartsWith("output=")) {
                executable = args[i].Remove(0, 7);
            }
            if (args[i].StartsWith("os=")) {
                Compiler.Config.Platform = args[i].Remove(0, 3) switch
                {
                    "windows" => PlatformID.Win32NT,
                    "linux" => PlatformID.Unix,
                    "macos" => PlatformID.MacOSX,
                    "freestanding" => null,
                    _ => null
                }; 
                continue;
            }
            
            if (args[i].StartsWith("arch=")) {
                Compiler.Config.Archictecture = args[i].Remove(0, 5) switch
                {
                    "i386" or "x32" => Architecture.X86,
                    "amd64" or "x64" => Architecture.X64,
                    _ => Architecture.X86
                };
                continue;
            }

            if (args[i].EndsWith(".spr")) { 
                files.Add(args[i]);
            }
        }

        switch(instruction) {
            case "run":
                string fileStr = "";
                for(int i = 0; i < files.Count; i++) {
                    fileStr += $"{Config.ProjectDir}/{files[i].Remove(files[i].Length-4, 4)}.c ";
                }
                Transpile(files);
            

                string outputFile = $"{Config.ProjectDir}/{executable}";
                string commandArgs = $"{fileStr} -o {outputFile}";

                System.Diagnostics.Process.Start("/usr/bin/gcc", $"{commandArgs}");
                System.Diagnostics.Process.Start("/usr/bin/chmod", $"+x {outputFile}");
                System.Diagnostics.Process.Start(outputFile);
                break;
        }
    }

    public static void Transpile(List<string> files) {
        for(int i = 0; i < files.Count(); i++) {
            new Sphere.Compiler.Transpiler(
                files[i],
                new Parser( 
                    files[i],
                    new Lexer(files[i], File.ReadAllText($"{Directory.GetCurrentDirectory()}/{files[i]}")).Lex()
                ).Parse().ToArray()
            ).Transpile();

            // new Pretty().Print(items.ToArray());
        }
        Code.Dump();
    }
}

class Pretty
{
    public void Print(Node[] node)
    {
        for (int a = 0; a < node.Count(); a++)
        {
            switch (node[a].Value)
            {
                case Instruction:
                    Instruction inst = (node[a].Value as Instruction)!;
                    Out($"{inst.Type}: ");
                    GetArgs(inst.Args);
                    break;
                
                case Variable:
                    Variable var = (node[a].Value as Variable)!;
                    Out($"{var.Type}: ");
                    GetArgs(var.Value!);
                    break;
                
                case Function:
                    Function func = (node[a].Value as Function)!;
                    Outln($"{func.Name}: {{");
                    GetFuncParam(func.Parameters);
                    GetFuncBody(func.Body);
                    Outln($"}}");
                    break;
            }
        }
    }

    void GetFuncParam(Dictionary<string, Variable> parameters)
    {
        foreach (var v in parameters)
        {
            Out($"    {v.Key}: ");
            GetVariable(v.Value.Value!);
            Outln();
        }
    }
    void GetVariable(object value)
    {
        switch (value)
        {
            case Token: Outln((value as Token)!.value); break;
            case Variable: 
                var val = (value as Variable)!.Value;
                Outln((val as string) + "\n");
                break;
            case Function:
                Function func = (value as Function)!;
                Out($"{func.Name}: ");
                GetFuncParam(func.Parameters);
                GetFuncBody(func.Body);
                break;
        }
    }

    void GetFuncBody(IEnumerable<Node> body)
    {
        foreach (var i in body)
        {
            switch (i.Value)
            {
                case Instruction:
                    Instruction inst = (i.Value as Instruction)!;
                    Out($"    {inst.Type}: ");
                    GetArgs(inst.Args);
                    break;
                
                case Variable:
                    Variable var = (i.Value as Variable)!;
                    Out($"{var.Type}: ");
                    GetArgs(var.Value!);
                    break;
                
                case Function:
                    Function func = (i.Value as Function)!;
                    Out($"{func.Name}: ");
                    GetFuncParam(func.Parameters);
                    GetFuncBody(func.Body);
                    break;
            }
        }
    }

    void GetArgs(object args)
    {
        switch (args)
        {
            case Token: Outln((args as Token)!.value); break;
            case Variable: 
                var val = ((args as Variable)!).Value;
                Outln((val as string)!);
                break;
        }
    }
}