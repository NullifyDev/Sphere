using System.Text.Json;

namespace Sphere;

using Sphere.Parsers;
using Sphere.Parsers.AST;

public class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        IEnumerator<ExprNode> parser = new Parser(args[0], File.ReadAllText(args[0])).Parse().GetEnumerator();
        while (parser.MoveNext()) Console.WriteLine($"{parser.Current}");

        // Console.WriteLine(JsonSerializer.Serialize(parser));
    }
}