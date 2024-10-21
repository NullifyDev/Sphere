namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Foreach : Node
    {

        public Expressions.Operator? In;
        public List<Node> Body;

        public Foreach(Node In, List<Node> Body, string file, int line, int col) : base(file, line, col)
        {
            if (In is not Expressions.Operator) Utils.ErrorLang(ErrorType.Syntax, $"Expected a Binary Operator! Got {In} instead", In.File, In.Line, In.Column);
            if ((In as Expressions.Operator)!.OpType != Lexer.TokenKind.Colon) Utils.ErrorLang(ErrorType.Syntax, $"Expected ':' Got {(In as Expressions.Operator)!.GetType().Name} instead", In.File, In.Line, In.Column);

            this.In = (In as Expressions.Operator)!;
            this.Body = Body;
        }
        public override string ToString() => $"for {In!.Left} in {In!.Right} {{\n    {string.Join("\n    ", Body)}\n}}";
    }
}