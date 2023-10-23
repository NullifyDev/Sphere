using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using Qbe.AST;

namespace Qbe;

public static partial class Global
{
    public static List<Global.ASTNodes> ASTs = new();
    // public static List<VarNode> Variables = new();
    // public static List<Function> Functions = new(); 
    
    public static IEnumerable<Token> Tokens;
    public static int curr = 0;
}