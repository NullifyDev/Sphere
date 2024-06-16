using Sphere.Lexer;

namespace Sphere.Parsers.AST;

public class Expressions
{
    public record Operator : Node
    {
        public Node Left, Right;
        public string Value;
        public TokenKind OpType;
        public Operator(Token type, Node Left, Node Right, string file, int line, int col) : base(file, line, col)
        {
            this.OpType = type.Kind;
            this.Value = type.Value;

            this.Left = Left;
            this.Right = Right;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"({Left} {Value} {Right})";
    }
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

            base.Type = this.GetType().Name;
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

            base.Type = this.GetType().Name;
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
    public record Prefix : Node
    {
        public string Value;
        public TokenKind Type;
        public Prefix(Token tok, string file, int line, int col) : base(file, line, col)
        {
            this.Value = tok.Value;
            this.Type = tok.Kind;

            base.Type = this.GetType().Name;
        }
        public Prefix(Token tok) : base(tok.File, tok.Line, tok.Column) {
            this.Value = tok.Value;
            this.Type = tok.Kind;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => this.Value;
    }
    public record Identifier : Node
    {
        public string Name;
        public Prefix? Prefix;
        public Identifier(string name, Prefix? prefix, string file, int line, int col) : base(file, line, col)
        {
            this.Name = name;
            this.Prefix = prefix;

            base.Type = this.GetType().Name;
        }
        public Identifier(string name, string file, int line, int col) : base(file, line, col)
        {
            this.Name = name;
            this.Prefix = null;

            base.Type = this.GetType().Name;
        }
        public Identifier(Token tok, Prefix? prefix = null) : base(tok.File, tok.Line, tok.Column)
        {
            this.Name = tok.Value;
            this.Prefix = prefix;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => this.Prefix == null ? Name.ToString() : $"{this.Prefix}{Name}";
    }
    public record Call : Node
    {
        Expressions.Identifier Target;
        List<Node> Arguments;
        public Call(Expressions.Identifier target, List<Node> arguments, string file, int line, int col) : base(file, line, col)
        {
            this.Target = target;
            this.Arguments = arguments;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"{Target}({string.Join(", ", Arguments)})";
    }

    public record Grouping : Node
    {
        public Node Expr;
        public Grouping(Node expr, string file, int line, int col) : base(file, line, col) {
            this.Expr = expr;
        } 
        public override string ToString() => $"({Expr})";
    }
    public record Comment : Node
    {
        public Comment(string file, int line, int col) : base(file, line, col)
        {
            base.Type = this.GetType().Name;
        }
        public override string ToString() => "Comment";
    }
    public record EOF : Node
    {
        public EOF(string file, int line, int col) : base(file, line, col) {
            base.Type = this.GetType().Name;
        }
        public override string ToString() => "";
    }
    public record EOL : Node
    {
        public EOL(string file, int line, int col) : base(file, line, col) {
            base.Type = this.GetType().Name;
        }
        public override string ToString() => "";
    }
}

public enum LiteralType
{
    String, Int, Boolean, Hex
}