using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser
{
    public Expressions.Operator ParseOperators()
    {
        if (CurrNode == null) Utils.Error(Peek().File, $"Left Hand Side of operator (\"{Peek()?.Value}\") is null", Peek().Line, Peek().Column);

        var Op = Peek()!;
        Node LHS = CurrNode!;
        Node? RHS = null;
        if (!IsEither(new TokenKind[] { TokenKind.And, TokenKind.Or }))
            RHS = this.ParseOne(Next())!;
        else Next();

        return new Expressions.Operator(Op, LHS, RHS, Op.File, Op.Line, Op.Column);
    }
}