namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Else : Node
    {
        public List<Node> Body;
        public Else(List<Node> body, string file, int line, int col) : base(file, line, col)
        {
            this.Body = body ?? new();
        }
        public override string ToString() => $"else {{ {string.Join("\n     ", Body)} }}";
    }
}