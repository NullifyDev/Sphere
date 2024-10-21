namespace Sphere;

using System.Linq.Expressions;
using Sphere.Parsers.AST;

public static class Utils
{
    public static void Out(string msg = "") => Console.Write(msg);
    public static void Outln(string msg = "") => Console.WriteLine(msg);
    public static object Error(string file, string? msg, int line, int column) => throw new Exception($"{file}({line}:{column}): {msg} ");
    public static void ErrorLang(ErrorType type, string? msg, string file, int line, int column)
    {
        if (type == ErrorType.INTERNAL) Utils.Outln($"[{type} ERROR]: {file}({line}:{column}): {msg} ");
        else Utils.Outln($"[{type} Error]: {file}({line}:{column}): {msg} ");
    }
    public static void WarningLang(string file, string? msg, int line, int column) => Utils.Outln($"[Warning]: {file}({line}:{column}): {msg} ");

    public static string SetForeground(int r, int g, int b) => $"\x1b[38;2;{r};{g};{b}m";
    public static string SetBackground(int r, int g, int b) => $"\x1b;48;2;{r};{g};{b}m";
    public static string ResetForeground() => $"\x1b;38;0m";
    public static string ResetBackground() => $"\x1b;38;0m";

    public static void Error(string? msg) => throw new Exception($"{msg}");

    public static object InternalError(FailedProcedure proc, string obj, string? msg, string file, int line, int column) => throw new Exception($"[INTERNAL ERROR @ {file}]: {msg} | i{proc}{obj}l{line}c{column}");


    public static void PrintCode(List<Node> nodes, int depth = 0)
    {
        foreach (Node node in nodes)
        {
            if (node == null) continue;
            var n = node as Node;
            Console.SetCursorPosition(0, n.Line);
            switch (n)
            {
                case Instructions.If i:
                    Utils.Out($"{new string(' ', depth * 4)}if {i!.Cond} {{");
                    PrintCode(i!.Body, depth + 1);
                    Console.CursorLeft = 0;
                    Utils.Out($"\n{new string(' ', depth * 4)}}}");
                    break;
                case Instructions.Elif Elif:
                    Utils.Out($"{new string(' ', depth * 4)}elif {Elif!.Cond} {{");
                    PrintCode(Elif!.Body, depth + 1);
                    Console.CursorLeft = 0;
                    Utils.Out($"\n{new string(' ', depth * 4)}}}");
                    break;
                case Instructions.Else Else:
                    Utils.Out($"{new string(' ', depth * 4)}else {{");
                    PrintCode(Else!.Body, depth + 1);
                    Console.CursorLeft = 0;
                    Utils.Out($"\n{new string(' ', depth * 4)}}}");
                    break;
                case Instructions.Mov Mov:
                    if (Mov!.Item == null)
                    {
                        Utils.Out($"\n{new string(' ', depth * 4)}mov [LastSelectedObject] {Mov!.Amount}");
                        continue;
                    }
                    Utils.Out($"\n{new string(' ', depth * 4)}mov {Mov.Item} {Mov.Amount}");
                    break;
                case Instructions.Incr Incr:
                    if (Incr!.Item == null)
                    {
                        Utils.Out($"\n{new string(' ', depth * 4)}incr [LastSelectedObject] {Incr!.Amount}");
                        continue;
                    }
                    Utils.Out($"\n{new string(' ', depth * 4)}incr {Incr.Item} {Incr.Amount}");
                    break;
                case Instructions.Decr Decr:
                    if (Decr!.Item == null)
                    {
                        Out($"\n{new string(' ', depth * 4)}decr [LastSelectedObject] {Decr!.Amount}");
                        continue;
                    }

                    Out($"\n{new string(' ', depth * 4)}decr {Decr.Item} {Decr.Amount}");
                    break;
                case Instructions.Out Out:
                    Utils.Out($"\n{new string(' ', depth * 4)}{Out!.ToString()}");
                    break;
                case Instructions.Outln Outln:
                    Out($"\n{new string(' ', depth * 4)}{Outln!.ToString()}");
                    break;
                case Instructions.In In:
                    Out($"\n{new string(' ', depth * 4)}{In!.ToString()}");
                    break;
                case Instructions.Inln Inln:
                    Out($"\n{new string(' ', depth * 4)}{Inln!.ToString()}");
                    break;
                case Instructions.Continue Continue:
                    Utils.Out($"\n{new string(' ', depth * 4)}continue");
                    break;
                case Instructions.For For:
                    Out($"{new string(' ', depth * 4)}for {For!.Start} {For!.End} {For!.Id} {{");
                    PrintCode(For!.Body, depth + 1);
                    Console.CursorLeft = n.Column;
                    Out($"{new string(' ', depth * 4)}}}");
                    break;
                case Instructions.Foreach Foreach:
                    Out($"{new string(' ', depth * 4)}foreach {Foreach!.In!.Left} in {Foreach!.In!.Right} {{");
                    PrintCode(Foreach!.Body, depth + 1);
                    Out($"{new string(' ', depth * 4)}}}");
                    break;
                case Expressions.Function Func:
                    Utils.Out($"{new string(' ', depth * 4)}{Func!.Name} ({string.Join(", ", Func!.Params)}): {Func.Type} {{");
                    PrintCode(Func.Body, depth + 1);
                    Utils.Out($"{new string(' ', depth * 4)}}}");
                    break;
                case Instructions.While While:

                    Utils.Out($"{new string(' ', depth * 4)}while {While!.Condition} {{");
                    PrintCode(While.Body, depth + 1);
                    Utils.Out($"{new string(' ', depth * 4)}}}");
                    break;
                case Instructions.Sphere Sph:
                    Utils.Out($"{new string(' ', depth * 4)}!SPHERE {{");
                    PrintCode(Sph!.Body, depth + 1);
                    Utils.Out($"{new string(' ', depth * 4)}}}");
                    break;

                case Instructions.Sphereln Sphereln:
                    Utils.Out($"\n{new string(' ', depth * 4)}!SPHERE {Sphereln!.Body}");
                    break;
                case Instructions.Return Return:
                    Utils.Out($"{new string(' ', depth * 4)}ret {string.Join("\n    ", Return!.Items)}");
                    break;
            }
        }



    }
}


public enum FailedProcedure
{
    R,  // Runtime
    T,  // Transpiler
    P,  // Parser
    L,  // Lexer
}