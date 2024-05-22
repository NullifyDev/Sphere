using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    private ExprNode[] GetInstArgs()
    {
        // ExprNode? lastNode = null;
        List<ExprNode> args = new();
        while (Next()?.Kind != TokenKind.EOL && Peek()?.Kind != TokenKind.EOF)
        {
            var node = this.ParseOne(Peek());
            if (node is Expressions.EOF || node is Expressions.EOL) break;

            CurrNode = node;
            args.Add(node!);
        }
        return args.ToArray();
    }

    // mov <identifier> <int>     # move the object by <int> amount of addresses to the right (number can be negative)
    public InstNode ParseMov()
    {
        ExprNode[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.Type != LiteralType.Int) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Mov((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)));
        }
        return new Instructions.Mov((args[0] as Expressions.Identifier)!, Convert.ToInt64(new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)));
    }

    // incr <identifier> <int>  # increment the object by <int>
    public InstNode ParseIncr()
    {
        ExprNode[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.Type != LiteralType.Int) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Incr((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)));
        }
        return new Instructions.Incr((args[0] as Expressions.Identifier)!, Convert.ToInt64(new Literal(TokenKind.IntLit, 1, args[0].File, args[0].Line, args[0].Column).Value));
    }

    // decr <identifier> <int>  # decrement the object by <int>
    public InstNode ParseDecr()
    {
        ExprNode[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.Type != LiteralType.Int) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Decr((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)));
        }
        return new Instructions.Decr((args[0] as Expressions.Identifier)!, Convert.ToInt64(new Literal(TokenKind.IntLit, 1, args[0].File, args[0].Line, args[0].Column).Value));
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
        CodeBlock nodes = new();
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
        }
        else if (In != null)
        {
            // Next(); 
        }
        else
        {
            Utils.Error("what?");
        }

        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        while (token.MoveNext())
        {
            if (Peek()?.Kind == TokenKind.RBrace) break;
            while (Peek().Kind != TokenKind.EOL && Peek().Kind != TokenKind.EOF)
            {
                CurrNode = this.ParseOne()!;
                if (CurrNode == null) break;
                // Utils.Outln($"[For.Body()] {CurrNode.ToString()}");
                if (CurrNode is Expressions.Execute && CurrNode != null)
                {
                    nodes.Add(CurrNode!);
                }

            }
            // nodes.Add(CurrNode!);
        }
        return In == null ?
            new Instructions.For(start!, end!, Id, nodes) :
            new Instructions.Foreach(In, nodes);
    }

    #region IfStatement

    public InstNode ParseIf()
    {
        Next();
        ExprNode Cond = this.ParseOne()!;
        CurrNode = Cond;
        while (Next()?.Kind != TokenKind.LBrace)
        {
            if (Peek()?.Kind == TokenKind.EOL) continue;
            Cond = this.ParseOne()!;
        }
        // Expect(new TokenKind[] { TokenKind.RBrace, TokenKind.Colon });
        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        ExprNode curr;
        CodeBlock nodes = new();
        while (Peek()?.Kind != TokenKind.RBrace)
        {
            Token tok = Peek()!;
            if (tok.Kind == TokenKind.EOF || tok.Kind == TokenKind.EOL || tok == null)
            {
                Next();
                continue;
            }
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
        var t = Peek();
        ExprNode Cond = this.ParseOne()!;
        CurrNode = Cond;
        while (Next()?.Kind != TokenKind.LBrace)
        {
            if (Peek()?.Kind == TokenKind.EOL) continue;
            Cond = this.ParseOne()!;
        }
        // Expect(new TokenKind[] { TokenKind.RBrace, TokenKind.Colon });
        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        ExprNode curr;
        CodeBlock nodes = new();
        while (Peek()?.Kind != TokenKind.RBrace)
        {
            Token tok = Peek()!;
            if (tok.Kind == TokenKind.EOF || tok == null) continue;
            curr = this.ParseOne()!;

            if (curr is Expressions.EOL || curr is Expressions.EOF ||
                !(curr is Expressions.Identifier))
                nodes.Add(curr);

            if (!token.MoveNext()) break;
        }

        return new Instructions.Elif(Cond, nodes!, t.File, t.Line, t.Column);
    }

    public InstNode ParseElse()
    {
        
        ExprNode curr;
        List<ExprNode?> nodes = new();
        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        if (Peek()?.Kind == TokenKind.LBrace)
        {
            Next();
            if (Peek()?.Kind != TokenKind.RBrace)
            {
                while (Peek()?.Kind != TokenKind.RBrace)
                {
                    Token tok = Peek()!;
                    if (tok.Kind == TokenKind.EOF || tok == null) continue;
                    curr = this.ParseOne()!;

                    if (curr is Expressions.EOL || curr is Expressions.EOF ||
                        !(curr is Expressions.Identifier))
                        nodes.Add(curr);

                    if (!token.MoveNext()) break;
                    CurrNode = curr;
                }
            }
        }
        else
        {
            while (Peek()?.Kind != TokenKind.EOL && Peek()?.Kind != TokenKind.EOF)
            {
                Token tok = Peek()!;
                if (tok.Kind == TokenKind.EOF || tok == null) continue;
                curr = this.ParseOne()!;

                if (curr is Expressions.EOL || curr is Expressions.EOF ||
                    !(curr is Expressions.Identifier))
                    nodes.Add(curr);

                if (!token.MoveNext()) break;
                CurrNode = curr;
            }
            if (CurrNode is Expressions.Execute)
                if ((CurrNode as Expressions.Execute).Instruction is Instructions.If) 
                    return new Instructions.Elif(((CurrNode as Expressions.Execute).Instruction as Instructions.If)!);
        }

        return new Instructions.Else(nodes!);
    }

    public List<ExprNode?> GetFunctionBody()
    {
        Next();
        while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
        ExprNode curr;
        List<ExprNode?> nodes = new();
        while (Peek()?.Kind != TokenKind.RBrace)
        {
            Token tok = Peek()!;
            if (tok == null)
            {
                Next();
                continue;
            }

            if (tok.Kind == TokenKind.EOF)
            {
                if (CurrNode != null)
                    nodes.Add(CurrNode);
                return nodes;
            }
            curr = this.ParseOne()!;

            if (curr is Expressions.EOL || curr is Expressions.EOF)
            {
                nodes.Add(CurrNode);
                Next();
                continue;
            }
            CurrNode = curr;
            Next();
        }

        return nodes;
    }

    public InstNode ParseSphere()
    {
        
        
        ExprNode curr;
        List<ExprNode?> nodes = new();
        if (Peek()?.Kind == TokenKind.LBrace)
        {
            while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
            while (Peek()?.Kind != TokenKind.RBrace)
            {
                Token tok = Peek()!;
                if (tok.Kind == TokenKind.EOF || tok == null) continue;
                curr = this.ParseOne()!;

                if (curr is Expressions.EOL || curr is Expressions.EOF)
                    nodes.Add(CurrNode);
                else CurrNode = curr;
                if (!token.MoveNext()) break;
            }
            return new Instructions.Sphere(nodes);
        }
        else
        {
            while (Peek()?.Kind != TokenKind.EOL && Peek()?.Kind != TokenKind.EOF)
            {
                Token tok = Peek()!;
                if (tok.Kind == TokenKind.EOF || tok == null) continue;
                curr = this.ParseOne()!;

                if (curr is Expressions.EOL || curr is Expressions.EOF)
                    nodes.Add(CurrNode);
                else CurrNode = curr;

                if (!token.MoveNext()) break;
            }
            return new Instructions.Sphereln(CurrNode);
        }

        return new Instructions.Sphere(new());
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
        TokenKind.Sphere => ParseSphere(),
        _ => throw new Exception($"Unexpected token {Peek()?.Kind}")
    };
}