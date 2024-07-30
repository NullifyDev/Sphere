namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record GetCurrAddressVal : Node {
        public GetCurrAddressVal(string file, int line, int col) : base(file, line, col) {}
    }
}