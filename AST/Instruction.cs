using System.Runtime.InteropServices.JavaScript;

namespace Qbe.AST;

using static Qbe.Utils;
using static Qbe.Global;

public class Instruction
{
    public string instruction = "";
    public int line;
    public int col;
    public List<Token> Values = new();

    private Function Func = new();
    private List<Qbe.AST.Function> Funcs = new();

    public Instruction(int line, int col)
    {
        this.line = line;
        this.col = col;
    }
    
    public Instruction Parse(Function? func)
    {
        this.Func = func;
        return Parse();
    }

    public Instruction() {}

    public Instruction Parse(List<Qbe.AST.Function> funcs)
    {
        this.Funcs = funcs;
        return Parse();
    }

    public Instruction Parse()
    {
        var InstructionToken = Global.Tokens[curr];
        var InstructionTokenType = InstructionToken.Type;
        
        var argus = "";
        switch (InstructionTokenType)
        {
            case TokenType.PtrUp:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.PtrDown:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.Incr:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.Decr:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.Ret:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.Jmp:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.JmpTo:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.Comp:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.Call:
                this.instruction = InstructionTokenType.ToString();
                ++Global.curr;
                this.Values.Add(Global.Tokens[Global.curr++]);

                if (Global.Tokens[Global.curr].Type == TokenType.LBracket)
                    while (Global.Tokens[++Global.curr].Type != TokenType.RBracket)
                        this.Values.Add(Global.Tokens[Global.curr]);

                // Outln($"{this.instruction} | {string.Join(" ", this.Values.Select(x => x.value))}");
                return this;
            case TokenType.PrintOut:
                this.instruction = InstructionTokenType.ToString();
                this.line = Global.Tokens[Global.curr].line;
                this.col = Global.Tokens[Global.curr].column;
                while (++Global.curr < Global.Tokens.Count && !(Global.Tokens[Global.curr].Type == TokenType.EOL ||
                                                                Global.Tokens[Global.curr].Type == TokenType.EOF ||
                                                                Global.Tokens[Global.curr].Type == TokenType.End))
                    this.Values.Add(Global.Tokens[Global.curr]);
                
                return this;
            case TokenType.PrintOutln:
                this.instruction = InstructionTokenType.ToString();
                this.line = Global.Tokens[Global.curr].line;
                this.col = Global.Tokens[Global.curr].column;
                while (++Global.curr < Global.Tokens.Count && !(Global.Tokens[Global.curr].Type == TokenType.EOL ||
                                                                Global.Tokens[Global.curr].Type == TokenType.EOF ||
                                                                Global.Tokens[Global.curr].Type == TokenType.End))
                    this.Values.Add(Global.Tokens[Global.curr]);
                
                return this;
            case TokenType.Input:
                this.instruction = InstructionTokenType.ToString();
                return this;
            case TokenType.InputLine:
                this.instruction = InstructionTokenType.ToString();
                return this;
            // case TokenType.End:
                // break;
            default:
                Utils.Crash($"[Instruction]: Line: {InstructionToken.line} | \"{InstructionToken.value}\" (Type: {InstructionToken.Type}) does not exist as an instruction.");
                break;
        };
        return this;
    }


    private string CheckFuncParams()
    {
        string argus = "";
        foreach (var y in this.Func.Parameters)
        {
            if (y.Name == Global.Tokens[Global.curr++].value)
            {
                foreach (var z in y.Value)
                        argus += $"{z} ";
                
                argus += $"{y.Value} ";
            }
        }

        return argus;
    }
}