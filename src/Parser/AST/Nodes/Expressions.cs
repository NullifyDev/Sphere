using Sphere.Lexer;

namespace Sphere.Parsers.AST
{
    public class Expressions
    {
        public record BinOp : ExprNode
        {
            public Token Op;
            public ExprNode Left, Right;
            public BinOp(Token Op, ExprNode Left, ExprNode Right)
            {
                this.Op = Op;
                this.Left = Left;
                this.Right = Right;
                this.File = Op.File;
                this.Line = Op.Line;
                this.Column = Op.Column;
            }
            public override string ToString() => $"{Left} {Op.Value} {Right}";
        }
        public record UnaryOp(Token Op, ExprNode Right, string file, int line, int column) : ExprNode
        {
            public override string ToString() => $"{Op} {Right}";
        }
        public record Literal : ExprNode
        {
            public LiteralType Type;
            public object Value;
            public Literal(Token token)
            {
                this.File = token.File;
                this.Line = token.Line;
                this.Column = token.Column;
                this.Type = token.Kind switch
                {
                    TokenKind.StringLit => LiteralType.String,
                    TokenKind.IntLit => LiteralType.Int,
                    TokenKind.BoolLit => LiteralType.Boolean,
                    TokenKind.HexLit => LiteralType.Hex,
                    _ => (LiteralType)Utils.InternalError($"Unkown Literal {token.Kind}", this.File, this.Line, this.Column)
                };
                this.Value = token.Value;
            }
            public Literal(TokenKind Type, object value, string file, int line, int column)
            {
                this.File = file;
                this.Line = line;
                this.Column = column;
                this.Type = Type switch
                {
                    TokenKind.StringLit => LiteralType.String,
                    TokenKind.IntLit => LiteralType.Int,
                    TokenKind.BoolLit => LiteralType.Boolean,
                    TokenKind.HexLit => LiteralType.Hex,
                    _ => (LiteralType)Utils.InternalError($"Unkown Literal {Type}", this.File, this.Line, this.Column)
                };
                this.Value = value;
            }
            public override string ToString() => this.Type switch
            {
                LiteralType.String => $"\"{this.Value.ToString()}\"" ?? "",
                LiteralType.Int => this.Value.ToString() ?? "null",
                LiteralType.Boolean => this.Value.ToString() ?? "null",
                LiteralType.Hex => this.Value.ToString() ?? "null",
                _ => (string)Utils.InternalError($"Unrecognised Literal {this.Type}", this.File, this.Line, this.Column)
            };
        }
        public record Prefix : ExprNode
        {
            public string Value;
            public TokenKind Type;
            public Prefix(Token tok)
            {
                this.File = tok.File;
                this.Line = tok.Line;
                this.Column = tok.Column;
                this.Value = tok.Value;
                this.Type = tok.Kind;
            }
            public override string ToString() => this.Value;
        }
        public record class Identifier : ExprNode
        {
            public string Name;
            public Prefix? Prefix;
            public Identifier(string name, Prefix? prefix, string file, int line, int column)
            {
                this.File = file;
                this.Line = line;
                this.Column = column;

                this.Name = name;
                this.Prefix = prefix;
            }
            public Identifier(string name, string file, int line, int column)
            {
                this.File = file;
                this.Line = line;
                this.Column = column;

                this.Name = name;
                this.Prefix = null;
            }
            public override string ToString() => this.Prefix == null ? Name.ToString() : $"{this.Prefix}{Name}";
        }
        public record Call : ExprNode
        {
            Expressions.Identifier Target;
            List<ExprNode> Arguments;
            public Call(Expressions.Identifier target, List<ExprNode> arguments, string file, int line, int column)
            {
                this.Target = target;
                this.Arguments = arguments;
                this.File = file;
                this.Line = line;
                this.Column = column;
            }
            public override string ToString() => $"{Target}({string.Join(", ", Arguments)})";
        }

        public record Grouping(ExprNode Expr) : ExprNode
        {
            public override string ToString() => $"({Expr})";
        }

        public record CodeBlock
        {
            public List<ExprNode> Block;
            public CodeBlock(List<ExprNode>? CodeBlock = null)
            {
                this.Block = CodeBlock ?? new();
            }
            public void Add(ExprNode node) => Block.Add(node);
            public override string ToString() => $"{string.Join("\n    ", Block ?? new())}\n";
        }

        public record Execute : ExprNode
        {
            public string Name;
            public InstNode Instruction;
            public Execute(string Name, InstNode Instruction)
            {
                this.Name = Name;
                this.Instruction = Instruction;
                this.File = Instruction.File;
                this.Line = Instruction.Line;
                this.Column = Instruction.Column;
            }
            public override string ToString() => $"{Instruction.ToString()}";
        }
        public record EOF : ExprNode
        {
            public override string ToString() => "";
        };
        public record EOL : ExprNode
        {
            public override string ToString() => "";
        };
    }
}

public enum LiteralType
{
    String, Int, Boolean, Hex
}