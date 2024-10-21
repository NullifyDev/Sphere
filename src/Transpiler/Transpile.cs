using Sphere.Parsers.AST;
using Sphere.Lexer;
using Sphere.Types;

namespace Sphere.Compilation;

public partial class Transpiler
{
    public string File;

    public static Dictionary<string, object> Variables = new();
    public static List<Expressions.Function> Functions = new();
    public static Dictionary<string, List<Node>> Objects = new();

    public static Expressions.Identifier DefaultPointer = new("DEFAULT_SPHERE_POINTER", null, "", 0, 0);
    private IEnumerable<Node> nodes;
    private IEnumerator<Node>? node;
    private Expressions.Identifier LastItem;
    private int depth = 0;

    public Transpiler(string file)
    {
        this.File = file;
        this.nodes = new Sphere.Parsers.Parser(this.File).Parse(); // =  new TypeChecker(this.File).Check();
        DefaultPointer.File = file;
    }

    public IEnumerable<string> Transpile()
    {
        node = nodes.GetEnumerator();
        while (node.MoveNext())
            yield return TranspileOne();

        // yield return string.Join("\n", nodes.Select(x => TranspileOne(x)));
    }
    public string Transpile(IEnumerable<Node> nodes, bool isParam = false, bool indent = true)
    {
        string res = "";
        foreach (Node n in nodes.Select(x => x))
        {
            if (n == null) continue;
            if (n.GetType().DeclaringType!.Name == "Instructions")
            {
                if (n is not Instructions.If && n is not Instructions.Elif && n is not Instructions.Else && n is not Instructions.For && n is not Instructions.Foreach && n is not Instructions.While && n is not Expressions.Function)
                    res += $"{TranspileOne(n, indent)}\n";
                else res += $"{TranspileOne(n, indent)}";
                continue;
            }
            else if (n is Expressions.Identifier || n is Expressions.Literal)
            {
                res += $"{TranspileOne(n, indent)}";
                continue;
            }
            res += isParam ? $"{TranspileOne(n, indent)}, " : $"{TranspileOne(n, indent)}\n";
        }

        if (isParam && res.EndsWith(", "))
            return res[0..^2];
        else return res;
    }

    public string TranspileOne(Node? node = null, bool indent = true)
    {
        var curr = node ?? Peek();
        string body = "";
        switch (curr)
        {
            case Expressions.Function Func:
                if (Variables.ContainsKey(Func.Name.Name))
                {
                    Error.Add(new(ErrorType.Compilation, new Token(TokenKind.Identifier, Func.File, Func.Name.Name, Func.Line, Func.Column), $"{Func.Name.Name} already exists as a Variable"));
                    break;
                }

                if (Transpiler.Objects.ContainsKey(Func.Name.Name))
                {
                    Error.Add(new(ErrorType.Compilation, new Token(TokenKind.Identifier, Func.File, Func.Name.Name, Func.Line, Func.Column), $"{Func.Name.Name} already exists as an Object"));
                    break;
                }

                Expressions.Function? fun = Functions.SingleOrDefault(x => x == Func);
                if (fun == null)
                {
                    Functions.Add(Func);
                    fun = Functions.SingleOrDefault(x => x == Func);

                    depth++;
                    body = Transpile(fun!.Body);
                    depth--;

                    return $"{new string(' ', depth * 4)}{fun.Type} {fun.Name}({string.Join(", ", this.Transpile(fun.Params, true))})\n{new string(' ', depth * 4)}{{\n{body}{new string(' ', depth * 4)}}}\n";
                }
                break;



            case Instructions.Up U:
                return $"{new string(' ', depth * 4)}up({U.Item}, {U.Amount})";
            case Instructions.Down D:
                return $"{new string(' ', depth * 4)}down({D.Item}, {D.Amount})";
            case Instructions.If If:
                depth++;
                body = this.Transpile(If.Body);
                depth--;
                return $"{new string(' ', depth * 4)}if ({TranspileOne(If.Cond)}) \n{new string(' ', depth * 4)}{{\n{body}{new string(' ', depth * 4)}}}\n";
            case Instructions.Elif Elif:
                depth++;
                body = this.Transpile(Elif.Body);
                depth--;
                return $"{new string(' ', depth * 4)}else if ({TranspileOne(Elif.Cond)}) \n{new string(' ', depth * 4)}{{\n{body}{new string(' ', depth * 4)}}}\n";
            case Instructions.Else Else:
                depth++;
                body = Transpile(Else.Body);
                depth--;
                return $"{new string(' ', depth * 4)}else \n{new string(' ', depth * 4)}{{\n{body}{new string(' ', depth * 4)}}}";
            case Instructions.Mov Mov:
                return $"{new string(' ', depth * 4)}mov({Mov.Item ?? LastItem}, {Mov.Amount.ToString() ?? 1.ToString()})";
            case Instructions.Incr Incr:
                return $"{new string(' ', depth * 4)}incr({Incr.Item ?? LastItem}, {Incr.Amount.ToString() ?? 1.ToString()})";
            case Instructions.Decr Decr:
                return $"{new string(' ', depth * 4)}decr({Decr.Item ?? LastItem}, {Decr.Amount.ToString() ?? 1.ToString()})";
            case Instructions.Out Out:
                return $"{new string(' ', depth * 4)}{Out.ToString()}";
            case Instructions.Outln Outln:
                return $"{new string(' ', depth * 4)}{Outln.ToString()}";
            case Instructions.In In:
                return $"{new string(' ', depth * 4)}{In}";
            case Instructions.Inln Inln:
                return $"{new string(' ', depth * 4)}{Inln}";
            case Instructions.Continue Continue:
                return $"{new string(' ', depth * 4)}continue";
            case Instructions.For For:
                depth++;
                body = this.Transpile(For.Body);
                depth--;
                return $"{new string(' ', depth * 4)}for (int {For.Id!.Name} = {For.Start}; {For.Id} < {For.End}; {For.Id}++) \n{new string(' ', depth * 4)}{{\n{body}\n{new string(' ', depth * 4)}}}\n";
            case Expressions.Literal Lit:
                return $"{Lit.Type.Kind switch
                {
                    TypeKind.String => $"\"{Lit.Value.ToString()}\"",
                    TypeKind.Int => Lit.Value.ToString() ?? "0",
                    TypeKind.Bool => Lit.Value.ToString() ?? "false",
                    _ => (TypeKind)Utils.InternalError(FailedProcedure.T, "Literal.Type", $"Unknown type \"{Lit.Type.Kind}\"", Lit.File, Lit.Line, Lit.Column),
                }}";
            case Expressions.Identifier Id:
                return $"{Id.Name}";
            case Expressions.Operator op:
                return op.OpType switch
                {
                    TokenKind.Equal => $"{new string(' ', depth * 4)}{this.TranspileOne(op.Left)} = {this.TranspileOne(op.Right)}",
                    TokenKind.DoubleEq => $"{this.TranspileOne(op.Left)} == {this.TranspileOne(op.Right)}",
                    TokenKind.Dot => $"{this.TranspileOne(op.Left)} . {this.TranspileOne(op.Right)}",
                    TokenKind.BangEq => $"{this.TranspileOne(op.Left)} != {this.TranspileOne(op.Right)}",
                    TokenKind.Greater => $"{this.TranspileOne(op.Left)} > {this.TranspileOne(op.Right)}",
                    TokenKind.GreaterEq => $"{this.TranspileOne(op.Left)} >= {this.TranspileOne(op.Right)}",
                    TokenKind.Less => $"{this.TranspileOne(op.Left)} < {this.TranspileOne(op.Right)}",
                    TokenKind.LessEq => $"{this.TranspileOne(op.Left)} <= {this.TranspileOne(op.Right)}",
                    TokenKind.Colon => $"{this.TranspileOne(op.Right)} {this.TranspileOne(op.Left)}",
                    TokenKind.And => $"{this.TranspileOne(op.Left)} && {this.TranspileOne(op.Right)}",
                    TokenKind.Or => $"{this.TranspileOne(op.Left)} || {this.TranspileOne(op.Right)}",
                    TokenKind.Slash => $"{this.TranspileOne(op.Left)} / {this.TranspileOne(op.Right)}",
                    TokenKind.SlashEq => $"{new string(' ', depth + 1 * 4)}{this.TranspileOne(op.Left)} /= {this.TranspileOne(op.Right)}",
                    TokenKind.Plus => $"{this.TranspileOne(op.Left)} + {this.TranspileOne(op.Right)}",
                    TokenKind.PlusEq => $"{new string(' ', depth * 4)}{this.TranspileOne(op.Left)} += {this.TranspileOne(op.Right)}",
                    TokenKind.Minus => $"{this.TranspileOne(op.Left)} - {this.TranspileOne(op.Right)}",
                    TokenKind.MinusEq => $"{new string(' ', depth * 4)}{this.TranspileOne(op.Left)} -= {this.TranspileOne(op.Right)}",
                    TokenKind.Modulo => $"{this.TranspileOne(op.Left)} % {this.TranspileOne(op.Right)}",
                    _ => throw (Exception)Utils.InternalError(FailedProcedure.T, "Node", $"Unknown Node \"{op.OpType}\"", op.File, op.Line, op.Column)
                };

            case Instructions.Foreach Foreach:
                depth++;
                body = Transpile(Foreach.Body);
                depth--;
                return $"{new string(' ', depth * 4)}for (auto {Foreach.In!.Left} : {Foreach.In.Right}) \n{new string(' ', depth * 4)}{{\n{body}{new string(' ', depth * 4)}}}\n";

            case Instructions.While While:
                depth++;
                body = Transpile(While.Body);
                depth--;
                return $"{new string(' ', depth * 4)}while ({TranspileOne(While.Condition)}) \n{new string(' ', depth * 4)}{{\n{body}{new string(' ', depth * 4)}}}\n";
            case Instructions.Sphere Sphere:
                foreach (var n in Sphere.Body)
                {
                    switch (n)
                    {
                        case Expressions.Operator op:

                            if (op.OpType != TokenKind.Colon) Error.Print(ErrorType.Compilation, op, $"Expected ':' but found \"{op.Left}\"");
                            if (op.Left is Expressions.Identifier)
                            {
                                var prop = (op.Left as Expressions.Identifier)!;
                                var val = (op.Right as Expressions.Literal)!;
                                Token? t = new Token(TokenKind.Dot, op.File, prop.Name.ToString() ?? "<null>", op.Line, op.Column);

                                if (Compilation.Variables.Props.ContainsKey(prop.Name))
                                {
                                    Compilation.Variables.Props[prop.Name] = (val.Type.Kind switch
                                    {
                                        TypeKind.String => $"\"{val.Value.ToString()}\"",
                                        TypeKind.Int => int.Parse(val.Value.ToString() ?? "0"),
                                        TypeKind.Bool => bool.Parse(val.Value.ToString() ?? "false"),
                                        _ => throw new Exception($"Unknown type \"{val.Type}\""),
                                    })!;
                                }
                                else
                                {
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
                    $"{new string(' ', depth * 4)}return ({string.Join(", ", Transpile(Return.Items))});" :
                    $"{new string(' ', depth * 4)}return {string.Join(", ", Transpile(Return.Items))};";

            case Instructions.GetCurrAddressVal:
                var output = DefaultPointer.Name == "$@" ? "DEFAULT_SPHERE_POINTER" : DefaultPointer.Name;
                return $"{new string(' ', depth * 4)}{output}";
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