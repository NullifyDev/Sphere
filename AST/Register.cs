namespace Qbe.AST;

public class Register
{
    public string Name;
    public DataType Type;
    public object? Value;

    public Register(string Name, DataType Type)
    {
        this.Name = Name;
        this.Type = Type;
    }
    
    public Register(string Name, DataType Type, object Value)
    {
        this.Name = Name;
        this.Type = Type;
        this.Value = Value;
    }
}