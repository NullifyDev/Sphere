namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Outln : Node
    {
        public IEnumerable<Node> Args;
        public Outln(IEnumerable<Node> args, string file, int line, int col) : base(file, line, col)
        {
            this.Args = args;
        }
        public override string ToString() => $"outln(\"{Type.GetStrFmt(this.Args.Select(x => x))}\", {string.Join(", ", this.Args.Select(x => x))});";

    }
}
