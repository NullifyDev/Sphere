using OneOf.Types;

namespace Sphere;

public class Transpiler
{
    private string file;
    private AST.Node[] ast;
    private int curr = 0;
    private AST.Node node => this.ast[curr];
    private string result = "";
    public Transpiler(string file, AST.Node[] ast)
    {
        this.file = file;
        this.ast = ast;
    }

    public void Transpile()
    {
        while (this.NotAtEnd())
            result += this.run(node);

        Code.Add(this.file, result);
    }

    private string run(AST.Node node) {
        switch (node.Value)
        {
            case AST.Function:
                if (AST.Functions.ContainsKey((node.Value as AST.Function).Name))
                {
                    if (AST.Functions[(node.Value as AST.Function).Name].Parameters
                            .Values == (node.Value as AST.Function).Parameters.Values)
                        Error.Add(new Error(ErrorType.Syntax, file,
                            $"Function '{(node.Value as AST.Function).Name}' already exists",
                            node.Line, node.Column));
                }

                var func = node.Value as AST.Function;
                string parameters = "";
                foreach (var p in func.Parameters)
                {
                    parameters += p.Value.Type switch
                    {
                        AST.Type.String => "char *",
                        AST.Type.Bit => "bool",
                        AST.Type.Void => "void",
                        AST.Type.Int => "int"
                    };
                    parameters += $" {p.Key}, ";
                }

                if (parameters.Length > 0)
                    parameters.Remove(parameters.Length - 2, 2);

                string instructions = "";
                foreach(var n in func.Body)
                    instructions += this.run(n) + '\n';
                    
                result += @$"{func.ReturnType} {func.Name}({parameters}) {{
                    {instructions}
                }}";
                return result;
            case AST.Instruction: return ParseInstruction(node.Value as AST.Instruction);
            
            case AST.Variable:
            case AST.Vector: 
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return "";
    }

    private string ParseInstruction(AST.Instruction inst)
    {
        switch (inst.Type)
        {
            case AST.Type.Out:
            case AST.Type.Outln:
                return inst.Type == AST.Type.Outln ? $"{(inst.Args as Token).value}\n" : $"(inst.Args as Token).value";
        }

        return "";
    }

    public bool NotAtEnd() => curr < this.ast.Count();
    private void Write()
    {
        File.WriteAllText("imports.h", "#ifndef imports \n#define imports\n\n#include <stdio.h>\n\n#endif");
        foreach (var c in Code.Codes)
            File.AppendAllText(c.name,  c.content);
    }
}

