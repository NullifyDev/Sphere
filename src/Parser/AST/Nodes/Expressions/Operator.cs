using System.Transactions;
using Sphere.Compilation;
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
        public Operator(Token type, Node? Left, Node? Right, string file, int line, int col) : base(file, line, col)
        {
            this.OpType = type.Kind;
            this.Value = type.Value;

            this.Left = Left;
            this.Right = Right;

            if (OpType == TokenKind.Colon) {
                if (this.Right is Type && this.Left is Identifier) {
                    var v = (this.Left as Identifier);
                    v.Literal = new Literal((this.Right as Type).Kind, null, this.Right.File, this.Right.Line, this.Right.Column);
                    v.Literal.Type = this.Right as Type;
                    Transpiler.Variables.Add(v);
                } 
            } 

        }
        public override string ToString() => $"{Left} {Value} {Right}";
    }
}