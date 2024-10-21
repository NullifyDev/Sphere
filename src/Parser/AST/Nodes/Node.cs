namespace Sphere.Parsers.AST;

public record Node
{
    public string File;
    public int Line, Column;
    public Node(string File, int Line, int Column)
    {
        this.File = File;
        this.Line = Line;
        this.Column = Column;
    }

    // public object GetValue() => GetValue(this);

    // public object GetValue(Node n) => (n switch {
    //     Expressions.Operator => n as Expressions.Operator,
    //     Expressions.Literal => n as Expressions.Literal,
    //     Expressions.Prefix => n as Expressions.Prefix,
    //     Expressions.Identifier => n as Expressions.Identifier,
    //     Expressions.Grouping => n as Expressions.Grouping,
    //     Expressions.Comment => n as Expressions.Comment,
    //     Expressions.EOF => n as Expressions.EOF,
    //     Expressions.EOL => n as Expressions.EOL,

    //     Instructions.Call => n as Instructions.Call,
    //     Instructions.Mov => n as Instructions.Mov,
    //     Instructions.Incr => n as Instructions.Incr,
    //     Instructions.Decr => n as Instructions.Decr,
    //     Instructions.If => n as Instructions.If,
    //     Instructions.Elif => n as Instructions.Elif,
    //     Instructions.Else => n as Instructions.Else,
    //     Instructions.Out => n as Instructions.Out,
    //     Instructions.Outln => n as Instructions.Outln,
    //     Instructions.In => n as Instructions.In,
    //     Instructions.Inln => n as Instructions.Inln,
    //     Instructions.Continue => n as Instructions.Continue,
    //     Instructions.For => n as Instructions.For,
    //     Expressions.Function => n as Expressions.Function,
    //     Instructions.Foreach => n as Instructions.Foreach,
    //     Instructions.While => n as Instructions.While,
    //     Instructions.Sphere => n as Instructions.Sphere,
    //     Instructions.Sphereln => n as Instructions.Sphereln,
    //     Instructions.Return => n as Instructions.Return,
    //     Instructions.Invoke => n as Instructions.Invoke,
    //     Instructions.Variable => n as Instructions.Variable,

    //     _ => throw new Exception($"No such Node as \"{n}\"")
    // })!;

}