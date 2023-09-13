using System.Runtime.InteropServices.JavaScript;
using Qbe.AST;

namespace Qbe;

public static partial class Global
{
    public static List<Variable> Variables = new();
    public static List<Function> Functions = new(); 
    
    public static List<Token> Tokens = new();
    public static int curr = 0;
}