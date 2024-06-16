using Newtonsoft.Json;
using System.Text.Json;

namespace Sphere;

using Sphere.Parsers;
using Sphere.Compilation;
using Sphere.Parsers.AST;

public class Program
{
    static void Main(string[] args)
    {
        Console.Clear();

        List<Node> code = new();
        string cmd = "";

        foreach (string a in args) 
            if (a.StartsWith("--"))
                if (a == "--ast" || a == "--code")
                    cmd = a;

        foreach (string a in args) {
            if (!a.StartsWith("--")) {
                if (cmd == "--ast") {
                    var p = new Parser(a).Parse().GetEnumerator();

                    while(p.MoveNext()) 
                        code.Add(p.Current);

                    Console.WriteLine(JsonConvert.SerializeObject(code, Formatting.Indented));
                }
                else if (cmd == "--code") {
                    var t = new Transpiler(a).Transpile().GetEnumerator();
                    while (t.MoveNext()) 
                        Utils.Outln(t.Current);
                }
            }
        }
    }
}