namespace Sphere;

public enum TokenType
{
    // Instructions
    Ret, Jmp, If, Func, Call, PrintOut, PrintOutln, Input, InputLine, Plus, LParen, RParen, LBracket, RBracket, LBrace, RBrace,
    Colon, MThan, LThan, Equals, Star, Pipe, Object, Exit, LTEqu, MTEqu, NEqu,
   
    // Memory & pointer manipulation
    PtrUp, PtrDown, IncrAddr, DecrAddr,
    
    // Data Literal
    Identifier, NumLit, StringLit, BoolLit,

    // Compiler Instruction Tokens
    Pragma, Config,
    
    EOL, EOF
}

public class Token
{
    public TokenType Type;
    public string value;
    public int? line;
    public int? column;

    public Token(TokenType type, char value, int? line = null, int? column = null)
    {
        this.Type = type;
        this.value = value.ToString();
        this.line = line;
        this.column = column;
    }
    public Token(TokenType type, string value, int? line = null, int? column = null)
    {
        this.Type = type;
        this.value = value;
        this.line = line;
        this.column = column;
    }
}