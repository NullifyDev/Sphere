using Newtonsoft.Json;
using System.Text.Json;

namespace Sphere;

using Sphere.Parsers;
using Sphere.Compilation;
using Sphere.Parsers.AST;
using System.Text;

public class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        
        // Console.OutputEncoding = Encoding.ASCII;

        List<Node> code = new();
        string cmd = "";

        foreach (string a in args) 
            if (a.StartsWith("--"))
                if (a == "--ast" || a == "--code" || a == "--error")
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
                    Utils.Outln(t.Current);
                    while (t.MoveNext()) 
                        Utils.Outln(t.Current);
                }
            }
        }
    }
}