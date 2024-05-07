using Sphere.Lexer;

namespace Sphere.Parsers.AST
{
    public class Expressions
    {
        public record BinOp(ExprNode Left, Token Op, ExprNode Right) : ExprNode
        {
            public override string ToString() => $"{Left} {Op.Value} {Right}";
        }
        public record UnaryOp(Token Op, ExprNode Right) : ExprNode
        {
            public override string ToString() => $"{Op} {Right}";
        }
        public record Literal : ExprNode
        {
            public LiteralType Type;
            public object Value;
            public Literal(Token token)
            {
                this.Type = token.Kind switch {
                    TokenKind.StringLit => LiteralType.String,
                    TokenKind.IntLit    => LiteralType.Int,
                    TokenKind.BoolLit   => LiteralType.Boolean,
                    TokenKind.HexLit    => LiteralType.Hex,
                    _                   => throw new Exception($"Unkown Literal {token.Kind}")
                };
                this.Value = token.Value;
            }
            public Literal(TokenKind Type, object value)
            {
                this.Type = Type switch {
                    TokenKind.StringLit => LiteralType.String,
                    TokenKind.IntLit    => LiteralType.Int,
                    TokenKind.BoolLit   => LiteralType.Boolean,
                    TokenKind.HexLit    => LiteralType.Hex,
                    _                   => throw new Exception($"Unkown Literal {Type}")
                };
                this.Value = value;
            }
            public override string ToString() => this.Type switch {
                LiteralType.String    => $"\"{this.Value}\""   ?? "",
                LiteralType.Int       => this.Value.ToString() ?? "null",
                LiteralType.Boolean   => this.Value.ToString() ?? "null",
                LiteralType.Hex       => this.Value.ToString() ?? "null",
                _                     => throw new Exception($"Unrecognised Literal {this.Type}")
            };
        }
        public record Prefix(Token tok) : ExprNode
        {
            public override string ToString() => tok.Value;
        }
        public record class Identifier(string Name, Prefix? prefix = null) : ExprNode
        {
            public override string ToString() => prefix == null ? Name.ToString() : $"{prefix.tok.Value}{Name}";
        }
        public record Assign(string Name, ExprNode Value) : ExprNode
        {
            public override string ToString() => $"{Name}: {Value}";
        }
        public record Call(ExprNode Callee, List<ExprNode> Arguments) : ExprNode
        {
            public override string ToString() => $"{Callee}({string.Join(", ", Arguments)})";
        }
        public record Grouping(ExprNode Expr) : ExprNode
        {
            public override string ToString() => $"({Expr})";
        }
        public record Execute(InstNode inst) : ExprNode
        {
            public override string ToString() => $"{inst.ToString()}";
        }
        public record EOF : ExprNode;
        public record EOL : ExprNode;
    }
}

public enum LiteralType
{
    String, Int, Boolean, Hex
}