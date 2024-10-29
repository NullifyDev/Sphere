namespace Sphere.Parsers.AST;

using Sphere.Types;
using static Sphere.Parsers.AST.Expressions;

public partial class Instructions
{
    public record For : Node
    {
        public Node Start, End;
        public Identifier? Id;
        public List<Node> Body;

        public For(Node start, Node end, Identifier? id, List<Node> body, string file, int line, int col) : base(file, line, col)
        {

            if (id != null) {
                if (id.Literal == null)
                    id.Literal = new Literal(TypeKind.Int, 0, id.File, id.Line, id.Column);

                id.Literal!.Type = new(Lexer.TokenKind.DataType_Int, id.File, id.Line, id.Column);
            }
            this.Start = start;
            this.End = end;
            this.Id = id;
            this.Body = body;
        }
        public override string ToString() => $"for (int {this.Id!.Name} = {this.Start}; {this.Id.Name} < {this.End}; {this.Id}++) {{\n    {this.Body}\n}}\n";
    }
}