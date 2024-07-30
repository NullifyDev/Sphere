using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseSphere(string file, int line, int col)
    {
        Node curr;
        List<Node?> nodes = new();
        if (Peek()?.Kind == TokenKind.LBrace)
        {
            while (token!.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
            while (Peek()?.Kind != TokenKind.RBrace)
            {
                Token tok = Peek()!;
                if (tok.Kind == TokenKind.EOF || tok == null) continue;
                curr = this.ParseOne()!;

                if (curr is Expressions.EOL || curr is Expressions.EOF)
                    nodes.Add(CurrNode);
                else CurrNode = curr;
                if (!token.MoveNext()) break;
            }
            return new Instructions.Sphere(nodes!, file, line, col);
        }
        else
        {
            while (Peek()?.Kind != TokenKind.EOL && Peek()?.Kind != TokenKind.EOF)
            {
                Token tok = Peek()!;
                if (tok.Kind == TokenKind.EOF || tok == null) continue;
                curr = this.ParseOne()!;

                if (curr is Expressions.EOL || curr is Expressions.EOF)
                    nodes.Add(CurrNode);
                else CurrNode = curr;

                if (!token!.MoveNext()) break;
            }
            return new Instructions.Sphereln(CurrNode!, file, line, col);
        }

        // return new Instructions.Sphere(new(), file, line, col);
    }
}