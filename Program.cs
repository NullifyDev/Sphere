namespace Qbe;

using static Utils;

public class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        if (args.Length > 0)
        {
            new Executer(
                new Parser(
                    new Lexer(
                        File.ReadAllText(args[0]).Replace("\r\n", "\n").Replace("\r", "\n")
                    ).Lex()
                ).Parse()
            ).Execute();
        }
    }
}
