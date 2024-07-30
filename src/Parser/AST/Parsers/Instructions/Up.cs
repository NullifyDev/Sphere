using System.Diagnostics;
using System.Runtime.InteropServices;
using Sphere.Lexer;
using Sphere.Parsers.AST;
using static Sphere.Parsers.AST.Expressions;

namespace Sphere.Parsers;

public partial record Parser
{
    // First Argument: Either an item to move or the amount of times to move the default value
    // Second Argument: Only exists if the first argument is either set to null or has an object.
    public Node ParseUp(string file, int line, int col)
    {
        Node[] args = GetInstArgs();
        Expressions.Identifier? targetPtr = null;
        long amount = 1;

        if (args.Length == 0) return new Instructions.Up(file, line, col, targetPtr, amount);

        if (args[0] is not Expressions.Identifier)
            if (args[0] is not Expressions.Literal)
                Utils.ErrorLang(ErrorType.Compilation, file, $"Expected an Identifier or Literal as first arguent", line, col);

        if (args[0] is Literal)
        {
            if (args.Length == 2 && args[1] is Identifier) Utils.WarningLang(file, $"A pointer is already chosen: {Compilation.Transpiler.DefaultPointer}", line, col);

            var l = args[0] as Literal;

            amount = l.LitType switch
            {
                LiteralType.String => long.Parse(l.Value.ToString() ?? "1"),
                LiteralType.Hex => long.Parse(l.Value.ToString() ?? "0x1", System.Globalization.NumberStyles.HexNumber),
                LiteralType.Int => long.Parse(l.Value.ToString() ?? "1"),
                LiteralType.Boolean => l.Value.ToString() == "true" || l.Value.ToString() == "false" ? (l.Value.ToString() == "true" ? 1 : 0) : 0,
            };
        }

        if (args.Length == 2)
        {
            if (args[0] is Literal && args[1] is Identifier)
                Utils.ErrorLang(ErrorType.Compilation, file, $"Cannot have swapped arguments", line, col);

            if (args[0] is Identifier)
            {
                if (args[1] is Literal)
                {
                    var l = args[1] as Literal;
                    if (l.LitType == LiteralType.String)
                    {
                        int x;
                        if (int.TryParse(l.Value.ToString(), out x))
                        {
                            if (x < 0) Utils.ErrorLang(ErrorType.Compilation, file, $"Amount of steps cannot be less than 0", line, col);
                        }
                    }
                    if (l.LitType == LiteralType.Int || l.LitType == LiteralType.Int || l.LitType == LiteralType.Hex) { }
                }
            }
        }

        return new Instructions.Up(file, line, col, targetPtr, amount);
    }
}