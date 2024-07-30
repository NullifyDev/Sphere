using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    public List<Node> GetBody()
    {
        CurrNode = null;
        Node curr;
        List<Node> nodes = new();
        if (Peek().Kind == TokenKind.Colon)
        {
            Next();
            while(Peek().Kind != TokenKind.EOL && Peek().Kind != TokenKind.EOF) {
                Token tok = Peek()!;
                while (tok.Kind == TokenKind.EOL) tok = Next()!;
                if (tok.Kind == TokenKind.LBrace) return this.GetBody();

                curr = this.ParseOne()!;

                if (curr is Expressions.EOF || curr is not Expressions.Identifier)
                    if (curr != null)nodes.Add(curr);
                
                if (curr != null)
                    CurrNode = curr;
            }
        }
        else if (Peek().Kind == TokenKind.LBrace)
        {
            while (Next()?.Kind == TokenKind.EOL) ;
            while (Peek()?.Kind != TokenKind.RBrace)
            {
                Token tok = Peek()!;
                while (tok.Kind == TokenKind.EOL) tok = Next()!;
                if (tok.Kind == TokenKind.RBrace) break;
                curr = this.ParseOne()!;

                if (curr is Expressions.EOF || curr is not Expressions.Identifier)
                    if (curr != null) nodes.Add(curr);
                
                if (curr != null)
                    CurrNode = curr;
            }
            
        }
        Next();
        return nodes;
    }
}