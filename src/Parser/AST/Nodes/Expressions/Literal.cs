using Sphere.Lexer;

namespace Sphere.Parsers.AST;

using Sphere.Types;

public partial class Expressions
{

    public record Literal : Node
    {
        public Expressions.Type Type;
        public object Value;
        public Literal(Token token, string file, int line, int col) : base(file, line, col)
        {
            this.Type = new(token);
            this.Value = token.Value;
        }
        public Literal(TokenKind type, object value, string file, int line, int col) : base(file, line, col)
        {
            // if (this.Type == null)
            //     Utils.InternalError(FailedProcedure.P, "Literal.Type", "Could not determine Data Type (is null)", file, line, col);
            this.Type = type switch
            {
                TokenKind.StringLit => new(TypeKind.String, file, line, col),
                TokenKind.DataType_String => new(TypeKind.String, file, line, col),
                TokenKind.IntLit => new(TypeKind.Int, file, line, col),
                TokenKind.DataType_Int => new(TypeKind.Int, file, line, col),
                TokenKind.BoolLit => new(TypeKind.Bool, file, line, col),
                TokenKind.DataType_Bool => new(TypeKind.Bool, file, line, col),

                _ => (Expressions.Type)Utils.InternalError(FailedProcedure.P, "Literal.Type", $"Unkown or unimplemented Literal Type {this.Type.Kind}", this.File, this.Line, this.Column)
            };
            this.Value = value;
        }
        public override string ToString() => this.Type.Kind switch
        {
            TypeKind.String => $"\"{this.Value.ToString()}\"" ?? "",
            TypeKind.Int => this.Value.ToString() ?? "null",
            TypeKind.Bool => this.Value.ToString() ?? "null",
            _ => (string)Utils.InternalError(FailedProcedure.P, "Literral.Type", $"Unrecognised or unimplemented Literal Type {this.Type.ToString()}", this.File, this.Line, this.Column)
        };
    }
}