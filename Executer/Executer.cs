using System.Reflection.Metadata;
using Qbe.AST;

namespace Qbe;

using static Qbe.Utils;

public class Executer
{
    private int curr = 0;
    private Global.AST ast = new();

    private List<Function> funcs = new();
    private List<Instruction> insts = new();

    
    public Executer(Global.AST tokens)
    {
        this.ast = tokens;
    }
    
    public Executer(Qbe.AST.Function func)
    {
        this.ast.Functions.Add(func);
        this.ast.Instructions = func.Body;
        this.ast.Variables = func.Parameters;
    }

    public void Execute()
    {
        while (this.curr < ast.Instructions.Count())
        {
            // Outln($"[{this.curr + 1}/{ast.Instructions.Count()}] Instruction: {this.ast.Instructions[this.curr].instruction} | Values: [ {string.Join(", ", this.ast.Instructions[this.curr].Values.Select(x => x.value))} ]"); 
            var instruction = this.ast.Instructions[this.curr];
            switch (instruction.instruction)
            {
                case "PtrUp":
                    break;
                case "PtrDown":
                    break;
                case "Incr":
                    break;
                case "Decr":
                    break;
                case "Ret":
                    break;
                case "Jmp":
                    break;
                case "JmpTo":
                    break;
                case "Comp":
                    break;
                case "Call":
                    var targetFuncName = this.ast.Instructions[this.curr].Values[0].value; 
                    var func = this.ast.Functions.FirstOrDefault(x => x.name == targetFuncName);
                    if (func == null) Crash($"Error: Could not find method named \"{targetFuncName}\"");
                    
                    this.ast.Instructions[this.curr].Values.RemoveAt(0);
                    var callParams = this.ast.Instructions[this.curr].Values;
                    for (int i = 0; i < func.Parameters.Count; i++)
                    {
                        func.Parameters[i].Type = Utils.ToDataType(callParams[i].Type);
                        if (func.Parameters[i].isStack) {
                            func.Parameters[i].Value = func.Parameters[i].Value ?? new List<object>(0);
                            if (func.Parameters[i].Name == "*")
                            {
                                for (int j = i; j < callParams.Count; j++)
                                {
                                    var stack = new Variable(func.Parameters[i].Name, callParams[j]);
                                    if ((callParams.Count() - i) > 1)
                                        func.Parameters[i].Value.Add(stack);
                                    else func.Parameters[i] = stack;
                                }
                            }
                        }
                        else
                        {
                            func.Parameters[i].Value!.Add(callParams[i].value);
                        }
                        
                        #region CheckFuncParams
                        /*Out($"Func Param: Name:   {func.Parameters[i].Name} | Type: {func.Parameters[i].Type} | ");

                        if (func.Parameters[i].isStack) {
                            func.Parameters[i].Value = func.Parameters[i].Value ?? new List<object>(0);
                            Outln($"Values: [ ");
                            this.TraverseMultiDimentionalStack(func.Parameters[i]);
                            Outln($"]");
                        } else Outln($"Value: {func.Parameters[i].Value[0]}");*/
                        #endregion
                    }
                    // foreach(var x in func.Body)
                    // Outln($"Func Body: Instruction: {x.instruction} | Values: {string.Join(" ", x.Values.Select(x => x.value.ToString()))}");
                    new Executer(func).Execute();
                break;
                case "PrintOut":
                case "PrintOutln":
                    string str = "";
                    foreach (var e in instruction.Values)
                    {
                        Outln(e.value.ToString());
                        switch (e.Type) {
                            case TokenType.Str:
                            case TokenType.Bool:
                            case TokenType.Num:
                            case TokenType.Char:
                                str += $"{e.value} ";
                                break;
                            case TokenType.Plus:
                                str = str.Length > 0 ? str.Remove(str.Length-1, 1) : str;
                                break;
                            case TokenType.Identifier:
                            case TokenType.Star:
                                var x = (e.value);
                                // foreach (var y in x)
                                // {
                                //     
                                // }

                                break;
                        }
                    }
                    if (instruction.instruction == "PrintOutln") Outln(str);
                    else Out(str);
                    str = "";
                    break;
                case "Input":
                    Console.ReadKey();
                    break;
                case "InputLine":
                    Console.ReadLine();
                    break;
                default:
                    Utils.Crash($"Unexpected instruction: {instruction}. Please contact the developer if neccessary by creating a GitHub Issue post.");
                    break;
            }
            this.curr++;
        }
    }
    
    private void TraverseMultiDimentionalStack(Variable x) => TraverseMultiDimentionalStack(x.Value);
    private void TraverseMultiDimentionalStack(Token x) => TraverseMultiDimentionalStack(x);
    private void TraverseMultiDimentionalStack(List<object> x)
    {
        foreach (var y in x)
        {
            Out($"    [ ");
            var e = y as Variable;
            if (y is List<object>)
            {
                foreach (var z in (y as List<object>))
                {
                    if (z is List<object>) TraverseMultiDimentionalStack(z as List<object>);
                    e = z as Variable;
                    Out($"{e.Value} ");
                }
            }
            else
            {
                if (e.Type == DataType.Ascii) Out($"\"");
                foreach (var z in e.Value)
                    Out($"{z}");
                
                if (e.Type == DataType.Ascii) Out($"\"");
            }
            Outln($" ]");
        }
    }
    
    public static void FlattenList(object obj, List<object> resultList) 
    {
        (obj is List<Variable>);
        var list = obj as List<Variable> ?? new List<Variable>();
        if (list != null) 
        {
            foreach (var item in list) 
            {
                FlattenList(item, resultList);
            }
        } 
        else 
        {
            resultList.Add(obj);
        }
    }

}

public enum InstructionType
{
    PtrUp, PtrDown, Incr, Decr, Ret, Jmp, JmpTo, Comp, Call, PrintOut, PrintOutln, Input, InputLine
}