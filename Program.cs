namespace Qbe;

using static Utils;

public class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        
        if (args.Length > 0)
        {
            
            new Parser(args[0]).run();
            // new Interpreter().Output();
        }
    }
}
