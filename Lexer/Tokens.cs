namespace Qbe;

public enum TokenType
{
    // Instructions
    // Ret, Jmp, JmpTo, Comp, Func, Call, PrintOut, PrintOutln, Input, InputLine, ShiftL, ShiftR, End, Plus,
        
    // Data Types
    // Num,   Str,   Bool, Char,  Void, //<-- These Data Types are treated as the following:
//  Bin,   Bin,   Bit,   Bin,   0 
//  Bin, Ascii[], bit,  ascii,  0
   
    // Memory & pointer
    PtrUp, PtrDown, IncrAddr, DecrAddr,
    
    // Data Literal
    Identifier, NumLit, StringLit,

    // Comma, Equals,
   
    // LBracket, RBracket, Star, Colon,
    
    EOL, EOF
}

public class Token
{
    public TokenType Type;
    public string value;
    public int? line;
    public int? column;

    public Token(TokenType type, string value, int? line, int? column)
    {
        this.Type = type;
        this.value = value;
        this.line = line;
        this.column = column;
    }
}