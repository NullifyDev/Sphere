using Sphere.Lexer;

namespace Sphere.Parsers.AST;

public partial class Expressions
{

    public record Prefix : Node
    {
        public string Value;
        public TokenKind Type;
        public Prefix(Token tok, string file, int line, int col) : base(file, line, col)
        {
            this.Value = tok.Value;
            this.Type = tok.Kind;
        }
        public Prefix(Token tok) : base(tok.File, tok.Line, tok.Column)
        {
            this.Value = tok.Value;
            this.Type = tok.Kind;
        }
        public override string ToString() => this.Value;
    }
}