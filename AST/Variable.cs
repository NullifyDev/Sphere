/*
using System.Reflection.Metadata;

namespace Qbe.AST;

public class VarNode
{
    public string Name { get; set; }
    public IVariable Data { get; set; }

    public VarNode(string name, DataType type, object? value = null)
    {
        this.Name = name;
        this.Data.Type = type; 
        this.Data.Value = this.Data.Type.ToString().EndsWith("Stack") ? new Stack() : new Variable(); 
    }

    public VarNode(string name, Token token)
    {
        this.Name = name;
    
        switch (token.Type)
        {
            case TokenType.Str:
                this.Data.Type = DataType.AsciiStack;
                this.Data.Value = new Stack();
                break;
            case TokenType.Star:
                this.Data.Type = DataType.AnyStack;
                this.Data.Value = new Stack();
                break;
            default:
                this.Data.Type = DataType.Any;
                this.Data.Value = new Variable();
                break;
            // Add other cases as needed
        }
    }
    public VarNode(Token token)
    {
        this.Name = token.value;
    
        switch (token.Type)
        {
            case TokenType.Str:
                this.Data.Type = DataType.AsciiStack;
                this.Data.Value = new Stack();
                break;
            case TokenType.Star:
                this.Data.Type = DataType.AnyStack;
                this.Data.Value = new Stack();
                break;
            default:
                this.Data.Type = DataType.Any;
                this.Data.Value = new Variable();
                break;
            // Add other cases as needed
        }
    }
    public VarNode(Token token)
    {
        this.Name = token.value;
    
        switch (token.Type)
        {
            case TokenType.Str:
                this.Data.Type = DataType.AsciiStack;
                this.Data.Value = new Stack();
                break;
            case TokenType.Star:
                this.Data.Type = DataType.AnyStack;
                this.Data.Value = new Stack();
                break;
            default:
                this.Data.Type = DataType.Any;
                this.Data.Value = new Variable();
                break;
            // Add other cases as needed
        }
    }
}

public interface IVariable
{
    DataType Type { get; set; }
    object Value { get; set; }
}

public struct Stack : IVariable
{
    public DataType Type { get; set; }
    public object Value { get; set; }

    public Stack(string name, DataType type, int[]? value = null, int? length = null)
    {
        this.Type = type;
        this.Value = value == null ? new int[length ?? 0] : value;
    }
}

public struct Variable : IVariable
{
    public DataType Type { get; set; }
    public object Value { get; set; }

    public Variable(string name, DataType type, int? value = null)
    {
        this.Type = type;
        this.Value = value ?? 0;
    }
}
*/
    