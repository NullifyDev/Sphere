namespace Sphere.Parsers.AST;

using Sphere;

public partial class Instructions
{
    public record Down : Node
    {
        public Expressions.Identifier Item;
        public long Amount;

        public Down(string file, int line, int col, Expressions.Identifier? item = null, long amount = 1) : base(file, line, col)
        {
            this.Item = item ?? Compilation.Transpiler.DefaultPointer;
            this.Amount = amount;
        }
        public override string ToString() => $"Mov: [ {Item}, {Amount} ]";
    }
}