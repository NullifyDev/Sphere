using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseOut  (string file, int line, int col) => new Instructions.Out  (GetInstArgs(), file, line, col);
    public Node ParseOutln(string file, int line, int col) => new Instructions.Outln(GetInstArgs(), file, line, col);
}