using System.Drawing;
using System.Runtime.CompilerServices;
using Qbe.AST;

namespace Qbe;

using static Utils;
using static Global;

public class Parser
{
    public Parser(List<Token> Tokens)
    {
        Global.Tokens = Tokens;
    }

    public void Output()
    {
        foreach (var x in Tokens)
        {
            Outln($"Line: {x.line} | Column: {x.column} | Type: {x.Type} | Value: {x.value} | Length: {x.value.ToString().Length}");
        }
    }

    public Global.AST Parse()
    {
        // Outln($"[Paser]");
        Global.AST ast = new();
        List<Global.AST> asts = new();
        var Tokens = Global.Tokens;
        var showAST = true;
        while (curr < Tokens.Count && Tokens[curr].Type != TokenType.EOF)
        {
            var x = Global.Tokens[Global.curr];
            switch (x.Type)
            {
                case TokenType.mlCommentL:
                    while (++Global.curr < Global.Tokens.Count && Global.Tokens[Global.curr].Type == TokenType.mlCommentR)
                        if (Global.Tokens[Global.curr].Type == TokenType.EOF) LangErr(Global.Tokens[Global.curr], "Trailing Multi-Line Comment. Please add \"#>\" anywhere after \"<#\"");
                    break;
                
                case TokenType.slComment:
                    while (++Global.curr < Global.Tokens.Count && Global.Tokens[Global.curr].Type != TokenType.EOL) continue; 
                    break;
                
                case TokenType.Input:
                case TokenType.InputLine:
                case TokenType.PrintOut:
                case TokenType.PrintOutln:
                    ast.AddNode(new Instruction(x.line, x.column).Parse());
                    break;
                case TokenType.Call:
                    ast.AddNode(new Instruction(x.line, x.column).Parse(ast.Functions));
                    break;
                case TokenType.Func:
                    var func = new AST.Function().Parse();
                    ast.AddNode(func);
                    break;
            }

            Global.curr++;
        }
        return ast;
    }
}