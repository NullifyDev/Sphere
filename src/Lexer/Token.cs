using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphere.Lexer;

public enum TokenKind
{
    RMLComment,
    LMLComment,
    // Single character tokens
    Plus, 
    Minus, 
    Star, 
    Slash, 
    LParen, 
    RParen, 
    LSquare, 
    RSquare, 
    LCurly, 
    RCurly,
    Dot, 
    Comma, 
    Pipe, 
    Colon, 
    Dollar, 
    At, 
    AtPrefix,
    Bang,
    Modulo,
    And,
    Or,
    In,

    // Multi-character tokens
    Equal,
    BangEq,
    DoubleEq,
    Less,
    LessEq,
    Greater,
    GreaterEq,

    // Instructions 
        // Memory Manipulation
        PtrIncr, PtrDecr, Mov, 

        // Other
        Out, Outln, Input, Inputln, If, Elif, Else, For, While,

    // Literal
    Identifier, IntLit, HexLit, StringLit, BoolLit, Return,

    // Misc 
    Pragma, Comment, Config, EOL, EOF
}

public record Token(TokenKind Kind, string Value, int Line, int Column)
{
    public override string ToString() => $"{Kind switch {
        TokenKind.Identifier => $"{Value}",
        TokenKind.IntLit => $"{Value}",
        TokenKind.StringLit => $"{Value}",
        TokenKind.BoolLit => $"{Value}",
        TokenKind.HexLit => $"{Value}",

        TokenKind.EOF => "EOF",
        TokenKind.EOL => "EOL",
        _ => Value
    }} ({Kind})";
}