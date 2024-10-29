using Sphere.Lexer;

namespace Sphere.Parsers.AST;

using Sphere.Types;

public partial class Expressions
{

    public record Literal : Node
    {
        public Expressions.Type Type;
        public object? Value;
        public bool Nullable;
        public Literal(Token token, string file, int line, int col) : base(file, line, col)
        {
            this.Type = new(token);
            this.Value = token.Kind switch
            {
                TokenKind.IntLit => int.Parse(token.Value),
                TokenKind.StringLit => token.Value.ToString(),
                TokenKind.BoolLit => token.Value == "true" ? true : (token.Value == "false" ? false : null),
                _ => null
            };
        }

        public Literal(TypeKind type, object? value, string file, int line, int col) : base(file, line, col)
        {
            // if (this.Type == null)
            //     Utils.InternalError(FailedProcedure.P, "Literal.Type", "Could not determine Data Type (is null)", file, line, col);
            this.Type = new(type, file, line, col);
            if (this.Value == null)
            {
                this.Value = this.Nullable ? null : (this.Type.Kind switch
                {
                    TypeKind.String => "",
                    TypeKind.Int => 0,
                    TypeKind.Bool => false,
                    TypeKind.Void => null,
                    _ => (Expressions.Type)Utils.InternalError(FailedProcedure.P, "Literal.Type", $"Unkown or unimplemented Literal Type {this.Type.Kind}", this.File, this.Line, this.Column)
                });
            }
        }

        public Literal(TokenKind type, object value, string file, int line, int col) : base(file, line, col)
        {
            // if (this.Type == null)
            //     Utils.InternalError(FailedProcedure.P, "Literal.Type", "Type of Literal is null", file, line, col);
            this.Type = type switch
            {
                TokenKind.StringLit => new Type(TypeKind.String, file, line, col),
                TokenKind.DataType_String => new Type(TypeKind.String, file, line, col),
                TokenKind.IntLit => new Type(TypeKind.Int, file, line, col),
                TokenKind.DataType_Int => new Type(TypeKind.Int, file, line, col),
                TokenKind.BoolLit => new Type(TypeKind.Bool, file, line, col),
                TokenKind.DataType_Bool => new Type(TypeKind.Bool, file, line, col),
                TokenKind.DataType_Void => new Type(TypeKind.Bool, file, line, col),

                _ => (Expressions.Type)Utils.InternalError(FailedProcedure.P, "Literal.Type", $"Unkown or unimplemented Literal Type {type}", this.File, this.Line, this.Column)
            };
            this.Value = value;
        }

        // public string GetValue() => this.Type.Kind switch {
        //     TypeKind.String => $"\"{this.Value.ToString()}\"",
        //     TypeKind.Int => this.Value.ToString(),
        //     TypeKind.Bool => this.Value.ToString(),
        //     _ => (string)Utils.InternalError(FailedProcedure.P, "Literral.Type", $"Unrecognised or unimplemented Literal Type {this.Type.ToString()}", this.File, this.Line, this.Column)
        // };
        public void SetToNullable() => this.Nullable = true;
        public override string ToString() => this.Type.Kind switch
        {
            TypeKind.String => $"\"{this.Value.ToString()}\"",
            TypeKind.Int => this.Value.ToString(),
            TypeKind.Bool => this.Value.ToString(),
            _ => (string)Utils.InternalError(FailedProcedure.P, "Literral.Type", $"Unrecognised or unimplemented Literal Type {this.Type.ToString()}", this.File, this.Line, this.Column)
        };
    }
}