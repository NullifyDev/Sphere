using System.Collections;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Serialization;

namespace Qbe.AST;

public class Instruction
{
    public string instruction = "";
    public int? line;
    public int? col;
    public List<Token> Values = new();

    public Instruction(int? line, int? col)
    {
        this.line = line;
        this.col = col;
    }


    public Instruction(string instruction, List<Token> values)
    {
        this.instruction = instruction;
        this.Values = values;
    }
    public Instruction(string instruction)
    {
        this.instruction = instruction;
        this.Values = new();
    }
}