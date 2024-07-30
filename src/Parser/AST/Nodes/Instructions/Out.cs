namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Out : Node
    {
        public IEnumerable<Node> Args;
        public Out(IEnumerable<Node> args, string file, int line, int col) : base(file, line, col)
        {
            this.Args = args;

            base.Type = $"Instructions+{this.GetType().Name}";
        }
        public override string ToString() => $"out({string.Join(", ", Args ?? new List<object>(0).AsEnumerable())}) ];";
    }
}