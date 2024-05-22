using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser
{
    public Expressions.BinOp ParseOperators()
    {
        if (CurrNode == null) Utils.Error(Peek().File, $"Left Hand Side of operator (\"{Peek()?.Value}\") is null", Peek().Line, Peek().Column);

        var Op = Peek()!;
        var LHS = CurrNode;
        var RHS = this.ParseOne(Next())!;

        return new Expressions.BinOp(Op, LHS, RHS);
    }
}