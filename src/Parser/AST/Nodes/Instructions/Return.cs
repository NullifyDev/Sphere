namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Return : Node
    {
        public List<Node> Items;
        public Return(string file, int line, int col, List<Node> items) : base(file, line, col)
        {
            this.Items = items ?? new();
        }
        public override string ToString() => $"return {string.Join("\n    ", Items)}";
    }
}