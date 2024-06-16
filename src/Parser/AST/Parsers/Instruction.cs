using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseInstructions(string file, int line, int col) => Peek()?.Kind switch
    {
        TokenKind.Mov => ParseMov(file, line, col),
        TokenKind.PtrIncr => ParseIncr(file, line, col),
        TokenKind.PtrDecr => ParseDecr(file, line, col),
        TokenKind.Outln => ParseOutln(file, line, col),
        TokenKind.Out => ParseOut(file, line, col),
        TokenKind.Input => ParseIn(file, line, col),
        TokenKind.Inputln => ParseInln(file, line, col),
        TokenKind.If => ParseIf(file, line, col),
        TokenKind.Elif => ParseElif(file, line, col),
        TokenKind.Else => ParseElse(file, line, col),
        TokenKind.For => ParseFor(file, line, col),
        TokenKind.Sphere => ParseSphere(file, line, col),
        TokenKind.While => ParseWhile(file, line, col),
        _ => throw new Exception($"Unexpected instruction token {Peek()?.Kind}")
    };

    #region UtilFuncs
    private Node[] GetInstArgs()
    {
        // Node? lastNode = null;
        List<Node> args = new();
        while (Next()?.Kind != TokenKind.EOL && Peek()?.Kind != TokenKind.EOF)
        {
            var node = this.ParseOne(Peek());
            if (node is Expressions.EOF || node is Expressions.EOL) break;

            CurrNode = node;
            args.Add(node!);
        }
        return args.ToArray();
    }
    public Node GetCondition()
    {
        Node Cond = null;
        Node tmp = null;
        while (Next()?.Kind != TokenKind.LBrace && Peek()?.Kind != TokenKind.Colon)
        {
            if (Peek()?.Kind == TokenKind.EOL) continue;
            tmp = this.ParseOne()!;
            if (tmp is Expressions.EOF) break;

            if (tmp is Expressions.Operator)
            {
                var t = tmp as Expressions.Operator;
                if (t.OpType == TokenKind.And || t.OpType == TokenKind.Or)
                {
                    Utils.Outln(t.Type.ToString());
                    t.Left = null;
                    t.Right = null;
                    CurrNode = this.ParseLast();
                }
                tmp = t;
                
                if (Cond is Expressions.Operator)
                {
                    var cnd = Cond as Expressions.Operator;
                    if (cnd.OpType == TokenKind.And || cnd.OpType == TokenKind.Or)
                    {
                        if (cnd.Left == null) cnd.Left = t;
                        else if (cnd.Right == null) cnd.Right = t;
                        continue;
                    }
                    else
                    {
                        if (t.Left == null) t.Left = cnd;
                        else if (t.Right == null) t.Right = cnd;
                    }
                    Cond = t;
                    continue;
                }
                Cond = t;
            }
            if (tmp is Expressions.Literal)
                if ((tmp as Expressions.Literal)!.LitType == LiteralType.Boolean) 
                    Cond = tmp;

            CurrNode = tmp;
        }
        return Cond;
    }
    public List<Node> GetBody()
    {
        Node curr;
        List<Node> nodes = new();
        if (Peek().Kind == TokenKind.Colon)
        {
            Token tok = Next()!;
            while (tok.Kind == TokenKind.EOL) tok = Next()!;
            if (tok.Kind == TokenKind.LBrace) return this.GetBody();

            curr = this.ParseOne()!;

            if (curr is Expressions.EOF || curr is not Expressions.Identifier)
                nodes.Add(curr);
        }
        else if (Peek().Kind == TokenKind.LBrace)
        {
            while (token.MoveNext() && Peek()?.Kind == TokenKind.EOL) ;
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
        }
        return nodes;
    }
    #endregion

    public Node ParseMov   (string file, int line, int col)
    {
        Node[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.LitType != LiteralType.Int) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Mov((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)), file, line, col);
        }
        return new Instructions.Mov(
            (args[0] as Expressions.Identifier)!, 
            Convert.ToInt64(
                new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)
            ), file, line, col
        );
    }
    public Node ParseIncr  (string file, int line, int col)
    {
        Node[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.LitType != LiteralType.Int) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Incr((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)), file, line, col);
        }
        return new Instructions.Incr((args[0] as Expressions.Identifier)!, Convert.ToInt64(new Literal(TokenKind.IntLit, 1, args[0].File, args[0].Line, args[0].Column).Value), file, line, col);
    }
    public Node ParseDecr  (string file, int line, int col)
    {
        Node[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.LitType != LiteralType.Int) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Decr((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)), file, line, col);
        }
        return new Instructions.Decr((args[0] as Expressions.Identifier)!, Convert.ToInt64(new Literal(TokenKind.IntLit, 1, args[0].File, args[0].Line, args[0].Column).Value), file, line, col);
    }
    public Node ParseIn    (string file, int line, int col)
    {
        Node[] args = GetInstArgs();
        if (args.Length == 1)
        {
            if (args[0] is not Expressions.Literal) throw new Exception($"Expected Literal but got {args[1].GetType().Name}");
            return new Instructions.In(args[0] as Expressions.Literal, file, line, col);
        }
        return new Instructions.In(null, file, line, col);
    }
    public Node ParseInln  (string file, int line, int col)
    {
        Node[] args = GetInstArgs();
        if (args.Length == 1)
        {
            if (args[0] is not Expressions.Literal) throw new Exception($"Expected Literal but got {args[1].GetType().Name}");
            return new Instructions.In(args[0] as Expressions.Literal, file, line, col);
        }
        return new Instructions.In(null, file, line, col);
    }
    public Node ParseOut   (string file, int line, int col) => new Instructions.Out(GetInstArgs(), file, line, col);
    public Node ParseOutln (string file, int line, int col) => new Instructions.Outln(GetInstArgs(), file, line, col);
    public Node ParseReturn(string file, int line, int col) => new Instructions.Return(file, line, col, GetInstArgs().ToList());
    public Node ParseFor   (string file, int line, int col)
    {
        List<Node> nodes = new();
        Node? start = null;
        Node? end = null;
        Node? Id = null;

        // Node? LastNode = null;
        Node? In = null;

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
            while (Next().Kind != TokenKind.EOL && Peek().Kind != TokenKind.EOF)
            {
                CurrNode = this.ParseOne()!;
                if (CurrNode == null) break;
                // Utils.Outln($"[For.Body()] {CurrNode.ToString()}");
                if (CurrNode.GetType().DeclaringType.Name == "Instructions" && CurrNode != null)
                {
                    nodes.Add(CurrNode!);
                }

            }
            // nodes.Add(CurrNode!);
        }
        return In == null ?
            new Instructions.For(start!, end!, Id, nodes, file, line, col) :
            new Instructions.Foreach(In, nodes, file, line, col);
    }
    public Node ParseWhile (string file, int line, int col) => new Instructions.While(GetCondition(), GetBody(), file, line, col);

    public Node ParseIf(string file, int line, int col)   => new Instructions.If(GetCondition(), GetBody()!, file, line, col);
    public Node ParseElif(string file, int line, int col) => new Instructions.Elif(GetCondition(), GetBody()!, file, line, col);
    public Node ParseElse(string file, int line, int col)
    {
        Node curr;
        List<Node?> nodes = new();
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
        else if (Peek()?.Kind == TokenKind.Colon)
        {
            while (Next()?.Kind != TokenKind.EOL && Peek()?.Kind != TokenKind.EOF)
            {
                Token tok = Peek()!;
                if (tok.Kind == TokenKind.EOF || tok == null) continue;
                curr = this.ParseOne()!;

                if (curr is Expressions.EOL || curr is Expressions.EOF ||
                    !(curr is Expressions.Identifier))
                    nodes.Add(curr);
                else CurrNode = curr;
            }
            if (CurrNode.GetType().DeclaringType.Name == "Instructions")
                if (CurrNode.GetType().FullName.EndsWith("Instructions+If"))
                    return new Instructions.Elif(((CurrNode as Instructions.If)!), file, line, col);
        }
        else
        {
            if (Peek()?.Kind == TokenKind.If)
            {
                var t = Peek();
                Next();
                return ParseElif(t.File, t.Line, t.Column);
            }
            while (Peek()?.Kind != TokenKind.EOL && Peek()?.Kind != TokenKind.EOF)
            {
                Token tok = Peek()!;
                if (tok.Kind == TokenKind.EOF || tok == null) continue;
                curr = this.ParseOne()!;

                if (curr is Instructions.If)
                {
                    return new Instructions.Elif((CurrNode as Instructions.If)!, file, line, col);
                }
                if (curr is Expressions.EOL || curr is Expressions.EOF ||
                    !(curr is Expressions.Identifier))
                    nodes.Add(curr);

                if (!token.MoveNext()) break;
                CurrNode = curr;
            }
            if (CurrNode.GetType().DeclaringType.Name == "Instructions")
                if (CurrNode is Instructions.If)
                    return new Instructions.Elif((CurrNode as Instructions.If)!, file, line, col);
        }
        List<Node> cb = new();
        cb = nodes!;
        return new Instructions.Else(cb, file, line, col);
    }
    
    public Node ParseSphere(string file, int line, int col)
    {
        Node curr;
        List<Node?> nodes = new();
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
            return new Instructions.Sphere(nodes, file, line, col);
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
            return new Instructions.Sphereln(CurrNode, file, line, col);
        }

        return new Instructions.Sphere(new(), file, line, col);
    }
}