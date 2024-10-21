namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Sphere : Node
    {
        public List<Node> Body = new();
        public Sphere(List<Node> Body, string file, int line, int col) : base(file, line, col)
        {
            this.Body = Body;
        }
        public override string ToString() => $"SPHERE: {{ \n    {string.Join("\n    ", Body ?? new List<Node>(0).AsEnumerable())} \n}}\n";
    }
}