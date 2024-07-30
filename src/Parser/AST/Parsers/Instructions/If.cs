using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseIf  (string file, int line, int col) => new Instructions.If  (GetCondition(), GetBody()!, file, line, col);
    public Node ParseElif(string file, int line, int col) => new Instructions.Elif(GetCondition(), GetBody()!, file, line, col);
    public Node ParseElse(string file, int line, int col)
    {
        while (token!.MoveNext() && Peek()?.Kind == TokenKind.EOL);
        return Peek()?.Kind == TokenKind.If ? ParseElif(file, line, col) : new Instructions.Else(GetBody(), file, line, col);
    }
}