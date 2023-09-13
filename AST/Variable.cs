using System.Reflection.Metadata;

namespace Qbe.AST;

using static Qbe.Global;

public class Variable
{
    public DataType? Type = null;
    public string Name;
    public bool isStack = false;
    public List<object>? Value;

    public Variable(int length)
    {
        this.Value = new(length);
    }

    public Variable(string name)
    {
        this.Name = name;
    }

    public Variable(string name, Token token)
    {
        this.Name = name;
        this.Type = token.Type switch
        {
            TokenType.Bool => DataType.Bit,
            TokenType.Char => DataType.Byte,
            TokenType.Num  => DataType.Byte,
            TokenType.Str  => DataType.Ascii,
            TokenType.Star => DataType.Any,
            _              => DataType.Null
        };
        this.isStack = token.Type == TokenType.Star;
        this.Value = new List<object>();
        foreach(var x in token.value)
            this.Value.Add(x);
        
        // Utils.Outln($"[Variable] Name: {this.Name} | Type: {this.Type} | Value: {string.Join("",this.Value.Select(x => x.ToString()))}");
    }
    
    public Variable(Token token)
    {
        this.Name = token.value.ToString()!;
        this.Type = token.Type switch
        {
            TokenType.Bool => DataType.Bit,
            TokenType.Char => DataType.Byte,
            TokenType.Num  => DataType.Byte,
            TokenType.Str  => DataType.Ascii,
            TokenType.Star => DataType.Any,
            _              => DataType.Null
        };
        this.isStack = token.Type == TokenType.Star;
        this.Value = new List<object>(0);
    }

    public Variable(Token token, Variable? var = null)
    {
        // Utils.Outln($"[Variable] token to DataType: {token.Type} | Value: {token.value}");
        this.Name = token.value.ToString()!;
        this.Type = token.Type switch
        {
            TokenType.Bool => DataType.Bit,
            TokenType.Char => DataType.Byte,
            TokenType.Num  => DataType.Byte,
            TokenType.Str  => DataType.Ascii,
            TokenType.Star => DataType.Any,
            _              => DataType.Null
        };
        this.isStack = token.Type == TokenType.Star;
        this.Value = new List<object>(0);
    }

    public Variable Parse()
    {
        this.Name = Tokens[++curr].value[0].ToString();
        if (Tokens[++curr].Type == TokenType.Equals)
            this.Type = Tokens[++curr].Type switch
            {
                TokenType.Bool => DataType.Bit,
                TokenType.Char => DataType.Byte,
                TokenType.Num => DataType.Byte,
                TokenType.Str => DataType.Byte,
                TokenType.Star => DataType.Any
            };

        var tempTok = Tokens[++curr];
        this.Value.Add(Tokens[++curr].value);

        return this;
    }
}