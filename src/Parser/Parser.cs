using Sphere.Lexer;
using Sphere.Parsers.AST;

namespace Sphere.Parsers;

public partial record Parser(string file)
{
    private IEnumerable<Token> tokens = new Lexer.Lexer(file).Lex();
    private IEnumerator<Token>? token;
    private Token? LastToken;
    private Node? CurrNode;
    private Token Peek() => token!.Current;
    private bool IsAtEnd = true;
    private Token Next()
    {
        LastToken = Peek();
        IsAtEnd = token!.MoveNext();
        return Peek();
    }

    public Stack<Node> Processed = new();
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

    public IEnumerable<Node> Parse()
    {
        token = tokens.GetEnumerator();
        while (token.MoveNext())
        {
            if (Peek()?.Kind == TokenKind.EOF) break;
            if (Peek()?.Kind == TokenKind.EOL) continue;
            while (token.Current.Kind != TokenKind.EOL && token.Current.Kind != TokenKind.EOF)
            {
                CurrNode = this.ParseOne()!;
                if (CurrNode == null) break;
                if (CurrNode.GetType().DeclaringType.Name == "Instructions")
                {
                    yield return CurrNode!;
                    CurrNode = null;
                }
                else Next();
            }

        }
    eofFound:
        if (CurrNode != null) yield return CurrNode;
        else yield return new Expressions.EOF((token!.Current).File, (token!.Current).Line, (token!.Current).Column);
    }

    private Node? ParseLast()
    {
        var curr = LastToken;
        switch (LastToken.Kind)
        {
            case TokenKind.EOF: return new Expressions.EOF(curr.File, curr.Line, curr.Column);
            case TokenKind.EOL: return new Expressions.EOL(curr.File, curr.Line, curr.Column);

            case TokenKind.StringLit:
            case TokenKind.IntLit:
            case TokenKind.BoolLit:
            case TokenKind.HexLit:
                return new Expressions.Literal(Peek()!.Kind, Peek()!.Value, curr.File, curr.Line, curr.Column);

            case TokenKind.Identifier:
                return new Expressions.Identifier(Peek()!.Value, curr.File, curr.Line, curr.Column);

            case TokenKind.Continue:
                return new Instructions.Continue(curr.File, curr.Line, curr.Column);

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
                return new Expressions.Operator(LastToken, null, null, curr.File, curr.Line, curr.Column);

            case TokenKind.LParen:
            case TokenKind.RParen:
            case TokenKind.LBracket:
            case TokenKind.RBracket:
            case TokenKind.LBrace:
            case TokenKind.RBrace:
            case TokenKind.Comma:
                break;

            case TokenKind.Bang:
                if (Peek().Kind == TokenKind.Sphere)
                    return ParseSphere(curr.File, curr.Line, curr.Column);
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
                ParseInstructions(curr.File, curr.Line, curr.Column);
                break;

            case TokenKind.AtPrefix:
            case TokenKind.Dollar:
                Peek();
                if (!IsCurrent(TokenKind.Identifier)) Utils.Error("Epected Identifier after prefix '$'", curr.File, curr.Line, curr.Column);
                return new Expressions.Identifier(Peek()!.Value, new(LastToken!), curr.File, curr.Line, curr.Column);

            default:
                var tok = Peek();
                string val = tok?.Value ?? "null",
                       type = tok?.Kind.ToString() ?? "null";

                Utils.InternalError(tok!.File, $"Unknown token: {type} ({val})", tok.Line, tok.Column);
                break;
        }
        return null;
    }

    private Node? ParseOne(Token? token = null)
    {

        var curr = token == null ? Peek() : token;

        switch (curr.Kind)
        {
            case TokenKind.EOF: return new Expressions.EOF(curr.File, curr.Line, curr.Column);
            case TokenKind.EOL: return new Expressions.EOL(curr.File, curr.Line, curr.Column);

            case TokenKind.StringLit:
            case TokenKind.IntLit:
            case TokenKind.BoolLit:
            case TokenKind.HexLit:
                return new Expressions.Literal(Peek()!.Kind, Peek()!.Value, curr.File, curr.Line, curr.Column);

            case TokenKind.Identifier:
                return new Expressions.Identifier(Peek()!.Value, curr.File, curr.Line, curr.Column);

            case TokenKind.Continue:
                return new Instructions.Continue(curr.File, curr.Line, curr.Column);

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
                List<Node> parameters = new();
                if (Next().Kind != TokenKind.RParen)
                {
                    while (Peek().Kind != TokenKind.RParen)
                    {
                        if (Peek().Kind == TokenKind.Comma)
                            parameters.Add(CurrNode!);

                        CurrNode = this.ParseOne();
                        if (!this.token.MoveNext()) break;
                    }

                    if (CurrNode != null) 
                        parameters.Add(CurrNode!);
                }

                Node ReturnType = new Expressions.Identifier("void", curr.File, curr.Line, curr.Column);
                if (Next().Kind == TokenKind.Colon)
                {
                    Token t = Next();
                    if (t.Kind == TokenKind.Identifier)
                        ReturnType = this.ParseOne(Peek());
                    else Utils.Error(this.file, "Expected return type after ':'", t.Line, t.Column);
                }

                // while(Next().Kind == TokenKind.EOL) ;

                return new Instructions.Function(name, ReturnType!, parameters, GetBody(), curr.File, curr.Line, curr.Column);
            
            case TokenKind.RMLComment:
            case TokenKind.RParen:
            case TokenKind.LBracket:
            case TokenKind.RBracket:
            case TokenKind.LBrace:
            case TokenKind.RBrace:
            case TokenKind.Comma:
                break;

            case TokenKind.Bang:
                if (Next().Kind == TokenKind.Sphere)
                {
                    Next();
                    return ParseSphere(curr.File, curr.Line, curr.Column);
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
            case TokenKind.Return:
                return ParseInstructions(curr.File, curr.Line, curr.Column);

            case TokenKind.LMLComment:
                while(Next().Kind != TokenKind.RMLComment) ;
                // Next();
                break;
                // return new Expressions.Comment(curr.File, curr.Line, curr.Column);

            case TokenKind.AtPrefix:
            case TokenKind.Dollar:
                Next();
                if (!IsCurrent(TokenKind.Identifier)) Utils.Error("Epected Identifier after prefix '$'", curr.File, curr.Line, curr.Column);
                return new Expressions.Identifier(Peek()!.Value, new(LastToken!), curr.File, curr.Line, curr.Column);

            default:
                var tok = Peek();
                string val = tok?.Value ?? "null",
                       type = tok?.Kind.ToString() ?? "null";

                Utils.InternalError(tok!.File, $"Unknown token: {type} ({val})", tok.Line, tok.Column);
                break;
        }
        return null;
    }
}