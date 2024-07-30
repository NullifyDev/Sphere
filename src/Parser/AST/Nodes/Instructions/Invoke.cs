namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Invoke : Node
    {
        public Expressions.Identifier Target;
        public List<Node> Params;

        public Invoke(Expressions.Identifier target, List<Node> param, string file, int line, int column) : base(file, line, column)
        {
            this.Target = target;
            this.Params = param;
        }

        public Invoke(Expressions.Identifier target, string file, int line, int column) : base(file, line, column)
        {
            this.Target = target;
            this.Params = new();
        }
        public override string ToString() => $"{this.Target.Name}({string.Join(", ", this.Params)})";
    }
}