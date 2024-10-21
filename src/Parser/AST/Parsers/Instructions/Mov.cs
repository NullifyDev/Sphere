using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;
using Sphere.Types;

namespace Sphere.Parsers;

public partial record Parser
{
    public Node ParseMov(string file, int line, int col)
    {
        Node[] args = GetInstArgs();

        if (args[0] is not Expressions.Identifier) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");
        if ((args[0] as Expressions.Identifier)!.Name == "" ||
            (args[0] as Expressions.Identifier)!.Name == null) Utils.Error($"Expected Identifier but got {args[0].GetType().Name}");

        if (args.Length == 2)
        {
            if (args[1] is not Expressions.Literal) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            if ((args[1] as Expressions.Literal)!.Type.Kind != TypeKind.Int) Utils.Error($"Expected IntLit but got {args[1].GetType().Name}");
            return new Instructions.Mov((args[0] as Expressions.Identifier)!, Convert.ToInt64((args[1] as Expressions.Literal)!.Value ?? new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)), file, line, col);
        }
        return new Instructions.Mov(
            (args[0] as Expressions.Identifier)!,
            Convert.ToInt64(
                new Literal(TokenKind.IntLit, 1, args[1].File, args[1].Line, args[1].Column)
            ), file, line, col
        );
    }
}