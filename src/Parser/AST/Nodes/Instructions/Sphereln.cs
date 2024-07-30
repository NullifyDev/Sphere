namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Sphereln : Node
    {
        public Node Body;
        public Sphereln(Node Body, string file, int line, int col) : base(file, line, col)
        {
            this.Body = Body;
        }
        public override string ToString() => $"SPHERE: {string.Join("\n    ", Body)}";
    }
}