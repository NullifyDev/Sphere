namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Inln : Node
    {
        public Expressions.Literal Prompt;
        public Inln(Expressions.Literal prompt, string file, int line, int col) : base(file, line, col)
        {
            this.Prompt = prompt;
        }
        public override string ToString() => $"inputln({Prompt});";
    }
}