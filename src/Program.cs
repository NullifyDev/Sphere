// using Newtonsoft.Json;
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

        foreach (string a in args)
        {
            string source = "#include \"sphere.h\"\n\n";
            var t = new Transpiler(a).Transpile().GetEnumerator();
            Utils.Outln(t.Current);
            while (t.MoveNext())
                source += $"{t.Current}\n";

            Utils.Outln(source);
        }
    }
}