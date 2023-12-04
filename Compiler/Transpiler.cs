namespace Sphere.Compiler;

public class Transpiler
{
    private string file;
    private AST.Node[] ast;
    private int curr = 0;
    private AST.Node node => this.ast[curr];
    private string result = "";
    private List<AST.Pragma> Pragmas;
    public Transpiler(string file, AST.Node[] ast)
    {
        this.file = file;
        this.ast = ast;
    }

    public void Transpile()
    {

        while (this.NotAtEnd())
        {
            result += this.run(node);
            curr++;
        }
        Code.Add(Pragmas, this.file, result);
    }

    private string run(AST.Node node) {
        switch (node.Value)
        {
            case AST.Function:
                var func = (node.Value as AST.Function);
                if (AST.Functions.ContainsKey((func!).Name))
                {
                    if (AST.Functions[func.Name].Parameters
                            .Values == func.Parameters.Values)
                        Error.Add(new Error(ErrorType.Syntax, file,
                            $"Function '{func.Name}' already exists",
                            node.Line, node.Column));
                }
                string parameters = "";
                foreach (var p in func.Parameters)
                {
                    parameters += p.Value.Type switch
                    {
                        AST.Type.String => p.Key == "*" ? $"char * wildcard, " : $"char * {p.Key}, ",
                        AST.Type.Bit => p.Key == "*" ? $"bool[] wildcard, " : $"bool {p.Key}, ",
                        AST.Type.Int => p.Key == "*" ? $"int[] wildcard, " : $"int {p.Key}, ",
                        _            => p.Key == "*" ? $"char * wildcard, " : $"int {p.Key}, "
                    };
                }

                if (parameters.Length > 0)
                    parameters = parameters.Remove(parameters.Length - 2, 2);

                string instructions = "";
                foreach (var n in func.Body)
                    instructions += $"\t{this.run(n)}";

                result += $"{func.ReturnType.ToString().ToLower()} {func.Name}({parameters}) {{\n{instructions}}}\n";
                return result;
            case AST.Instruction: 
                return this.ParseInstruction((node.Value as AST.Instruction)!);
            
            case AST.Variable:
            case AST.Vector: 
                break;

            case "EOF": return result;
            default:
                if (node.Value.ToString() == "EOF")
                    return "";

                Utils.Outln($"Uuhhhh... {node.Value}");
                //throw new ArgumentOutOfRangeException();
                break;
        }

        return "";
    }

    private string ParseInstruction(AST.Instruction inst)
    {
        switch (inst.Type)
        {
            case AST.Type.Out:
            case AST.Type.Outln:
                return inst.Type == AST.Type.Outln ? $"printf(\"{((inst.Args as Token)!).value}\\n\");\n" : $"printf(\"{((inst.Args as Token)!).value}\");\n";
        }

        return "";
    }

    public bool NotAtEnd() => curr < this.ast.Count();
}