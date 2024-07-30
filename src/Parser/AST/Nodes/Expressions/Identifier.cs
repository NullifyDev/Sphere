using Sphere.Lexer;

namespace Sphere.Parsers.AST;

public partial class Expressions
{
    public record Identifier : Node
    {
        public string Name;
        public Prefix? Prefix;
        public Identifier(string name, Prefix? prefix, string file, int line, int col) : base(file, line, col)
        {
            this.Name = name;
            this.Prefix = prefix;

            base.Type = $"Expressions+{this.GetType().Name}";
        }
        public Identifier(string name, string file, int line, int col) : base(file, line, col)
        {
            this.Name = name;
            this.Prefix = null;

            base.Type = $"Expressions+{this.GetType().Name}";
        }
        public Identifier(Token tok, Prefix? prefix = null) : base(tok.File, tok.Line, tok.Column)
        {
            this.Name = tok.Value;
            this.Prefix = prefix;

            base.Type = $"Expressions+{this.GetType().Name}";
        }
        public override string ToString() => this.Prefix == null ? Name.ToString() : $"{this.Prefix}{Name}";
    }
}