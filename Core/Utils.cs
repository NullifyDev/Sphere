namespace Qbe;

public static class Utils
{
    public static void Out(string str = "") => Console.Write(str);
    public static void Outln(string str = "") => Console.WriteLine(str);
    public static void Wait(int ms = 0) => Task.Delay(ms);
    public static void LangErr(Token token, string msg = "Unspecified Error") => Crash($"Line: {token.line} | {msg}");

    public static void Crash(string msg)
    {
        Outln($"Error: {msg}");
        Environment.Exit(0);
    }

    public static DataType ToDataType(TokenType tType)
    {
        // Outln($"[Utils] Token ToDataType: {tType}");
        return tType switch
        {
            TokenType.Bool => DataType.Bit,
            TokenType.Char => DataType.Byte,
            TokenType.Num  => DataType.Byte,
            TokenType.Str  => DataType.Ascii,
            TokenType.Star => DataType.Any,
            _              => DataType.Any
        };
    }
    public static DataType ToDataType(Token token) => ToDataType(token.Type);
}