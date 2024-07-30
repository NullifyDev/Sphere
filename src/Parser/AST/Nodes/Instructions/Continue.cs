namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Continue : Node
    {
        public Continue(string file, int line, int col) : base(file, line, col)
        {
            base.Type = $"Instructions+{this.GetType().Name}";
        }
        public override string ToString() => $"continue";
    }
}