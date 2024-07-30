namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record If : Node
    {
        public Node Cond;
        public List<Node> Body;
        public If(Node cond, List<Node> body, string file, int line, int col) : base(file, line, col)
        {
            base.Type = $"Instructions+{this.GetType().Name}";

            this.Cond = cond;
            this.Body = body;
        }
        public override string ToString() => $"if ({Cond}) {{ {string.Join("\n    ", Body)} }} ";
    }
}