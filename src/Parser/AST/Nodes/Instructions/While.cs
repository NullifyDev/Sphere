namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record While : Node
    {
        public Node Condition;
        public List<Node> Body;

        public While(Node condition, List<Node> body, string file, int line, int col) : base(file, line, col)
        {
            this.Condition = condition;
            this.Body = body;
        }

        public override string ToString() => $"while {Condition} {{\n    {string.Join(", ", Body)}\n}}";
    }
}