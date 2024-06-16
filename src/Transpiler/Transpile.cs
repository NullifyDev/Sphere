using Sphere.Parsers.AST;
using Sphere.Parsers;

namespace Sphere.Compilation;

public partial class Transpiler {
    
    private IEnumerator<Node>? parser;

    public Transpiler(string name = "") {
        parser = new Parser(name).Parse().GetEnumerator();
    }

    public Transpiler() {
        
    }
    public string Transpile(List<Node> nodes) {
        return string.Join("\n", nodes.Select(x => Transpile(x)));
    }

    public IEnumerable<string> Transpile(Node? node = null) {
        var curr = node ?? this.Peek();
        while (isAtEnd()) {
            switch (Peek()) {
                case Instructions.If If:
                    ParseConditions(If.Cond);
                    yield return $"if ({ParseConditions(If.Cond)}) \n{{\n{Transpile(If.Body)} }}";
                    break;
                case Instructions.Elif Elif:

                    break;
                case Instructions.Else Else:

                    break;
                case Instructions.Mov Mov:

                    break;
                case Instructions.Incr Incr:

                    break;
                case Instructions.Decr Decr:

                    break;
                case Instructions.Out Out:

                    break;
                case Instructions.Outln Outln:

                    break;
                case Instructions.In In:

                    break;
                case Instructions.Inln Inln:

                    break;
                case Instructions.Continue Continue:

                    break;
                case Instructions.For For:

                    break;
                case Instructions.Foreach Foreach:

                    break;
                case Instructions.Function Function:

                    break;
                case Instructions.While While:

                    break;
                case Instructions.Sphere Sphere:

                    break;
                case Instructions.Sphereln Sphereln:

                    break;
                case Instructions.Return Return:

                    break;
            }
        }
        yield return "";
    }

    private Node Peek() => parser.Current;
    private Node Next() {
        isAtEnd();
        return Peek();
    }
    private bool isAtEnd() => !parser.MoveNext();
}