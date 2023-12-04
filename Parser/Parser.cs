using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Runtime.InteropServices.JavaScript;

namespace Sphere;
public class Parser
{
    private string file = ""; 
    public Token[] source;
    private int curr = 0;

    public Parser(string file, IEnumerable<Token> source)
    {
        this.file = file;
        this.source = source.ToArray();
    }

    public IEnumerable<AST.Node> Parse() {
        while (NotAtEnd())
            yield return GetInstruction();
    }

    public bool AtEnd(int curr) => curr >= this.source.Count();
    private bool  NotAtEnd(int ahead = 0) => curr + ahead < this.source.Count();
    private Token Peek    (int ahead = 0) => this.NotAtEnd(ahead) ? this.source[curr + ahead] : new Token(TokenType.EOF, "");
    private bool Expect(Token token, int ahead = 0) => this.Peek(ahead).Type == token.Type;
    private bool Expect(TokenType type, int ahead = 0) => this.Peek(ahead).Type == type;
    private Token Next () {
        if (NotAtEnd()) {
            curr++;
            return this.Peek();
        }
        return new Token(TokenType.EOF, "");
    }
    private AST.Node GetInstruction()
    {
        while (Expect(TokenType.EOL)) Next();
        var instruction = this.Peek();
        switch (this.Peek().Type)
        {
            case TokenType.EOL:
                Next();
                break;
            case TokenType.Object:
                var obj = this.Peek();
                string name = obj.value.Remove(0, 1);
                Next();
                if (Expect(TokenType.LParen))
                    return new AST.Node(CreateFunction(name), obj.line, obj.column);

                if (!AST.Variables.ContainsKey(obj.value.Remove(0, 1)))
                    AST.Variables.Add(obj.value.Remove(0, 1), new AST.Variable(Peek()));

                break;
            case TokenType.Pragma:
                Pragma();
                break;
            case TokenType.PrintOut:
            case TokenType.PrintOutln:
                return Print();
            case TokenType.EOF: break;
            default:
                Error.Add(new(ErrorType.Syntax, file,
                    $"No such instruction like \"{instruction.value}\"", instruction.line,
                    instruction.column));
                Error.DumpErrors();
                break;
        }

        this.Next();
        return new AST.Node(AST.Type.EOF, Peek().line, Peek().column);
    }
    private AST.Pragma? Pragma() {
        int? line = this.Next().line;

        TokenType type;
        string property;
        object value;
    
        while (this.Next().line == line) {
            TokenType tok = this.Peek().Type;
            switch (tok) {
                case TokenType.Config: return new(tok, 
                    Next().value.ToString(), 
                    this.Next().value
                );
                default: 
                    Utils.Outln($"[Parser.Pragma()]: \"{this.Peek().Type}\" is either unknown or not yet implemented.");
                    break;
            }
        }
        return null;
    }
    private AST.Function CreateFunction(string name)
    {
        Dictionary<string, AST.Variable> parameters = new();
        AST.Type returnType = AST.Type.Void;
        List<AST.Node> body = new();
        
        Next();
        while (!Expect(TokenType.RParen))
        {
            if (Expect(TokenType.Colon, 1))
            {
                Next();
                parameters.Add(Peek(-1).value, new AST.Variable(Peek(1).value));
            }
            else parameters.Add(Peek().value, new AST.Variable(this.Peek(), true));
            Next();
        }
        Next();
        if (Expect(TokenType.Colon))
        {
            Next();
            returnType = this.Peek().value switch
            {
                "str" => AST.Type.String,
                "int" => AST.Type.Int,
                "bit" => AST.Type.Bit,
                "*"   => AST.Type.Any,
                 _    => AST.Type.Void
            };
        }

        Next();
        if (Expect(TokenType.LBrace))
        {
            Next();
            while (!Expect(TokenType.RBrace))
            {
                body.Add(this.GetInstruction());
                Next();
            }
            Next();
        }

        return new AST.Function(name, returnType, parameters, body);
    }
    private AST.Node Print()
    {
        Token inst = this.Peek();
        int? line = this.Peek().line;
        this.Next();
        string text = "";
        while (this.Peek().line == line)
        {
            if (Expect(TokenType.Call))
            {
                var args = new List<object>();
                var vars = new Dictionary<string, AST.Variable>();
                var name = Peek();
                if (!AST.Functions.ContainsKey(name.value))
                    Error.Add(new Error(ErrorType.Syntax, this.file, $"Unidentified function named '{name.value}' cannot be called", name.line, name.column));
                
                Next();
                while (!Expect(TokenType.RBracket)) {
                    if (!Expect(TokenType.Colon, 1))
                    {
                        Next();
                        vars.Add(Peek(-1).value, new AST.Variable(Peek(1).Type));
                    }
                }
                var invoke = new AST.Node(new AST.Instruction(AST.Type.Call, vars), inst.line, inst.column);
            }

            if (Expect(TokenType.EOL, 1) || !NotAtEnd(1))
            {
                text += $"{this.Peek().value}";
                Next();
                break;
            }
            if (!this.Expect(TokenType.Plus))
            {
                text += $"{this.Peek().value} ";
                Next();
                continue;
            }

            text = text.Remove(text.Length - 2, 1);
            this.Next();
        }

        var tok = new Token(TokenType.StringLit, text, this.Peek().line, this.Peek().column);
        return inst.Type == TokenType.PrintOutln
            ? new AST.Node(new AST.Instruction(AST.Type.Outln, tok), inst.line, inst.column)
            : new AST.Node(new AST.Instruction(AST.Type.Out,   tok), inst.line, inst.column);
    }
}