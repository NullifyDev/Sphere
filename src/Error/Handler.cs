using System.Text;
using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere;

public enum ErrorType {
    INTERNAL,
    Syntax,
    Compilation,
    Warning
}

public record struct ErrorObj {
    public ErrorType Type;
    public object Token;
    public string Message;

    public ErrorObj(ErrorType type, Token token, string message)
    {
        this.Type = type;
        this.Token = token;
    }

    public ErrorObj(ErrorType type, Node node, string message) {
        this.Type = type;
        this.Token = node;
        this.Message = message;
    }

    public int GetLine() => this.Token is Node ? (this.Token as Node)!.Line : (this.Token as Token)!.Line;
    public int GetColumn() => this.Token is Node ? (this.Token as Node)!.Column : (this.Token as Token)!.Column;
}

public record struct WarningObj(Token Token, string Message);

public static class Error {
    public static List<ErrorObj> Errors = new List<ErrorObj>();
    
    public static void Add(ErrorObj error) => Errors.Add(error);
    public static void Dump() {
        foreach(var e in Errors) {
            Error.Print(e.Type, e.Token, e.Message);
        }
        Environment.Exit(1);
    }

    public static void Print(ErrorType type, object token, string? msg)
    {
        StringBuilder message = new();
        Token t = null;
        if (token is Node) {
            Node n = token as Node;
            t = new(TokenKind.ERROR, n.File, n.ToString(), n.Line, n.Column);
        } else t = token as Token;

        string[] file = File.ReadAllLines(t.File);
        message.Append(
            type == ErrorType.INTERNAL
                ? $"\u001b[31m[{type} ERROR]: {t.File} ({t.Line}:{t.Column}): {msg}\n"
                : $"\u001b[31m[{type} Error]: {t.File} ({t.Line}:{t.Column}): {msg}\n"
        );

        string border = $"\u001b[0m\u001b[31m{new string('▔', message.Length)}\n";
        message.Append(border);

        int i = 9 + t.Line.ToString().Length-1;
        message.Append($"  {t.Line} │ ");
        
        message.Append($"{file[t.Line-1]
            .Insert(t.Column, "\u001b[41m")
            .Insert(t.Column + t.Value.Length+5, "\u001b[0m\u001b[31m")}\n\n"
        );

        message.Append(border);

        Utils.Outln(message.ToString());
        Warning.Dump();
        Console.Write("\x1b[0m");
    }
}

public static class Warning {
    private static List<WarningObj> Warnings = new List<WarningObj>();

    public static void Add(WarningObj warning) => Warnings.Add(warning);
    public static void Dump() {
        foreach (var w in Warnings)
            Warning.Print(w.Token, w.Message);
    }

    public static List<WarningObj> GetWarnings() => Warnings;
    public static void Print(Token token, string? msg)
    {
        StringBuilder message = new();
        
        message.Append("\x1b[38;2;200;2000;0m ");

        string m = $"[Warning]: {token.File}({token.Line}:{token.Column}): {msg} ";
        message.Append(m);
        message.Append(new string('▔', m.Length));
        message.Append("\n");
        message.Append($"  {token.Line} │ {File.ReadAllLines(token.File)[token.Line - 1]}")
            .Insert(token.Column, "\x1b[48;2;200;200;0m")
            .Insert(token.Column + token.Value.Length -1, "\x1b[0m\x1b[38;2;200;2000;0m");
        message.Append("\n\n");
        message.Append(new string('▔', m.Length));
        message.Append("\x1b[0m");
        
        Utils.Outln(message.ToString());
    }
}
