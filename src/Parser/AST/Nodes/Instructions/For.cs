namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record For : Node
    {
        public Node Start, End;
        public Identifier? Id;
        public List<Node> Body;

        public For(Node Start, Node End, Identifier? Id, List<Node> Body, string file, int line, int col) : base(file, line, col)
        {
            this.Start = Start;
            this.End = End;
            this.Id = Id;
            this.Body = Body;
        }
        public override string ToString() => $"for {Start} {End} {Id} {{\n    {string.Join("\n    ", Body)}\n}}";
    }
}