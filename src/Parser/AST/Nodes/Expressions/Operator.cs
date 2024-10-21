using Sphere.Lexer;

namespace Sphere.Parsers.AST;

public partial class Expressions
{
    public record Operator : Node
    {
        public Node? Left;
        public Node? Right;
        public string Value;
        public TokenKind OpType;
        public Operator(Token type, Node Left, Node? Right, string file, int line, int col) : base(file, line, col)
        {
            this.OpType = type.Kind;
            this.Value = type.Value;

            this.Left = Left;
            this.Right = Right;
        }
        public override string ToString() => $"{Left} {Value} {Right}";
    }
}