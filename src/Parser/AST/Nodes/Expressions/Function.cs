namespace Sphere.Parsers.AST;

using Sphere.Types;

public partial class Expressions
{
    public record Function : Node
    {
        public Identifier Name;
        public Type Type;
        public List<Node> Params, Body;

        public Function(Identifier name, Type returnType, List<Node> Params, List<Node> Body, string file, int line, int col) : base(file, line, col)
        {
            this.Name = name;
            this.Type = returnType ?? new Type(TypeKind.Void, file, line, col);
            this.Params = Params;
            this.Body = Body;
        }
        public override string ToString() => $"{this.Name}({string.Join(", ", this.Params)}): {this.Type} {{\n    {string.Join("\n    ", this.Body ?? new List<object>(0).AsEnumerable())}\n}}";
    }
}