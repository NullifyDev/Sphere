using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser
{
    public Expressions.BinOp ParseOperators()
    {
        if (CurrNode == null) throw new Exception($"Left Hand Side of operator (\"{Peek()?.Value}\") is null");
        var Op = Peek()!;
        var LHS = CurrNode;
        var RHS = this.ParseOne(Next())!;
        // System.Console.WriteLine($"{LHS} {Op} {RHS}");
        return new Expressions.BinOp(LHS, Op, RHS);
    }
}