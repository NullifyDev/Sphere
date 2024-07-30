using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseReturn(string file, int line, int col) => new Instructions.Return(file, line, col, GetInstArgs().ToList());
}