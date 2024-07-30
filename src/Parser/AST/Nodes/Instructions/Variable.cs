namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Variable : Node
    {
        public string Name;
        public Literal Value;

        public Variable(string name, Literal value, string file, int line, int column) : base(file, line, column)
        {
            this.Name = name;
            this.Value = value;
        }

        public override string ToString() => $"{this.Value.Type} {this.Name} = {this.Value.Value};";
    }
}