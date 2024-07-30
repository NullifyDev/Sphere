using Sphere.Lexer;

namespace Sphere.Parsers.AST;

public partial class Expressions
{
    public record Grouping : Node
    {
        public Node Expr;
        public Grouping(Node expr, string file, int line, int col) : base(file, line, col)
        {
            this.Expr = expr;
        }
        public override string ToString() => $"({Expr})";
    }
}