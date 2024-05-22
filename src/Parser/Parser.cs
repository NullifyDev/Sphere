using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser(string file, string contents)
{
    private IEnumerable<Token> tokens = new Lexer.Lexer(file, contents).Lex();
    private IEnumerator<Token>? token;
    private Token? LastToken;
    private ExprNode? CurrNode;
    private Token Peek() => token!.Current;
    private bool IsAtEnd = true;
    private Token Next()
    {
        LastToken = Peek();
        IsAtEnd = token!.MoveNext();
        return Peek();
    }

    public Stack<ExprNode> Processed = new();
    public List<object> Previous = new();

    #region Expectation Functions
    public bool Is(TokenKind tok) => SafelyExpect(tok) != null;
    public bool IsCurrent(TokenKind tok) => tok == Peek()?.Kind;

    public Token Expect(TokenKind tok) => (Peek()?.Kind == tok ? Next() : (Token)Utils.Error($"Obtained \"{Peek()?.Kind}\" instead of \"{tok}\"", Peek().File, Peek().Line, Peek().Column))!;
    public Token? SafelyExpect(TokenKind tok) => Peek()?.Kind == tok ? Next() : null;

    public bool Is(params TokenKind[] toks) => SafelyExpect(toks) != null;
    public bool IsCurrent(params TokenKind[] toks) => toks.All(x => x == Peek()?.Kind);
    public Token Expect(params TokenKind[] toks) => (toks.All(x => x == Peek()?.Kind) ? Next() : (Token)Utils.Error(Peek().File, $"Obtained \"{Peek()?.Kind}\" instead of: [ {String.Join(", ", toks.Select(x => x))} ]", Peek().Line, Peek().Column))!;
    public Token? SafelyExpect(params TokenKind[] toks) => toks.All(x => x == Peek()?.Kind) ? Next() : null;

    #endregion

    public IEnumerable<ExprNode> Parse()
    {
        token = tokens.GetEnumerator();
        while (token.MoveNext())
        {
            if (Peek()?.Kind == TokenKind.EOF) break;
            while (token.Current.Kind != TokenKind.EOL && token.Current.Kind != TokenKind.EOF)
            {
                CurrNode = this.ParseOne()!;
                if (!token.MoveNext()) goto eofFound;
                if (CurrNode is Expressions.Execute)
                {
                    yield return CurrNode!;
                    CurrNode = null;
                }
                
            }
            
        }
        eofFound: 
            if (CurrNode != null) yield return CurrNode;
            else yield return new Expressions.EOF();
    }

    private ExprNode? ParseOne(Token? token = null)
    {

        var curr = token == null ? Peek() : token;

        switch (curr.Kind)
        {
            case TokenKind.EOF: return new Expressions.EOF();
            case TokenKind.EOL: return new Expressions.EOL();

            case TokenKind.StringLit:
            case TokenKind.IntLit:
            case TokenKind.BoolLit:
            case TokenKind.HexLit:
                return new Expressions.Literal(Peek()!.Kind, Peek()!.Value, curr.File, curr.Line, curr.Column);

            case TokenKind.Identifier:
                return new Expressions.Identifier(Peek()!.Value, curr.File, curr.Line, curr.Column);

            case TokenKind.Continue:
                return new Expressions.Execute("continue", new Instructions.Continue());

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
            case TokenKind.Dot:

            case TokenKind.GreaterEq:
            case TokenKind.LessEq:
            case TokenKind.Colon:
            case TokenKind.DoubleEq:

            case TokenKind.Pipe:
                return ParseOperators();

            case TokenKind.LParen:
                if (CurrNode is not Expressions.Identifier) Utils.Error(this.file, "Expected Identifier", curr.Line, curr.Column);
                Expressions.Identifier name = (CurrNode as Expressions.Identifier)!;
                CurrNode = null;
                List<ExprNode> parameters = new();
                if (Next().Kind != TokenKind.RParen) {
                    while (Peek().Kind != TokenKind.RParen)
                    {
                        if (Peek().Kind == TokenKind.Comma)
                            parameters.Add(CurrNode!);

                        CurrNode = this.ParseOne();
                        if (!this.token.MoveNext()) break;
                    }
                    parameters.Add(CurrNode);
                }

                ExprNode ReturnType = new Expressions.Identifier("void", file, curr.Line, curr.Column);
                if (Next().Kind == TokenKind.Colon) {
                    Token t = Peek();
                    if (Next().Kind == TokenKind.Identifier)
                        ReturnType = this.ParseOne(Next());
                    else Utils.Error(this.file, "Expected return type after ':'", t.Line, t.Column);
                }

                // while(Next().Kind == TokenKind.EOL) ;

                return new Expressions.Execute("Function",
                    new Instructions.Function(name, ReturnType!, parameters, GetFunctionBody())
                );

            case TokenKind.RParen:
            case TokenKind.LBracket:
            case TokenKind.RBracket:
            case TokenKind.LBrace:
            case TokenKind.RBrace:
            case TokenKind.Comma:
                break;            

            case TokenKind.Bang:
                if (Next().Kind == TokenKind.Sphere) {
                    Next();
                    return new Expressions.Execute(
                        "Local Compilation Config (!SPHERE)",
                        ParseSphere()
                    );
                }
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
            case TokenKind.While:
                var inst = ParseInstructions();

                return new Expressions.Execute(inst.GetType().Name, inst);

            case TokenKind.AtPrefix:
            case TokenKind.Dollar:
                Next();
                if (!IsCurrent(TokenKind.Identifier)) Utils.Error("Epected Identifier after prefix '$'", curr.File, curr.Line, curr.Column);
                return new Expressions.Identifier(Peek()!.Value, new(LastToken!), curr.File, curr.Line, curr.Column);

            default:
                var tok = Peek();
                string val = tok?.Value ?? "null",
                       type = tok?.Kind.ToString() ?? "null";

                Utils.InternalError(tok.File, $"Unknown token: {type} ({val})", tok.Line, tok.Column);
                break;
        }
        return null;
    }
}