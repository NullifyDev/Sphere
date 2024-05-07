using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    private ExprNode[] GetInstArgs()
    {
        List<ExprNode> args = new();
        while (Next()?.Kind != TokenKind.EOL && Peek()?.Kind != TokenKind.EOF)
        {
            var node = this.ParseOne(Peek());
            if (node is not Expressions.EOF && node is not Expressions.EOL)
                args.Add(node!);
        }
        return args.ToArray();
    }

    // mov <identifier> <int>     # move the object by <int> amount of addresses to the right (number can be negative)
    public InstNode ParseMov()
    {
        ExprNode[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) throw new Exception($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) throw new Exception($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) throw new Exception($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.Type != LiteralType.Int) throw new Exception($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Mov((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1)));
        }
        return new Instructions.Mov((args[0] as Expressions.Identifier)!, Convert.ToInt64(new Literal(TokenKind.IntLit, 1).Value));
    }

    // incr <identifier> <int>  # increment the object by <int>
    public InstNode ParseIncr()
    {
        ExprNode[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) throw new Exception($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) throw new Exception($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) throw new Exception($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.Type != LiteralType.Int) throw new Exception($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Incr((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1)));
        }
        return new Instructions.Incr((args[0] as Expressions.Identifier)!, Convert.ToInt64(new Literal(TokenKind.IntLit, 1).Value));
    }

    // decr <identifier> <int>  # decrement the object by <int>
    public InstNode ParseDecr()
    {
        ExprNode[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) throw new Exception($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) throw new Exception($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) throw new Exception($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.Type != LiteralType.Int) throw new Exception($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Decr((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1)));
        }
        return new Instructions.Decr((args[0] as Expressions.Identifier)!, Convert.ToInt64(new Literal(TokenKind.IntLit, 1).Value));
    }

    public InstNode ParseIn()
    {
        ExprNode[] args = GetInstArgs();
        if (args.Length == 1)
        {
            if (args[0] is not Expressions.Literal) throw new Exception($"Expected Literal but got {args[1].GetType().Name}");
            return new Instructions.In(args[0] as Expressions.Literal);
        }
        return new Instructions.In(null);
    }

    public InstNode ParseInln()
    {
        ExprNode[] args = GetInstArgs();
        if (args.Length == 1)
        {
            if (args[0] is not Expressions.Literal) throw new Exception($"Expected Literal but got {args[1].GetType().Name}");
            return new Instructions.In(args[0] as Expressions.Literal);
        }
        return new Instructions.In(null);
    }

    public InstNode ParseOut()
    {
        var args = GetInstArgs();
        return new Instructions.Out(args);
    }

    public InstNode ParseOutln()
    {
        var args = GetInstArgs();
        return new Instructions.Outln(args);
    }

    public InstNode ParseFor()
    {
        List<ExprNode> nodes = new();
        ExprNode? start = null;
        ExprNode? end = null;
        ExprNode? Id = null;

        // ExprNode? LastNode = null;
        ExprNode? In = null;

        if (Next()?.Kind == TokenKind.IntLit)
        {
            start = this.ParseOne()!;
        }
        else if (Peek()?.Kind == TokenKind.Dollar || Peek()?.Kind == TokenKind.Identifier)
        {
            CurrNode = this.ParseOne();
        }

        if (Next()?.Kind == TokenKind.IntLit)
        {
            end = this.ParseOne()!;
        }
        else if (Peek()?.Kind == TokenKind.Colon)
        {
            In = this.ParseOne();
        }

        if (In == null && Next()?.Kind == TokenKind.Identifier)
        {
            Id = this.ParseOne()!;
        } else if (In != null) { 
            // Next(); 
        }
        else {
            Utils.Error("what?");
        }

        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        while (token.MoveNext())
        {
            if (Peek()?.Kind == TokenKind.RCurly) break;
            while (this.token.MoveNext() && Peek().Kind != TokenKind.EOL && Peek().Kind != TokenKind.EOF)
            {
                CurrNode = this.ParseOne()!;
                // Utils.Outln($"[()] {CurrNode.ToString()}");
                if (!token.MoveNext()) break;
                if (CurrNode is Expressions.Execute)
                {
                    nodes.Add(CurrNode!);
                    CurrNode = null;
                }
            }
            nodes.Add(CurrNode!);
        }
        return In == null ? 
            new Instructions.For(start!, end!, Id, nodes) :
            new Instructions.For(In, nodes);
    }

    #region IfStatement

    public InstNode ParseIf()
    {
        Next();
        ExprNode Cond = this.ParseOne()!;
        CurrNode = Cond;
        while (Next()?.Kind != TokenKind.LCurly)
        {
            if (Peek()?.Kind == TokenKind.EOL) continue;
            Cond = this.ParseOne()!;
        }
        // Expect(new TokenKind[] { TokenKind.LCurly, TokenKind.Colon });
        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        ExprNode curr;
        List<ExprNode?> nodes = new();
        while (Peek()?.Kind != TokenKind.RCurly)
        {
            Token tok = Peek()!;
            if (tok.Kind == TokenKind.EOF || tok == null) continue;
            curr = this.ParseOne()!;

            if (curr is Expressions.EOL || curr is Expressions.EOF ||
                !(curr is Expressions.Identifier))
                nodes.Add(curr);

            if (!token.MoveNext()) break;
        }

        return new Instructions.If(Cond, nodes!);
    }

    public InstNode ParseElif()
    {
        Next();
        ExprNode Cond = this.ParseOne()!;
        CurrNode = Cond;
        while (Next()?.Kind != TokenKind.LCurly)
        {
            if (Peek()?.Kind == TokenKind.EOL) continue;
            Cond = this.ParseOne()!;
        }
        // Expect(new TokenKind[] { TokenKind.LCurly, TokenKind.Colon });
        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        ExprNode curr;
        List<ExprNode?> nodes = new();
        while (Peek()?.Kind != TokenKind.RCurly)
        {
            Token tok = Peek()!;
            if (tok.Kind == TokenKind.EOF || tok == null) continue;
            curr = this.ParseOne()!;

            if (curr is Expressions.EOL || curr is Expressions.EOF ||
                !(curr is Expressions.Identifier))
                nodes.Add(curr);

            if (!token.MoveNext()) break;
        }

        return new Instructions.Elif(Cond, nodes!);
    }

    public InstNode ParseElse()
    {
        Next();
        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        ExprNode curr;
        List<ExprNode?> nodes = new();
        while (Peek()?.Kind != TokenKind.RCurly)
        {
            Token tok = Peek()!;
            if (tok.Kind == TokenKind.EOF || tok == null) continue;
            curr = this.ParseOne()!;

            if (curr is Expressions.EOL || curr is Expressions.EOF ||
                !(curr is Expressions.Identifier))
                nodes.Add(curr);

            if (!token.MoveNext()) break;
        }

        return new Instructions.Else(nodes!);
    }

    #endregion

    public InstNode ParseInstructions() => Peek()?.Kind switch
    {
        TokenKind.Mov => ParseMov(),
        TokenKind.PtrIncr => ParseIncr(),
        TokenKind.PtrDecr => ParseDecr(),
        TokenKind.Outln => ParseOutln(),
        TokenKind.Out => ParseOut(),
        TokenKind.Input => ParseIn(),
        TokenKind.Inputln => ParseInln(),
        TokenKind.If => ParseIf(),
        TokenKind.Elif => ParseElif(),
        TokenKind.Else => ParseElse(),
        TokenKind.For => ParseFor(),
        _ => throw new Exception($"Unexpected token {Peek()?.Kind}")
    };
}
