using Sphere.Lexer;

namespace Sphere.Parsers.AST;

public partial class Expressions
{

    public record Literal : Node
    {
        public LiteralType LitType;
        public object Value;
        public Literal(Token token, string file, int line, int col) : base(file, line, col)
        {
            this.LitType = token.Kind switch
            {
                TokenKind.StringLit => LiteralType.String,
                TokenKind.IntLit => LiteralType.Int,
                TokenKind.BoolLit => LiteralType.Boolean,
                TokenKind.HexLit => LiteralType.Hex,
                _ => (LiteralType)Utils.InternalError($"Unkown Literal {token.Kind}", this.File, this.Line, this.Column)
            };
            this.Value = token.Value;

            base.Type = $"Expressions+{this.GetType().Name}";
        }
        public Literal(TokenKind type, object value, string file, int line, int col) : base(file, line, col)
        {
            this.LitType = type switch
            {
                TokenKind.StringLit => LiteralType.String,
                TokenKind.IntLit => LiteralType.Int,
                TokenKind.BoolLit => LiteralType.Boolean,
                TokenKind.HexLit => LiteralType.Hex,
                _ => (LiteralType)Utils.InternalError($"Unkown Literal {Type}", this.File, this.Line, this.Column)
            };
            this.Value = value;

            base.Type = $"Expressions+{this.GetType().Name}";
        }
        public override string ToString() => this.LitType switch
        {
            LiteralType.String => $"\"{this.Value.ToString()}\"" ?? "",
            LiteralType.Int => this.Value.ToString() ?? "null",
            LiteralType.Boolean => this.Value.ToString() ?? "null",
            LiteralType.Hex => this.Value.ToString() ?? "null",
            _ => (string)Utils.InternalError($"Unrecognised Literal {this.LitType.ToString()}", this.File, this.Line, this.Column)
        };
    }
}

public enum LiteralType
{
    String, Int, Boolean, Hex
}