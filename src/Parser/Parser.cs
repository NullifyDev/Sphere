using Sphere.Lexer;
using Sphere.Parsers.AST;
using Sphere.Types;

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
                if (CurrNode.GetType().DeclaringType!.Name == "Instructions")
                {
                    yield return CurrNode!;
                    CurrNode = null;
                }
            }

        }
        if (CurrNode != null) yield return CurrNode;
        else yield return new Expressions.EOF((token!.Current).File, (token!.Current).Line, (token!.Current).Column);
    }

    private Node? ParseLast()
    {
        var curr = LastToken;
        switch (LastToken!.Kind)
        {
            case TokenKind.EOF: return new Expressions.EOF(curr!.File, curr.Line, curr.Column);
            case TokenKind.EOL: return new Expressions.EOL(curr!.File, curr.Line, curr.Column);


            case TokenKind.DataType_String:
            case TokenKind.DataType_Int:
            case TokenKind.DataType_Bool:
            case TokenKind.DataType_Void:
                Next();
                return new Expressions.Type(LastToken);

            case TokenKind.StringLit:
            case TokenKind.IntLit:
            case TokenKind.BoolLit:
            case TokenKind.HexLit:
                var l = new Expressions.Literal(Peek()!.Kind, Peek()!.Value, curr!.File, curr.Line, curr.Column);
                Next();
                return l;

            case TokenKind.Identifier:
                var i = new Expressions.Identifier(Peek()!.Value, curr!.File, curr.Line, curr.Column);
                Next();
                return i;

            case TokenKind.Continue:
                Next();
                return new Instructions.Continue(curr!.File, curr.Line, curr.Column);

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
                var op = new Expressions.Operator(LastToken, null!, null!, curr!.File, curr.Line, curr.Column);
                if (op.OpType == TokenKind.Equal)
                {
                    if (op.Left is Expressions.Identifier && op.Right is Expressions.Literal)
                    {
                        Expressions.Identifier left = op.Left as Expressions.Identifier;
                        if (left.Literal == null)
                        {
                            Expressions.Identifier id = new(op.Left as Expressions.Identifier);
                            id.Literal = op.Right as Expressions.Literal;
                            return id;
                        }
                    }
                }
                if (op.OpType == TokenKind.Colon) { }

                return op;


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
                    return ParseSphere(curr!.File, curr.Line, curr.Column);
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
                return ParseInstructions(curr!.File, curr.Line, curr.Column);

            case TokenKind.AtPrefix:
            case TokenKind.Dollar:
                Peek();
                if (!IsCurrent(TokenKind.Identifier)) Utils.Error("Epected Identifier after prefix '$'", curr!.File, curr.Line, curr.Column);
                return new Expressions.Identifier(Peek()!.Value, new(LastToken), curr!.File, curr.Line, curr.Column);

            case TokenKind.SLComment:
                while(Next().Kind != TokenKind.EOL) ;
                break;

            default:
                var tok = Peek();
                string val = tok?.Value ?? "null",
                       type = tok?.Kind.ToString() ?? "null";

                Utils.InternalError(FailedProcedure.P, "Token", $"Unknown token: {type} ({val})", tok.File, tok.Line, tok.Column);
                break;
        }
        return null;
    }
    private Node? ParseOne(Token? token = null)
    {
        var curr = token ?? Peek();

        switch (curr.Kind)
        {

            case TokenKind.DataType_String:
            case TokenKind.DataType_Int:
            case TokenKind.DataType_Bool:
            case TokenKind.DataType_Void:
                Next();
                return new Expressions.Type(curr);

            case TokenKind.EOF:
                Next();
                return new Expressions.EOF(curr.File, curr.Line, curr.Column);
            case TokenKind.EOL:
                Next();
                return new Expressions.EOL(curr.File, curr.Line, curr.Column);

            case TokenKind.StringLit:
            case TokenKind.IntLit:
            case TokenKind.BoolLit:
                Next();
                return new Expressions.Literal(LastToken!.Kind, LastToken!.Value, curr.File, curr.Line, curr.Column);
                // Node lit = new Expressions.Literal(Peek()!.Kind, Peek()!.Value, curr.File, curr.Line, curr.Column);
                // Next();
                // return lit;

            case TokenKind.Identifier:
                string n = Peek()!.Value;
                Next();
                return new Expressions.Identifier(n, curr.File, curr.Line, curr.Column);

            case TokenKind.Continue:
                return new Instructions.Continue(curr.File, curr.Line, curr.Column);

            case TokenKind.Equal:
            case TokenKind.Plus:
            case TokenKind.PlusEq:
            case TokenKind.Slash:
            case TokenKind.SlashEq:
            case TokenKind.Minus:
            case TokenKind.MinusEq:
            case TokenKind.Star:
            case TokenKind.StarEq:
            case TokenKind.At:
            case TokenKind.Modulo:
            case TokenKind.And:
            case TokenKind.Or:
            case TokenKind.In:
            case TokenKind.Dot:

            case TokenKind.Greater:
            case TokenKind.GreaterEq:
            case TokenKind.Less:
            case TokenKind.LessEq:
            case TokenKind.Colon:
            case TokenKind.DoubleEq:

            case TokenKind.Pipe:
                return ParseOperators();

            case TokenKind.RParen:
                Next();
                break;

            // FUNCTION DELCARATION AND INVOKATION!
            case TokenKind.LParen:
                if (CurrNode is not Expressions.Identifier)
                {
                    Error.Add(new(
                        ErrorType.Compilation, (CurrNode as Node)!,
                        CurrNode == null ?
                            $"[Parser: LParen]: Expected Identifier but got null instead." :
                            $"[Parser: LParen]: Expected Identifier but got \"{CurrNode}\" instead."
                    ));
                    Error.Dump();
                }
                Expressions.Identifier name = (CurrNode as Expressions.Identifier)!;

                List<Node> parameters = new();
                if (Peek()?.Kind == TokenKind.LParen)
                {
                    Node? cn = null;
                    Next();
                    while (Peek()?.Kind != TokenKind.RParen)
                    {
                        cn = this.ParseOne();
                        if (cn is Expressions.Identifier)
                            CurrNode = cn;

                        if (Peek().Kind == TokenKind.Comma)
                        {
                            if (cn != null) parameters.Add(cn!);
                            Next();
                        }

                        if (LastToken?.Kind == TokenKind.RParen)
                        {
                            break;
                        }
                    }
                    if (cn != null) parameters.Add(cn);
                }

                TypeKind? ReturnType = null;
                if (Next().Kind == TokenKind.Colon)
                {
                    Node t = ParseOne(Next());
                    if (t == null)
                        Utils.InternalError(FailedProcedure.P, $"Function.ReturnType", $"returned Node of {name} is null", t.File, t.Line, t.Column);

                    ReturnType = t switch
                    {
                        Expressions.Identifier i => i.Type.Kind,
                        Expressions.Type dt => dt.Kind,
                    };

                    Peek();
                }
                if (Peek().Kind == TokenKind.EOL)
                    while (Peek().Kind == TokenKind.EOL) Next();
                if (Peek().Kind == TokenKind.LBrace)
                {
                    // Next();
                    return new Expressions.Function(
                        name,
                        ReturnType ?? TypeKind.Void,
                        parameters,
                        GetBody(),
                        curr.File, curr.Line, curr.Column
                    );
                }
                return new Instructions.Invoke(name, parameters, name.File, name.Line, name.Column);

            // while(Next().Kind == TokenKind.EOL) ;
            // while (node.MoveNext()) nKind.LBracket;

            case TokenKind.RBracket:
            case TokenKind.LBrace:
            case TokenKind.RBrace:
            case TokenKind.Comma:
                Next();
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
            case TokenKind.Up:
            case TokenKind.Down:
            case TokenKind.Mov:
            case TokenKind.If:
            case TokenKind.Elif:
            case TokenKind.Else:
            case TokenKind.For:
            case TokenKind.While:
            case TokenKind.Return:
                return ParseInstructions(curr.File, curr.Line, curr.Column);

            case TokenKind.LMLComment:
                while (Next().Kind != TokenKind.RMLComment);
                break;

            case TokenKind.AtPrefix:
            case TokenKind.Dollar:
                Next();

                Node id = new Expressions.Identifier(Peek()!.Value, new(LastToken), curr.File, curr.Line, curr.Column);

                if (LastToken.Kind == TokenKind.Dollar && Peek().Kind == TokenKind.AtPrefix) id = new Instructions.GetCurrAddressVal(curr.File, curr.Line, curr.Column);
                else if (!IsCurrent(TokenKind.Identifier)) Utils.Error($"Epected Identifier after prefix '{LastToken!.Value}'. Got {Peek()} Instead ", curr.File, curr.Line, curr.Column);

                Next();
                return id;

            case TokenKind.SLComment:
                while(Next().Kind != TokenKind.EOL) ;
                // Next();
                break;

            default:
                var tok = Peek();
                string val = tok?.Value ?? "null",
                       type = tok?.Kind.ToString() ?? "null";

                Utils.InternalError(FailedProcedure.P, "Token", $"Unknown token: {type} ({val})", tok.File, tok.Line, tok.Column);
                break;
        }
        return null;
    }
}