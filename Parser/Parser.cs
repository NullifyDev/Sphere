using System.Drawing;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Qbe.AST;

namespace Qbe;

using static Utils;
using static Global;

public class Parser
{
    private string File = "";
    // public Parser(IEnumerable<Token> tokens)
    // {
    //     Global.Tokens = tokens;
    // }

    
    public Parser(string file)
    {
        this.File = file;
        // Global.Tokens = new Lexer(System.IO.File.ReadAllText(this.File).Replace("\r\n", "\n").Replace("\r", "\n")).Lex();
    }

    
    
    public void Output()
    {
        int curr = 0;
        var lex = new Lexer(System.IO.File.ReadAllText(this.File).Replace("\r\n", "\n").Replace("\r", "\n")).Lex();
        while (curr > lex.Count())
        {
            var tok = Peek();
            Utils.Outln($"[{Peek().line}:{tok.column}]: {tok.Type} | {tok.value}");
            lex.GetEnumerator().MoveNext();
        }
        // foreach(var tok in Tokens)
        // {
        //     Utils.Outln($"[{tok.line}:{tok.column}]: {tok.Type} | {tok.value}");
        // }
    }
    
    public void run()
    {
        var lex = new Lexer(this.File);
        Global.Tokens = lex.Lex();
        while (lex.NotAtEnd())
        {
            string instruction = "";
            List<Token> values = new();

            for (int i = 0; i < Global.Tokens.Count(); i++)
            {
                var tok = Peek();
                switch (tok.Type)
                {
                    case TokenType.PtrUp:
                    case TokenType.PtrDown:
                    case TokenType.IncrAddr:
                    case TokenType.DecrAddr:
                        if (values.Count > 0)
                        {
                            new ASTNodes().Instructions.Add(new(instruction, values));
                            instruction = "";
                            values.Clear();
                        }

                        if (i + 1 >= Tokens.Count()) new ASTNodes().Instructions.Add(new(instruction));
                        else
                        {
                            if (instruction == "") instruction = tok.Type.ToString();
                            else
                            {
                                new ASTNodes().Instructions.Add(new(instruction));
                                instruction = "";
                            }
                        }

                        continue;
                    default:
                        switch (instruction)
                        {
                            case "PtrUp":
                            case "PtrDown":
                            case "IncrAddr":
                            case "DecrAddr":
                                values.Add(tok);
                                continue;
                        }

                        continue;
                }
            }
        }
        // foreach (var x in Global.Tokens)
        // {
        //     Outln($"Line: {x.line} | Column: {x.column} | Type: {x.Type} | Value: {x.value} | Length: {x.value.ToString().Length}");
        // }
    }

    public Global.ASTNodes Parse()
    {
        Outln($"[Parser]");
        Global.ASTNodes ast = new();
        var Tokens = Global.Tokens;
        var showAST = true;
        while (curr < Tokens.Count() && Peek().Type != TokenType.EOF)
        {
            /*string instruction = "";
            List<Token> values = new();

            for (int i = 0; i < Tokens.Count(); i++)
            {
                var tok = Peek();
                switch (tok.Type)
                {
                    case TokenType.PtrUp:
                    case TokenType.PtrDown:
                    case TokenType.IncrAddr:
                    case TokenType.DecrAddr:
                        if (values.Count > 0)
                        {
                            new ASTNodes().Instructions.Add(new(instruction, values));
                            instruction = "";
                            values.Clear();
                        }

                        if (i + 1 >= Tokens.Count()) new ASTNodes().Instructions.Add(new(instruction));
                        else
                        {
                            if (instruction == "") instruction = tok.Type.ToString();
                            else
                            {
                                new ASTNodes().Instructions.Add(new(instruction));
                                instruction = "";
                            }
                        }

                        continue;
                    default:
                        switch (instruction)
                        {
                            case "PtrUp":
                            case "PtrDown":
                            case "IncrAddr":
                            case "DecrAddr":
                                values.Add(tok);
                                continue;
                        }

                        continue;
                }
            }*/
        }
        return ast;
    }
    
    public Token Poll()
    {
        var enumerator = Global.Tokens.GetEnumerator();
        if (enumerator.MoveNext())
        {
            return enumerator.Current;
        }

        return null; // No more tokens
    }


    public Token Peek()
    {
        // Create a copy of the current position to restore it later
        int currentPosition = curr;

        // Get the next token without advancing the position
        Token peekedToken = Global.Tokens.FirstOrDefault();

        // Restore the original position
        curr = currentPosition;

        return peekedToken;

        return null; // No more tokens
    }   

}













 /*var x = Global.Tokens[Global.curr];
            switch (x.Type)
            {
                case TokenType.PtrDown:
                case TokenType.PtrUp:
                case TokenType.IncrAddr:
                case TokenType.DecrAddr:
                    var inst = new Instruction(x.line, x.column).Parse();
                    
                    // debugging
                    Out($"Instruction:          ".Remove(13, inst.instruction.Length)
                        .Insert(13, inst.instruction) + "| Value: ");
                    if (inst.Values.Count > 0) 
                        foreach (var y in inst.Values)
                        {
                            Outln($"{y.value}\n");
                        }
                    else Outln();

                    ast.AddNode(inst);
                    break;
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
                    Outln($"func {func.name} {func.Parameters.Select(x => "[ " + x.Name+ ": " + x.Data.Type + ": " + x.Data.Value + " ]")}");
                    break;
                
                default:
                    switch (this.instruction)
                    {
                        case "PtrUp":
                        case "PtrDown":
                        case "IncrAddr":
                        case "DecrAddr":
                            var currTok = Tokens[i];
                            if (currTok.Type == TokenType.NumLit) this.Values.Add(currTok);
                            else Utils.LangErr(currTok, $"Expected Number Literate. Received: {currTok.value} as {currTok.Type.ToString()} ");
                            continue;
                        
                    }
                    break;
            }

            Global.curr++;*/