using System.Collections;
using System.Reflection.Metadata;
using Qbe.AST;
// using Stack = Qbe.AST.Stack;

namespace Qbe;

using static Qbe.Utils;

public class Interpreter
{
    private int ptr = 0;
    private int[] memory;
    private int curr = 0;
    
    public void Output()
    {
        Utils.Outln($"[Interpreter: Instruction Dump]");
        foreach (var x in memory)
        {
            Utils.Out($"[ ");
            Utils.Out($"{x} ");
            Utils.Outln("]\n ");
        }
    }

    public void Run(int allocMemSize = 5)
    {
        this.memory = new int[allocMemSize];
        foreach (var ast in Global.ASTs)
        {
            for (; this.curr < ast.Instructions.Count; curr++)
            {
                Outln($"[{this.curr + 1}/{ast.Instructions.Count()}] Instruction: {ast.Instructions[this.curr].instruction} | Values: [ {string.Join(", ", ast.Instructions[this.curr].Values.Select(x => x.value))} ]"); 
                var currInst = ast.Instructions[this.curr];
                var currVal = ast.Instructions[this.curr].Values;
                switch (currInst.instruction)
                {
                    case "PtrUp": ptr = currVal.Count > 0 ? ptr - Convert.ToInt32(currVal[0].value) : ptr - 1; continue;
                    case "PtrDown": ptr = currVal.Count > 0 ? ptr + Convert.ToInt32(currVal[0].value) : ptr + 1; continue;
                    case "Incr": memory[ptr] = currVal.Count > 0 ? memory[ptr] + Convert.ToInt32(currVal[0].value) : memory[ptr] + 1; continue;
                    case "Decr": memory[ptr] = currVal.Count > 0 ? memory[ptr] - Convert.ToInt32(currVal[0].value) : memory[ptr] - 1; continue;
                    
                    #region OldCode
                    /*case "Call":
                        var targetFuncName = this.ast.Instructions[this.curr].Values[0].value;
                        var func = this.ast.Functions.FirstOrDefault(x => x.name == targetFuncName);
                        if (func == null) Crash($"Error: Could not find method named \"{targetFuncName}\"");

                        this.ast.Instructions[this.curr].Values.RemoveAt(0);
                        var callParams = this.ast.Instructions[this.curr].Values;
                        for (int i = 0; i < func.Parameters.Count; i++)
                        {
                            func.Parameters[i].Data.Type = Utils.ToDataType(callParams[i].Type);
                            if (func.Parameters[i].Data.Value is Stack) {
                                func.Parameters[i].Data.Value = func.Parameters[i].Data.Value ?? new List<object>(0);
                                if (func.Parameters[i].Name == "*")
                                {
                                    for (int j = i; j < callParams.Count; j++)
                                    {
                                        var stack = new VarNode(func.Parameters[i].Name, callParams[j]);
                                        if ((callParams.Count() - i) > 1)
                                            (func.Parameters[i].Data.Value).Add(stack);
                                        else func.Parameters[i] = stack;
                                    }
                                }
                            }
                            else
                            {
                                func.Parameters[i].Data.Value!.Add(callParams[i].value);
                            }

                            #region CheckFuncParams
                            /*Out($"Func Param: Name:   {func.Parameters[i].Name} | Type: {func.Parameters[i].Type} | ");

                            if (func.Parameters[i].isStack) {
                                func.Parameters[i].Value = func.Parameters[i].Value ?? new List<object>(0);
                                Outln($"Values: [ ");
                                this.TraverseMultiDimentionalStack(func.Parameters[i]);
                                Outln($"]");
                            } else Outln($"Value: {func.Parameters[i].Value[0]}");#1#
                            #endregion
                        }
                        // foreach(var x in func.Body)
                        // Outln($"Func Body: Instruction: {x.instruction} | Values: {string.Join(" ", x.Values.Select(x => x.value.ToString()))}");
                        new Executer(func).Execute();
                    break;*/

                    #endregion

                    default:
                        Utils.Crash(
                            $"Exception Thrwon: UnexpectedInstruction: {currInst}. Please check the spelling of the instruction. If it is still correct, please contact the developer by creating a GitHub Issue post.");
                        break;
                }

                this.curr++;
            }
        }
    }
    
    public string Compile()
    {
        return "";
    }

/* FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS
FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR
THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS FOR THE LOVE OF GOD REWRITE THIS  */
    /*public static void StackTraverse(VarNode variable)
    {
        var type = variable.Data.Type;
        var value = variable.Data.Value;
        if (type.ToString().ToLower().EndsWith("stack"))
        {
            foreach (var x in value as IEnumerable<object>)
            {
                if (x is VarNode) StackTraverse(new VarNode(x));
                else
                {
                    
                }
            }
        }
    }*/
    /*public static void StackTraverse(Token token)
    {
        Global.Variables.First(x => x.Name == token.value)
    }*/
    /*public static void FlattenList(object obj, List<object> resultList) 
    {
        // (obj is List<Variable>);
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
    }*/
}

public enum InstructionType
{
    PtrUp, PtrDown, Incr, Decr, Ret, Jmp, JmpTo, Comp, Call, PrintOut, PrintOutln, Input, InputLine
}