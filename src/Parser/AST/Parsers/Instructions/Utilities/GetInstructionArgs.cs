using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    private Node[] GetInstArgs()
    {
        // Node? lastNode = null;
        List<Node> args = new();
        bool cond = true;
        Next();
        while (Peek().Kind != TokenKind.EOF && Peek().Kind != TokenKind.EOL && Peek().Kind != TokenKind.RBrace)
        {
            var node = this.ParseOne(Peek());

            if (node == null || node is EOL || node is EOF) break;
            args.Add(node!);
        }
        return args.ToArray();
    }
}