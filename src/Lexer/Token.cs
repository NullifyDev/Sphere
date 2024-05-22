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
    LBracket,
    RBracket,
    LBrace,
    RBrace,
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
    Continue,

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
    Sphere, Comment, Config, EOL, EOF
}

public record Token(TokenKind Kind, string File, string Value, int Line, int Column)
{
    public override string ToString() => Kind != TokenKind.EOL ? $"{Kind switch
    {
        TokenKind.Identifier => $"{Value}",
        TokenKind.IntLit => $"{Value}",
        TokenKind.StringLit => $"{Value}",
        TokenKind.BoolLit => $"{Value}",
        TokenKind.HexLit => $"{Value}",
        _ => Value
    }} ({Kind})" : "";
}