namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record Incr : Node
    {
        public Expressions.Identifier Item;
        public long Amount;

        public Incr(Expressions.Identifier item, long amount, string file, int line, int col) : base(file, line, col)
        {
            this.Item = item;
            this.Amount = amount;

            base.Type = $"Instructions+{this.GetType().Name}";
        }
        public override string ToString() => $"Incr: [ {Item}, {Amount} ]";
    }
}