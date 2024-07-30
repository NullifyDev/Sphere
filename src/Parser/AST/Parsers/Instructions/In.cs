using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{

    public Node ParseIn(string file, int line, int col)
    {
        Node[] args = GetInstArgs();
        if (args.Length == 1)
        {
            if (args[0] is not Expressions.Literal) throw new Exception($"Expected Literal but got {args[1].GetType().Name}");
            return new Instructions.In((args[0] as Expressions.Literal)!, file, line, col);
        }
        return new Instructions.In(null!, file, line, col);
    }
    
    public Node ParseInln(string file, int line, int col)
    {
        Node[] args = GetInstArgs();
        if (args.Length == 1)
        {
            if (args[0] is not Expressions.Literal) throw new Exception($"Expected Literal but got {args[1].GetType().Name}");
            return new Instructions.In((args[0] as Expressions.Literal)!, file, line, col);
        }
        return new Instructions.In(null!, file, line, col);
    }
}