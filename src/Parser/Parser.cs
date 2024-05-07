using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser(string file, string contents)
{
    private IEnumerable<Token> tokens = new Lexer.Lexer(file, contents).Lex();
    private IEnumerator<Token> token;
    private Token? LastToken;
    private Token? ResultToken;
    private ExprNode? CurrNode;
    private int curr = 0;
    private Token? Peek() => token.Current;
    private bool IsAtEnd = true;
    private Token? Next()
    {
        LastToken = Peek();
        IsAtEnd = token.MoveNext();
        return Peek();
    }

    public Stack<ExprNode> Processed = new();
    public List<object> Previous = new();

    #region Expectation Functions
    public bool Is(TokenKind tok) => SafelyExpect(tok) != null;
    public bool IsCurrent(TokenKind tok) => tok == Peek()?.Kind;

    public Token Expect(TokenKind tok) => (Peek()?.Kind == tok ? Next() : throw new Exception($"Obtained \"{Peek()?.Kind}\" instead of \"{tok}\" | {Peek()?.Line}:{Peek()?.Column}"))!;
    public Token? SafelyExpect(TokenKind tok) => Peek()?.Kind == tok ? Next() : null;

    public bool Is(params TokenKind[] toks) => SafelyExpect(toks) != null;
    public bool IsCurrent(params TokenKind[] toks) => toks.All(x => x == Peek()?.Kind);
    public Token Expect(params TokenKind[] toks) => (toks.All(x => x == Peek()?.Kind) ? Next() : throw new Exception($"Obtained \"{Peek()?.Kind}\" instead of: [ {String.Join(", ", toks.Select(x => x))} ] | {Peek()?.Line}:{Peek()?.Column}"))!;
    public Token? SafelyExpect(params TokenKind[] toks) => toks.All(x => x == Peek()?.Kind) ? Next() : null;

    #endregion

    public IEnumerable<ExprNode> Parse()
    {
        token = tokens.GetEnumerator();
        while (token.MoveNext())
        {
            while (token.Current.Kind != TokenKind.EOL && token.Current.Kind != TokenKind.EOF) {
                CurrNode = this.ParseOne()!;
                // Utils.Outln($"[()] {CurrNode.ToString()}");
                if (!token.MoveNext()) break;
                if (CurrNode is Expressions.Execute ) {
                    yield return CurrNode!;
                    CurrNode = null;
                }
            }
            yield return CurrNode!;
        }
    }

    private ExprNode? ParseOne(Token? token = null)
    {

        var curr = token == null ? Peek()?.Kind : token.Kind;

        switch (curr)
        {
            case TokenKind.EOF:
                return new Expressions.EOF();
            case TokenKind.EOL:
                return new Expressions.EOL();

            case TokenKind.StringLit:
            case TokenKind.IntLit:
            case TokenKind.BoolLit:
            case TokenKind.HexLit:
                return new Expressions.Literal(Peek()!.Kind, Peek()!.Value);

            case TokenKind.Identifier:
                return new Expressions.Identifier(Peek()!.Value);

            case TokenKind.Equal:
            case TokenKind.Plus:
            case TokenKind.Slash:
            case TokenKind.Minus:
            case TokenKind.Star:
            case TokenKind.At:
            case TokenKind.Modulo:
            case TokenKind.And:
            case TokenKind.Or:
            case TokenKind.In:

            case TokenKind.GreaterEq:
            case TokenKind.LessEq:
            case TokenKind.Dot:
            case TokenKind.Colon:
            case TokenKind.DoubleEq:

            case TokenKind.Pipe:
                return ParseOperators();


            case TokenKind.LParen:
            case TokenKind.RParen:
            case TokenKind.LSquare:
            case TokenKind.RSquare:
            case TokenKind.LCurly:
            case TokenKind.RCurly:
            case TokenKind.Comma:
                break;

            case TokenKind.Out:
            case TokenKind.Outln:
            case TokenKind.Input:
            case TokenKind.Inputln:
            case TokenKind.PtrIncr:
            case TokenKind.PtrDecr:
            case TokenKind.Mov:
            case TokenKind.If:
            case TokenKind.Elif:
            case TokenKind.Else:
            case TokenKind.For:
                return new Expressions.Execute(ParseInstructions());

            case TokenKind.AtPrefix:
            case TokenKind.Dollar:
                Next();
                if (!IsCurrent(TokenKind.Identifier)) throw new Exception("Epected Identifier after prefix '$'");
                return new Expressions.Identifier(Peek()!.Value, new(LastToken!));

            default:
                var tok = Peek();
                string val = tok?.Value ?? "null",
                        type = tok?.Kind.ToString() ?? "null";

                throw new Exception($"[INTERNAL ERROR]: Unknown token: {type} ({val})");
        }
        return null;
    }
}
