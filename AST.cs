namespace Qbe;

public static partial class Global
{
    public class ASTNodes
    {
        public List<Qbe.AST.Instruction> Instructions = new();
        // public List<Qbe.AST.Function> Functions = new();
        // public List<Qbe.AST.VarNode> Variables = new();

        
        public void AddNode(Qbe.AST.Instruction node) => Instructions.Add(node);
        // public void AddNode(Qbe.AST.Function node) => Functions.Add(node);
        // public void AddNode(Qbe.AST.VarNode node) => Variables.Add(node);
    }
}