using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphere.Lexer;

public enum TokenKind
{
    ERROR = 0,
    RMLComment,
    LMLComment,
    Plus,
    PlusEq,
    Minus,
    MinusEq,
    Star,
    StarEq,
    Slash,
    SlashEq,
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
    BangEq,
    Modulo,
    And,
    Or,
    In,
    Continue,
    Equal,
    EqualEq,
    DoubleEq,
    Less,
    LessEq,
    Greater,
    GreaterEq,
    PtrIncr, PtrDecr, Mov,

    Out, Outln, Input, Inputln, If, Elif, Else, For, While, Up, Down,

    Identifier, IntLit, HexLit, StringLit, BoolLit, Return,

    DataType_Void, DataType_Int, DataType_String, DataType_Bool,

    Sphere, SLComment, Comment, Config, EOL, EOF
}

public record class Token
{
    public TokenKind Kind;
    public string File;
    public string Value;
    public int Line;
    public int Column;

    public Token(TokenKind kind, string file, string value, int line, int column)
    {
        this.Kind = kind;
        this.File = file;
        this.Value = value;
        this.Line = line;
        this.Column = column;
    }

    public override string ToString() => this.Kind != TokenKind.EOL ? $"{this.Kind switch
    {
        TokenKind.Identifier => $"{Value}",
        TokenKind.IntLit => $"{Value}",
        TokenKind.StringLit => $"{Value}",
        TokenKind.BoolLit => $"{Value}",
        TokenKind.HexLit => $"{Value}",
        _ => Value
    }} ({Kind})" : "";
}