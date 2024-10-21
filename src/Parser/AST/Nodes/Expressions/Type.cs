namespace Sphere.Parsers.AST;

using Sphere.Types;
using Sphere.Lexer;

public partial class Expressions
{
    public record class Type : Node
    {
        public TypeKind Kind;

        public Type(TypeKind kind, string file, int line, int col) : base(file, line, col)
        {
            this.Kind = kind;
        }

        public Type(Token t) : base(t.File, t.Line, t.Column)
        {
            this.Kind = t.Kind switch
            {
                TokenKind.StringLit => TypeKind.String,
                TokenKind.DataType_String => TypeKind.String,
                TokenKind.IntLit => TypeKind.Int,
                TokenKind.DataType_Int => TypeKind.Int,
                TokenKind.BoolLit => TypeKind.Bool,
                TokenKind.DataType_Bool => TypeKind.Bool,
                TokenKind.DataType_Void => TypeKind.Void,

                _ => (TypeKind)Utils.InternalError(FailedProcedure.P, "Literal.Type", $"Unkown or unimplemented Type {this.Kind}", t.File, t.Line, t.Column),
            };
        }

        public Type(TokenKind tk, string file, int line, int col) : base(file, line, col)
        {
            this.Kind = tk switch
            {
                TokenKind.StringLit => TypeKind.String,
                TokenKind.DataType_String => TypeKind.String,
                TokenKind.IntLit => TypeKind.Int,
                TokenKind.DataType_Int => TypeKind.Int,
                TokenKind.BoolLit => TypeKind.Bool,
                TokenKind.DataType_Bool => TypeKind.Bool,
                TokenKind.DataType_Void => TypeKind.Void,

                _ => (TypeKind)Utils.InternalError(FailedProcedure.P, "Literal.Type", $"Unkown or unimplemented Type {this.Kind}", file, line, col),
            };
        }

        public bool CompareTo(Type t) => this.Kind == t.Kind;

        public static string GetStrFmt(IEnumerable<Node> n) {
            string res = "";
            foreach(var s in n) {
                res += $"{GetStrFmt(s)} ";
            }
            return res[0..^1];
        }
        public static string GetStrFmt(Node n) => n switch
        {
            Literal l => GetStrFmt(l.Type),
            Identifier i => GetStrFmt(i.Type),
            Function f => GetStrFmt(f.Type),
            Type type => type.Kind switch
            {
                TypeKind.String => "%s",
                TypeKind.Int => "%d",
                TypeKind.Bool => "%d",
                TypeKind.Void => "",
            },
            _ => "",
        };

        public override string ToString() => $"{this.Kind.ToString().ToLower()}";
    }
}
