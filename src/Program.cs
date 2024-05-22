using Newtonsoft.Json;
using System.Text.Json;

namespace Sphere;

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using Sphere.Parsers;
using Sphere.Parsers.AST;

public class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        List<ExprNode> code = new();
        IEnumerator<ExprNode>? parser = null;
        foreach(string a in args) 
            if (!a.StartsWith("--")) 
                parser = new Parser(a, File.ReadAllText(a)).Parse().GetEnumerator();

        foreach(var a in args) {
            if (a == "--ast") { 
                while (parser!.MoveNext())
                    code.Add(parser.Current);   
                
                Console.WriteLine(JsonConvert.SerializeObject(code, Formatting.Indented));
            }
            else if (a == "--code") { 
                while (parser!.MoveNext())
                    Console.WriteLine(parser.Current);
            }
        }
    }
}