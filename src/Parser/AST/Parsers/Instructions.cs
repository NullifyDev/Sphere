using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseInstructions(string file, int line, int col) => Peek()?.Kind switch
    {
        TokenKind.Mov => ParseMov(file, line, col),
        TokenKind.Up => ParseUp(file, line, col),
        TokenKind.Down => ParseDown(file, line, col),
        TokenKind.PtrIncr => ParseIncr(file, line, col),
        TokenKind.PtrDecr => ParseDecr(file, line, col),
        TokenKind.Outln => ParseOutln(file, line, col),
        TokenKind.Out => ParseOut(file, line, col),
        TokenKind.Input => ParseIn(file, line, col),
        TokenKind.Inputln => ParseInln(file, line, col),
        TokenKind.If => ParseIf(file, line, col),
        TokenKind.Elif => ParseElif(file, line, col),
        TokenKind.Else => ParseElse(file, line, col),
        TokenKind.For => ParseFor(file, line, col),
        TokenKind.Sphere => ParseSphere(file, line, col),
        TokenKind.While => ParseWhile(file, line, col),
        TokenKind.Return => ParseReturn(file, line, col),
        _ => throw new Exception($"Unexpected instruction token {Peek()?.Kind}")
    };
}