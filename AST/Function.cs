/*using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static Qbe.Utils;
using static Qbe.Global;

namespace Qbe.AST;

public class Function
{
    public string name = "";
    public List<VarNode> Parameters = new();
    public List<Instruction> Body = new();
    public List<DataType> ReturnTypes = new();
    
    public Function Parse()
    {
        var func = new Function();
                    
        if (Global.Tokens[++Global.curr].Type != TokenType.Identifier) Utils.Crash($"Expected function name. Got {Global.Tokens[Global.curr].Type} instead.");
        func.name = Global.Tokens[Global.curr].value.ToString().Replace("\"", "");
        if (Global.Tokens[++Global.curr].Type != TokenType.LBracket) Utils.Crash($"Expected '['. Got {Global.Tokens[Global.curr].Type} instead.");
        while (Global.Tokens[Global.curr++].Type != TokenType.RBracket)
            if (Global.Tokens[Global.curr].Type == TokenType.Identifier ||
                Global.Tokens[Global.curr].Type == TokenType.Star)
            {
                if (func.Parameters.Any(x => x.Data.Type == DataType.Any && x.Name == "*")) LangErr(Global.Tokens[Global.curr], "Illegal Function Parameter Definition - Cannot define parameters after Wild-Card Parameter: \"*\"");
                else func.Parameters.Add(new VarNode(Global.Tokens[Global.curr]));
            }

        if (Global.Tokens[Global.curr++].Type != TokenType.Colon) Crash($"Expected ':'. Got {Global.Tokens[Global.curr].Type} instead");
        switch (Global.Tokens[Global.curr++].Type)
        {
            case TokenType.Bool: // bit
                func.ReturnTypes.Add(DataType.Bit);
                break;
            case TokenType.Char: // single ascii code (byte)
                func.ReturnTypes.Add(DataType.Byte);
                break;
            case TokenType.Num:  // byte
                func.ReturnTypes.Add(DataType.Byte);
                break;
            case TokenType.Star: // any (byte)
                func.ReturnTypes.Add(DataType.Any);
                break;
            case TokenType.Void: // discard
            case TokenType.EOL:  // self-definable - return is optional
                func.ReturnTypes.Add(DataType.Void);
                break;
            default:
                Crash($"Expected Data Type. Got {Global.Tokens[Global.curr].Type} instead");
                break;
        }

        while (Global.curr < Global.Tokens.Count && Global.Tokens[Global.curr].Type != TokenType.End)
        {
            if (Global.Tokens[Global.curr].Type == TokenType.EOL || Global.Tokens[Global.curr].Type == TokenType.EOF)
            {
                Global.curr++;
                continue;
            }
            func.Body.Add(new Instruction().Parse(func));
            Global.curr++;
        }

        return func;
    }
}*/