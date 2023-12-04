using OneOf;

namespace Sphere.Exceptions;

using static Sphere.Utils;

public class UnknownDataType : Exception
{
    object dType;
    public UnknownDataType(AST.Type type)
    {
        this.dType = type;
        Crash($"Unknown Data Type: Type '{this.dType}' is either unknown or null");
    }
    
    public UnknownDataType(TokenType type)
    {
        this.dType = type;
        Crash($"Unknown Data Type: Type '{this.dType}' is either unknown or null");
    }

    public UnknownDataType(string message, Type type) : base(message)
    {
        this.dType = type;
        Crash(message);
    }

    public UnknownDataType(string message, TokenType type) : base(message)
    {
        this.dType = type;
        Crash(message);
    }
}