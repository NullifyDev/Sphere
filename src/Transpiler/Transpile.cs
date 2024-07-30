using Sphere.Parsers.AST;
using Sphere.Parsers;
using Sphere.Lexer;
using System.Security.Cryptography;
using Sphere.Core;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata;
using Newtonsoft.Json.Linq;

namespace Sphere.Compilation;

public partial record Transpiler
{
    public string File;

    public static Expressions.Identifier DefaultPointer;
    private IEnumerable<Node> nodes;
    private IEnumerator<Node>? node;
    private Expressions.Identifier LastItem;

    public Transpiler(string file) {
        this.File = file;
        this.nodes = new Parser(this.File).Parse();
        DefaultPointer = new("DEFAULT_SPHERE_POINTER", null, file, 0, 0);
    }

    public IEnumerable<string> Transpile()
    {
        node = nodes.GetEnumerator();
        while (node.MoveNext())
            yield return TranspileOne();
        
        // yield return string.Join("\n", nodes.Select(x => TranspileOne(x)));
    }
    public string Transpile(IEnumerable<Node> nodes, bool isParam = false)
    {
        string res = "";
        foreach(Node n in nodes.Select(x => x)) {
            if (n == null) continue;
            if (n.GetType().DeclaringType.Name == "Instructions") {
                if (n is not Instructions.If && n is not Instructions.Elif && n is not Instructions.Else && n is not Instructions.For && n is not Instructions.Foreach && n is not Instructions.While && n is not Instructions.Function)
                    res += $"{TranspileOne(n)}; ";
                else res += $"{TranspileOne(n)} ";
                continue;
            } else if (n is Expressions.Identifier || n is Expressions.Literal) {
                res += $"{TranspileOne(n)}";
                continue;
            }
            res += isParam ? $"{TranspileOne(n)}, " : $"{TranspileOne(n)}; ";
        }

        if (isParam && res.EndsWith(", ")) 
            return res[0..^2];
        else return res;
    }

    public string TranspileOne(Node? node = null, int? depth = null)
    {
        depth ??= 0;
        var curr = node ?? Peek();
        switch (curr)
        {
            case Instructions.Function Func:
                if (Variables.ContainsKey(Func.Name.Name)) {
                    Error.Add(new(ErrorType.Compilation, new Token(TokenKind.Identifier, Func.File, Func.Name.Name, Func.Line, Func.Column), $"{Func.Name.Name} already exists as a Variable"));
                    break;
                }

                if (Transpiler.Objects.ContainsKey(Func.Name.Name)) {
                    Error.Add(new(ErrorType.Compilation, new Token(TokenKind.Identifier, Func.File, Func.Name.Name, Func.Line, Func.Column), $"{Func.Name.Name} already exists as an Object"));
                    break;
                }

                Instructions.Function? fun = Functions.SingleOrDefault(x => x == Func);
                if (fun == null)  {
                    Transpiler.Functions.Add(Func);
                    fun = Transpiler.Functions.SingleOrDefault(x => x == Func);
                    return $"{fun.ReturnType} {fun.Name}({string.Join(", ", this.Transpile(fun.Params, true))}) {{ {Transpile(fun.Body)} }}";
                }
                break;



            case Instructions.Up U:
                return $"up({U.Item}, {U.Amount})";
            case Instructions.Down D:
                return $"down({D.Item}, {D.Amount})";
            case Instructions.If If:
                return $"if ({TranspileOne(If.Cond)}) {{ {this.Transpile(If.Body)} }}";
            case Instructions.Elif Elif:
                return $"else if ({TranspileOne(Elif.Cond)}) {{ {this.Transpile(Elif.Body)} }}";
            case Instructions.Else Else:
                return $"else {{ {Transpile(Else.Body)} }}";
            case Instructions.Mov Mov:
                return $"mov({Mov.Item ?? LastItem}, {Mov.Amount.ToString() ?? 1.ToString()})";
            case Instructions.Incr Incr:
                return $"incr({Incr.Item ?? LastItem}, {Incr.Amount.ToString() ?? 1.ToString()})";
            case Instructions.Decr Decr:
                return $"decr({Decr.Item ?? LastItem}, {Decr.Amount.ToString() ?? 1.ToString()})";
            case Instructions.Out Out:
                return $"out({this.Transpile(Out.Args)})";
            case Instructions.Outln Outln:
                return $"outln({this.Transpile(Outln.Args)})";
            case Instructions.In In:
                return $"input({In.Prompt})";
            case Instructions.Inln Inln:
                return $"inputln({Inln.Prompt})";
            case Instructions.Continue Continue:
                return $"continue";
            case Instructions.For For:
                return $"for (int {For.Id.Name} = {For.Start}; {For.Id} < {For.End}; {For.Id}++) {{ {this.Transpile(For.Body)} }}"; 
            case Expressions.Literal Lit:
                return Lit.LitType switch
                {
                    LiteralType.String => $"\"{Lit.Value.ToString()}\"",
                    LiteralType.Int => Lit.Value.ToString() ?? "0",
                    LiteralType.Hex => Lit.Value.ToString() ?? "0",
                    LiteralType.Boolean => Lit.Value.ToString() ?? "false",
                    _ => throw new Exception($"Unknown type \"{Lit.LitType}\""),
                };
            case Expressions.Identifier Id:
                return $"{Id.Name}";
            case Expressions.Operator op:
                return $"{op.OpType switch  {
                    TokenKind.Equal     => $"{this.TranspileOne(op.Left)} = {this.TranspileOne(op.Right)}",
                    TokenKind.DoubleEq  => $"{this.TranspileOne(op.Left)} == {this.TranspileOne(op.Right)}",
                    TokenKind.Dot       => $"{this.TranspileOne(op.Left)} . {this.TranspileOne(op.Right)}",
                    TokenKind.BangEq    => $"{this.TranspileOne(op.Left)} != {this.TranspileOne(op.Right)}",
                    TokenKind.Greater   => $"{this.TranspileOne(op.Left)} > {this.TranspileOne(op.Right)}",
                    TokenKind.GreaterEq => $"{this.TranspileOne(op.Left)} >= {this.TranspileOne(op.Right)}",
                    TokenKind.Less      => $"{this.TranspileOne(op.Left)} < {this.TranspileOne(op.Right)}",
                    TokenKind.LessEq    => $"{this.TranspileOne(op.Left)} <= {this.TranspileOne(op.Right)}",
                    TokenKind.Colon     => $"{this.TranspileOne(op.Right)} {this.TranspileOne(op.Left)}",
                    TokenKind.And       => $"{this.TranspileOne(op.Left)} && {this.TranspileOne(op.Right)}",
                    TokenKind.Or        => $"{this.TranspileOne(op.Left)} || {this.TranspileOne(op.Right)}",
                    TokenKind.Slash     => $"{this.TranspileOne(op.Left)} / {this.TranspileOne(op.Right)}",
                    TokenKind.SlashEq   => $"{this.TranspileOne(op.Left)} /= {this.TranspileOne(op.Right)}",
                    TokenKind.Plus      => $"{this.TranspileOne(op.Left)} + {this.TranspileOne(op.Right)}",
                    TokenKind.PlusEq    => $"{this.TranspileOne(op.Left)} += {this.TranspileOne(op.Right)}",
                    TokenKind.Minus     => $"{this.TranspileOne(op.Left)} - {this.TranspileOne(op.Right)}",
                    TokenKind.MinusEq   => $"{this.TranspileOne(op.Left)} -= {this.TranspileOne(op.Right)}",
                    TokenKind.Modulo    => $"{this.TranspileOne(op.Left)} % {this.TranspileOne(op.Right)}",
                    _                   => throw (Exception)Utils.InternalError(op.File, $"Unknown Node \"{op.OpType}\"", op.Line, op.Column)
                }}";

            case Instructions.Foreach Foreach:
                return $"for (auto {Foreach.In.Left} : {Foreach.In.Right}) {{ {string.Join("; ", Foreach.Body)} }}";

            case Instructions.While While:
                return $"while ({this.TranspileOne(While.Condition)}) {{ {this.Transpile(While.Body)}}} ";
            case Instructions.Sphere Sphere:
                foreach (var n in Sphere.Body) {
                    switch(n) {
                        case Expressions.Operator op:

                            if (op.OpType != TokenKind.Colon) Error.Print(ErrorType.Compilation, op, $"Expected ':' but found \"{op.Left}\"");
                            if (op.Left.Type == "Identifier") {
                                var prop = (op.Left as Expressions.Identifier)!;
                                var val = (op.Right as Expressions.Literal)!;
                                Token? t = new Token(TokenKind.Dot, op.File, prop.Name.ToString() ?? "<null>", op.Line, op.Column);

                                if (Compilation.Variables.Props.ContainsKey(prop.Name)) {
                                                Compilation.Variables.Props[prop.Name] = (val.LitType switch {
                                                    LiteralType.String => $"\"{val.Value.ToString()}\"",
                                                    LiteralType.Int => int.Parse(val.Value.ToString() ?? "0"),
                                                    LiteralType.Hex => int.Parse(val.Value.ToString() ?? "0"),
                                                    LiteralType.Boolean => bool.Parse(val.Value.ToString() ?? "false"),
                                                    _      => throw new Exception($"Unknown type \"{val.Type}\""),
                                                })!;
                                } else {
                                    t.Line = val.Line;
                                    t.Column = val.Column;
                                    Error.Print(ErrorType.Compilation, t, $"asdased");
                                }
                            }
                            break;
                    }
                }
                break;
            case Instructions.Sphereln Sphereln:
                break;

            case Instructions.Return Return:
                return Return.Items.Count > 1 ? 
                    $"return ({string.Join(", ", Transpile(Return.Items))})" :
                    $"return {string.Join(", ", Transpile(Return.Items))}";

            case Instructions.GetCurrAddressVal:
                return DefaultPointer.Name == "$@" ? "DEFAULT_SPHERE_POINTER" : DefaultPointer.Name;
        }
        return "";
    }

    private Node Peek() => node!.Current;
    private Node Next()
    {
        isAtEnd();
        return Peek();
    }
    private bool isAtEnd() => !node!.MoveNext();
}