using Sphere.Parsers.AST;
using Sphere.Parsers;

namespace Sphere.Compilation;

public partial record Transpiler {
    public string ParseConditions(Node cond) {
        switch (cond.Type) {
            case "Literal":
                break;
        }
        
        return "";
    }
}