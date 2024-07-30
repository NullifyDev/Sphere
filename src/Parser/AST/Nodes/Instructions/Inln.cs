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

            base.Type = $"Instructions+{this.GetType().Name}";
        }
        public override string ToString() => $"inln({Prompt})";
    }
}