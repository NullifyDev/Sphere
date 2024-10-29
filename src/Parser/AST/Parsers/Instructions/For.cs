using System.ComponentModel.DataAnnotations.Schema;
using Sphere.Lexer;
using Sphere.Parsers.AST;
using Sphere.Types;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseFor(string file, int line, int col)
    {
        Node? start = null;
        Node? end = null;
        Identifier? Id = null;
        Node? In = null;

        Next();
        if (Peek()?.Kind == TokenKind.IntLit)
        {
            start = this.ParseOne()!;
        }
        else if (Peek()?.Kind == TokenKind.Dollar || Peek()?.Kind == TokenKind.Identifier)
            CurrNode = this.ParseOne();

        if (Peek()?.Kind == TokenKind.IntLit)
            end = this.ParseOne()!;
        else if (Peek()?.Kind == TokenKind.Colon)
            In = this.ParseOne();

        if (In == null && Peek()?.Kind == TokenKind.Identifier)
        {
            Id = this.ParseOne() as Identifier;

            var obj = Compilation.Transpiler.GetObject(Id) as Identifier;
            if (obj != null)
            {
                Id = obj;
            }

            /// TOFIX: OBJECT IS NOT SET TO AN INSTANCE OF AN OBJECT!
            /// TOFIX: OBJECT IS NOT SET TO AN INSTANCE OF AN OBJECT!
            /// TOFIX: OBJECT IS NOT SET TO AN INSTANCE OF AN OBJECT!
            Id.Name = $"{path}.For.Id_{Id.Name}.{Id.Literal.Type.Kind.ToString()}";


            if (Id is Identifier)
            {
                Id = new Identifier(Id, TypeKind.Int, Id.File, Id.Line, Id.Column);
            }
            Id.Literal = new Expressions.Literal(TypeKind.Int, start switch
            {
                Expressions.Identifier i => i.Literal.Value,
                Expressions.Literal l => l,
                Expressions.Function f => new Literal(f.Type.Kind, f.Type.Kind switch
                {
                    TypeKind.Int => 0,
                    TypeKind.String => "",
                    TypeKind.Bool => false,
                    _ => null
                }, f.File,
                f.Line,
                f.Column),
            }, Id.File, Id.Line, Id.Column);
        }
        else if (In != null)
            Utils.Outln($"{path}.For.Parse(): uh-huh...");

        while (Peek()?.Kind == TokenKind.EOL) Next();
        return In == null ?
            new Instructions.For(start!, end!, Id, GetBody($"{path}.For"), file, line, col) :
            new Instructions.Foreach(In, GetBody($"{path}.Foreach"), file, line, col);
    }
}