using Sphere.Lexer;

namespace Sphere.Parsers.AST;

public partial class Instructions
{
    public record Call : Node
    {
        Expressions.Identifier Target;
        List<Node> Arguments;
        public Call(Expressions.Identifier target, List<Node> arguments, string file, int line, int col) : base(file, line, col)
        {
            this.Target = target;
            this.Arguments = arguments;

            base.Type = $"Expressions+{this.GetType().Name}";
        }
        public override string ToString() => $"{Target}({string.Join(", ", Arguments)})";
    }
}