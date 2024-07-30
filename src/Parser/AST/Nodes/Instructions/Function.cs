namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Function : Node
    {
        public Expressions.Identifier Name;
        public Node ReturnType;
        public List<Node> Params, Body;

        public Function(Expressions.Identifier name, Node ReturnType, List<Node> Params, List<Node> Body, string file, int line, int col)
            : base(file, line, col)
        {
            this.Name = name;
            this.ReturnType = ReturnType;
            this.Params = Params;
            this.Body = Body;

            base.Type = $"Instructions+{this.GetType().Name}";
        }
        public override string ToString() => $"{this.Name}({string.Join(", ", this.Params)}): {this.ReturnType} {{\n    {string.Join("\n    ", this.Body ?? new List<object>(0).AsEnumerable())}\n}}";
    }
}