using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseWhile(string file, int line, int col) => new Instructions.While(GetCondition(), GetBody($"{path}.While"), file, line, col);
}