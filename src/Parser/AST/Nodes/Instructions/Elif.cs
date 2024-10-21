namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Elif : Node
    {
        public Node Cond;
        public List<Node> Body;
        public Elif(Node cond, List<Node> body, string file, int line, int col) : base(file, line, col)
        {

            this.Cond = cond;
            this.Body = body;
        }

        public Elif(If i, string file, int line, int col) : base(file, line, col)
        {

            this.Cond = i.Cond;
            this.Body = i.Body;
        }
        public override string ToString() => $"else if ({Cond}) {{ {string.Join("\n    ", Body)} }} ";
    }
}