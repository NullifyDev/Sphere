using Sphere.Compilation;
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

            if (node == null || node is EOL || node is EOF) break; {
                var res = Transpiler.GetObject(node);
                if (res == null) {
                    Error.Add(new ErrorObj(ErrorType.Syntax, node, $"{node} either doesn't exist or it isn't yet declared"));
                }
                args.Add(node!);
            }
        }
        return args.ToArray();
    }
}