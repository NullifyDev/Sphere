using Sphere.Lexer;

namespace Sphere.Parsers.AST;

public partial class Expressions
{
    public record EOF : Node
    {
        public EOF(string file, int line, int col) : base(file, line, col) {}
        public override string ToString() => "EOF";
    }
}