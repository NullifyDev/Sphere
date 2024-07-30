using System.ComponentModel.DataAnnotations.Schema;
using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseFor(string file, int line, int col)
    {
        Node? start = null;
        Node? end = null;
        Identifier? Id = null;
        Node? In = null;

        Next();
        if (Peek()?.Kind == TokenKind.IntLit)
            start = this.ParseOne()!;
        else if (Peek()?.Kind == TokenKind.Dollar || Peek()?.Kind == TokenKind.Identifier)
            CurrNode = this.ParseOne();

        if (Peek()?.Kind == TokenKind.IntLit)
            end = this.ParseOne()!;
        else if (Peek()?.Kind == TokenKind.Colon)
            In = this.ParseOne();

        if (In == null && Peek()?.Kind == TokenKind.Identifier)
            Id = this.ParseOne() as Identifier;
        else if (In != null)
            Utils.Outln("uh-huh...");

        while (Peek()?.Kind == TokenKind.EOL) Next();
        return In == null ?
            new Instructions.For(start!, end!, Id, GetBody(), file, line, col) :
            new Instructions.Foreach(In, GetBody(), file, line, col);
    }
}