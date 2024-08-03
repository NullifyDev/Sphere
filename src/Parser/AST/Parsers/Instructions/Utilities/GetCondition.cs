using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node GetCondition(bool exitOnGate = false)
    {
        if (!exitOnGate)
            Next();
        Node Cond = null!;
        while (!IsEither(new TokenKind[] { TokenKind.Colon, TokenKind.LBrace }))
        {
            if (exitOnGate && IsEither(new TokenKind[] {TokenKind.And, TokenKind.Or}))
                return Cond ?? CurrNode!;

            Node n = this.ParseOne()!;
            if (n == null) continue;
            if (n is Operator)
            {
                var o = n as Operator;
                var cn = CurrNode as Operator;
                var c = Cond as Operator;
                if (o.OpType == TokenKind.And || o.OpType == TokenKind.Or)
                {
                    if (o.Right == null)
                    {
                        o.Right = this.GetCondition(true);
                        CurrNode = o;
                        if (IsEither(new TokenKind[] { TokenKind.And, TokenKind.Or })) { 
                            o = ParseOne(token.Current) as Operator;
                            o.Right = this.GetCondition(true);
                            n = o;
                        }
                    }
                }
            }
            CurrNode = n;
        }
        return CurrNode!;
    }
}
