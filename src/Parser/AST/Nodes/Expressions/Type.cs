namespace Sphere.Parsers.AST;

using Sphere.Types;
using Sphere.Lexer;
using Sphere.Compilation;
using System.Security.Cryptography.X509Certificates;

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

        public static string GetStrFmt(IEnumerable<Node> n)
        {
            string res = "";
            foreach (var s in n)
            {
                res += $"{GetStrFmt(s)} ";
            }
            return res[0..^1];
        }
        public static string GetStrFmt(Node? n)
        {

            Utils.Outln(n == null ? $"n: {n.GetType().Name} - null" : $"n: {n.GetType().Name} - {n}");
            switch (n)
            {
                case Expressions.Literal l:
                    return GetStrFmt(l.Type);
                case Expressions.Identifier i:
                    return GetStrFmt(Transpiler.Variables.SingleOrDefault(x => x.Name == i.Name).Literal);
                case Expressions.Function f:
                    return GetStrFmt(Transpiler.Functions.SingleOrDefault(x => x.Name == f.Name).Type);
                case Expressions.Type type:

                    return type.Kind switch
                    {
                        TypeKind.String => "%s",
                        TypeKind.Int => "%d",
                        TypeKind.Bool => "%d",
                        TypeKind.Void => "",
                        _ => (string)Utils.InternalError(FailedProcedure.T, "TypeFormatting", $"Unknown Type {n}", n.File, n.Line, n.Column)
                    };
                default:
                    return n == null ? "null" : "what?";
            }
        }
        public override string ToString() => $"{this.Kind.ToString().ToLower()}";
    }
}