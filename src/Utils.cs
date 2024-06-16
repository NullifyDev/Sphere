using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphere;

using Sphere.Parsers.AST;

public static class Utils
{
    public static void Out(string msg = "") => Console.Write(msg);
    public static void Outln(string msg = "") => Console.WriteLine(msg);
    // public static void Error(string msg = "") => Outln($"[INTERNAL ERROR]: {msg}");
    public static object Error(string file, string? msg, int line, int column) => throw new Exception($"{file}({line}:{column}): {msg} ");
    public static void Error(string? msg) => throw new Exception($"{msg}");
    public static object InternalError(string file, string? msg, int line, int column) => throw new Exception($"[INTERNAL ERROR]: {msg} | Errored At: {file}: {line}:{column}");

    public static void PrintCode(List<Node> nodes, int depth = 0)
    {
        foreach (Node node in nodes)
        {
            if (node == null) continue;
            var n = node as Node;
            Console.SetCursorPosition(0, n.Line);
            switch (n.Type) {
                case "If":
                    var i = node as Instructions.If;
                    Utils.Out($"{new string(' ', depth * 4)}if {i.Cond} {{");
                    PrintCode(i.Body, depth+1);
                    Console.CursorLeft = 0;
                    Utils.Out($"\n{new string(' ', depth * 4)}}}");
                    break;
                case "Elif":
                    var Elif = node as Instructions.Elif;
                    Utils.Out($"{new string(' ', depth * 4)}elif {Elif.Cond} {{");
                    PrintCode(Elif.Body, depth+1);
                    Console.CursorLeft = 0;
                    Utils.Out($"\n{new string(' ', depth * 4)}}}");
                    break;
                case "Else":
                    var Else = node as Instructions.Else;
                    Utils.Out($"{new string(' ', depth * 4)}else {{");
                    PrintCode(Else.Body, depth+1);
                    Console.CursorLeft = 0;
                    Utils.Out($"\n{new string(' ', depth * 4)}}}");
                    break;
                case "Mov":
                    var Mov = node as Instructions.Mov;

                    if (Mov.Item == null) { 
                        Utils.Out($"\n{new string(' ', depth * 4)}mov [LastSelectedObject] {Mov.Amount}");
                        continue;
                    }
                    
                    Utils.Out($"\n{new string(' ', depth * 4)}mov {Mov.Item} {Mov.Amount}");
                    break;
                case "Incr":
                    var Incr = node as Instructions.Incr;

                    if (Incr.Item == null)
                    {
                        Utils.Out($"\n{new string(' ', depth * 4)}incr [LastSelectedObject] {Incr.Amount}");
                        continue;
                    }
                    Utils.Out($"\n{new string(' ', depth * 4)}incr {Incr.Item} {Incr.Amount}");
                    break;
                case "Decr":
                    var Decr = node as Instructions.Decr;

                    if (Decr.Item == null)
                    {
                        Utils.Out($"\n{new string(' ', depth * 4)}decr [LastSelectedObject] {Decr.Amount}");
                        continue;
                    }

                    Utils.Out($"\n{new string(' ', depth * 4)}decr {Decr.Item} {Decr.Amount}");
                    break;
                case "Out":
                    var Out = node as Instructions.Out;
                    Utils.Out($"\n{new string(' ', depth * 4)}{Out.ToString()}");
                    break;
                case "Outln":
                    var Outln = node as Instructions.Outln;
                    Utils.Out($"\n{new string(' ', depth * 4)}{Outln.ToString()}");
                    break;
                case "In":
                    var In = node as Instructions.In;
                    Utils.Out($"\n{new string(' ', depth * 4)}{In.ToString()}");
                    break;
                case "Inln":
                    var Inln = node as Instructions.Inln;
                    Utils.Out($"\n{new string(' ', depth * 4)}{Inln.ToString()}");
                    break;
                case "Continue":
                    var Continue = node as Instructions.Continue;
                    Utils.Out($"\n{new string(' ', depth * 4)}continue");
                    break;
                case "For":
                    var For = node as Instructions.For;
                    Utils.Out($"{new string(' ', depth * 4)}for {For.Start} {For.End} {For.Id} {{");
                    PrintCode(For.Body, depth+1);
                    Console.CursorLeft = n.Column;
                    Utils.Out($"{new string(' ', depth * 4)}}}");
                    break;
                case "Foreach":
                    var Foreach = node as Instructions.Foreach;
                    Utils.Out($"{new string(' ', depth * 4)}foreach {Foreach.In.Left} in {Foreach.In.Right} {{");
                    PrintCode(Foreach.Body, depth+1);
                    Utils.Out($"{new string(' ', depth * 4)}}}");
                    break;
                case "Function":
                    var Func = node as Instructions.Function;
                    Utils.Out($"{new string(' ', depth * 4)}{Func.Name} ({string.Join(", ", Func.Params)}): {Func.ReturnType} {{");
                    PrintCode(Func.Body, depth+1);
                    Utils.Out($"{new string(' ', depth * 4)}}}");
                    break;
                case "While":
                    var While = node as Instructions.While;

                    Utils.Out($"{new string(' ', depth * 4)}while {While.Condition} {{");
                    PrintCode(While.Body, depth+1);
                    Utils.Out($"{new string(' ', depth * 4)}}}");
                    break;
                case "Sphere":
                    var Sph = node as Instructions.Sphere;

                    Utils.Out($"{new string(' ', depth * 4)}!SPHERE {{");
                    PrintCode(Sph.Body, depth+1);
                    Utils.Out($"{new string(' ', depth * 4)}}}");
                    break;
                case "Sphereln":
                    var Sphereln = node as Instructions.Sphereln;
                    Utils.Out($"\n{new string(' ', depth * 4)}!SPHERE {Sphereln.Body}");
                    break;
                case "Return":
                    var Return = node as Instructions.Return;
                    Utils.Out($"{new string(' ', depth * 4)}ret {string.Join("\n    ", Return.Items)}");
                    break;
            }
        }
        
    }
}