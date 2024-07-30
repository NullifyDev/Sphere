using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser
{
    public bool Is(TokenKind tok) => SafelyExpect(tok) != null;
    public bool IsCurrent(TokenKind tok) => tok == Peek()?.Kind;

    public Token Expect(TokenKind tok) => (Peek()?.Kind == tok ? Next() : (Token)Utils.Error($"Obtained \"{Peek()?.Kind}\" instead of \"{tok}\"", Peek().File, Peek().Line, Peek().Column))!;
    public Token? SafelyExpect(TokenKind tok) => Peek()?.Kind == tok ? Next() : null;

    public bool Is(params TokenKind[] toks) => SafelyExpect(toks) != null;
    public bool IsEither(params TokenKind[] toks) => toks.Any(x => x == Peek()?.Kind);
    public bool IsCurrent(params TokenKind[] toks) => toks.All(x => x == Peek()?.Kind);
    public Token Expect(params TokenKind[] toks) => (toks.All(x => x == Peek()?.Kind) ? Next() : (Token)Utils.Error(Peek().File, $"Obtained \"{Peek()?.Kind}\" instead of: [ {String.Join(", ", toks.Select(x => x))} ]", Peek().Line, Peek().Column))!;
    public Token? SafelyExpect(params TokenKind[] toks) => toks.All(x => x == Peek()?.Kind) ? Next() : null;
}