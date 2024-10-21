using Sphere.Lexer;

namespace Sphere.Parsers.AST;

using Sphere.Types;

public partial class Expressions
{
    public record Identifier : Node
    {
        public string Name;
        public Expressions.Type Type;
        public Prefix? Prefix;
        public Literal? Literal;

        public Identifier(Identifier id, string? file = "", int? line = 0, int? col = 0) : base("", 0, 0)
        {
            this.Name = id.Name;
            this.Prefix = id.Prefix;
            this.Literal = id.Literal;

            base.File = id.File;
            base.Line = id.Line;
            base.Column = id.Column;
        }

        public Identifier(string name, Prefix? prefix, string file, int line, int col) : base(file, line, col)
        {
            this.Name = name;
            this.Prefix = prefix;
        }
        public Identifier(string name, string file, int line, int col) : base(file, line, col)
        {
            this.Name = name;
            this.Prefix = null;
        }
        public Identifier(Token tok, Prefix? prefix = null) : base(tok.File, tok.Line, tok.Column)
        {
            this.Name = tok.Value;
            this.Prefix = prefix;
        }
        public override string ToString() => this.Prefix == null ? Name.ToString() : $"{this.Prefix}{Name}";

        public void SetValue(Expressions.Literal lit)
        {
            if (this.Type == lit.Type)
                this.Literal = lit;
        }
    }
}