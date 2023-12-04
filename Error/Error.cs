using System.Linq;
using System.Diagnostics;

namespace Sphere;

public  enum ErrorType {
    Syntax, Compilation, Runtime
}

public class Error
{ 
    private static List<Error> List = new();
    
    private ErrorType Type;
    private string Message;
    private int? Line;
    private int? Column;
    private string File;

    public Error(ErrorType type, string file, string message, int? line, int? column)
    {
        this.Type = type;
        this.File = file;
        this.Message = message;
        this.Line = line;
        this.Column = column;
    }

    public static void Add(Error error) => List.Add(error);
    public static void DumpErrors() {
        foreach (var e in List)
        {
            Utils.LangErr(e.File, $"[{e.Type} - {e.File}:{e.Line}]: {e.Message}", e.Line);
        }
        Utils.Crash((""));
    }
}