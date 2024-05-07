using System.Collections.Generic;

namespace Sphere.Lexer;
public class Lexer
{
    public string source;
    private string file;
    private int curr = 0;
    private int line = 1;
    private int column = 1;

    public Lexer(string file, string source)
    {
        this.file = file;
        this.source = source;
    }

    public IEnumerable<Token> Lex()
    {
        while (NotAtEnd())
        {
            switch (this.Peek())
            {
                case '\"': yield return ScanString(); break;
                case '\n':
                case '\r':
                    yield return new Token(TokenKind.EOL, this.Peek().ToString(), this.line, this.column);
                    if (Peek(1) == '\n') Next();
                    line++;
                    this.column = 0;
                    break;
                case '(': yield return new Token(TokenKind.LParen, this.Peek().ToString(), this.line, this.column); break;
                case ')': yield return new Token(TokenKind.RParen, this.Peek().ToString(), this.line, this.column); break;
                case '[': yield return new Token(TokenKind.LSquare, this.Peek().ToString(), this.line, this.column); break;
                case ']': yield return new Token(TokenKind.RSquare, this.Peek().ToString(), this.line, this.column); break;
                case '}': yield return new Token(TokenKind.RCurly, this.Peek().ToString(), this.line, this.column); break;
                case '{': yield return new Token(TokenKind.LCurly, this.Peek().ToString(), this.line, this.column); break;
                case '$': yield return new Token(TokenKind.Dollar, this.Peek().ToString(), this.line, this.column); break;
                case '@': yield return new Token(TokenKind.AtPrefix, this.Peek().ToString(), this.line, this.column); break;
                case '*': yield return new Token(TokenKind.Star, this.Peek().ToString(), this.line, this.column); break;
                case '%': yield return new Token(TokenKind.Modulo, this.Peek().ToString(), this.line, this.column); break;
                case '=': 
                    if (Next() == '=') 
                        yield return new Token(TokenKind.DoubleEq, "==", this.line, this.column);
                    else 
                        yield return new Token(TokenKind.Equal, "=", this.line, this.column); 
                    break;

                case '<': yield return new Token(TokenKind.Less, this.Peek().ToString(), this.line, this.column); break;
                case '>': yield return new Token(TokenKind.Greater, this.Peek().ToString(), this.line, this.column); break;
                case '|': yield return new Token(TokenKind.Pipe, this.Peek().ToString(), this.line, this.column); break;
                case '+': yield return new Token(TokenKind.Plus, this.Peek().ToString(), this.line, this.column); break;
                case ':': yield return new Token(TokenKind.Colon, this.Peek().ToString(), this.line, this.column); break;
                case '#':
                    if (Next() == '>') {
                        while (Next() != '<')
                            if (Next()!= '#') continue;
                    }
                    while (Next() != '\n') {}
                    break;
                default:
                    if (Peek() == '#') {
                        if (Next() == '>') {
                            while (Next() != '<')
                                if (Next()!= '#') 
                                    continue;
                        }
                        while (Next() != '\n') 
                            continue;
                    }
                    if (char.IsLetter(this.Peek()))
                    {
                        yield return ScanIdentifier();
                        continue;
                    }
                    else if (char.IsDigit(this.Peek()))
                    {
                        yield return ScanNumber();
                        continue;
                    }
                    break;
            }
            Next();
        }
        yield return new Token(TokenKind.EOF, "EOF", line, column);
    }

    private Token ScanIdentifier()
    {
        int col = this.column;
        int start = curr;
        while (NotAtEnd() && char.IsLetterOrDigit(Peek()) || NotAtEnd() && Peek() == '_')
            Next();

        string identifier = source.Substring(start, curr - start);

        if (reserved.TryGetValue(identifier, out TokenKind TokenKind))
            return new Token(TokenKind, identifier, this.line, col);

        return new Token(TokenKind.Identifier, identifier, this.line, col);
    }

    private Token ScanNumber()
    {
        int col = this.column;
        int start = curr;
        while (NotAtEnd() && char.IsLetterOrDigit(Peek()))
            Next();

        // return new Token(TokenKind.NumLit, source.Substring(start, curr - start), this.line, start+1);
        string number = source.Substring(start, curr - start);
        if (number.StartsWith("0x")) {
            for(int i = 2; i < number.Length; i++) {
                int ascii = (int)number[i];
                     if (ascii >= 97 && ascii <= 102) continue;
                else if (ascii >= 65 && ascii <= 75) continue;
                else if (ascii >= 48 && ascii <= 57) continue;
                else throw new Exception($"Invalid Hexadecimal format | {this.file}{this.line}:{col}");
            }
            return new Token(TokenKind.HexLit, source.Substring(start, curr - start), this.line, col);
        }
        return new Token(TokenKind.IntLit, source.Substring(start, curr - start), this.line, col);
    }

    private Token ScanString()
    {
        int col = this.column;
        Next();
        int start = curr;
        while (NotAtEnd() && Peek() != '\"') {
            if (Next() == '\n') throw new Exception("Trailing String Found.");
        }
        return new Token(TokenKind.StringLit, source.Substring(start, curr - start), this.line, col);
    }
    private Dictionary<string, TokenKind> reserved = new() {
        { "mov",    TokenKind.Mov        },
        { "incr",   TokenKind.PtrIncr    },
        { "decr",   TokenKind.PtrDecr    },
        { "outln",  TokenKind.Outln      },
        { "out",    TokenKind.Out        },
        { "in",     TokenKind.Input      },
        { "inln",   TokenKind.Inputln    },
        { "for",    TokenKind.For        },
        { "ret",    TokenKind.Return     },
        { "if",     TokenKind.If         },
        { "elif",   TokenKind.Elif       },
        { "else",   TokenKind.Else       },
        { "<=",     TokenKind.LessEq     },
        { ">=",     TokenKind.GreaterEq  },
        { "!=",     TokenKind.BangEq     },
        { "sphere", TokenKind.Pragma     },
        { "conf",   TokenKind.Config     },
        { "true",   TokenKind.BoolLit    },
        { "false",  TokenKind.BoolLit    },
        { "#>",     TokenKind.RMLComment },
        { "<#",     TokenKind.LMLComment },
        { "&&",     TokenKind.And        },
        { "||",     TokenKind.Or        },

        { "at",     TokenKind.At         },
    };

    private bool NotAtEnd(int ahead = 0) => curr + ahead < this.source.Length;
    private char Peek(int ahead = 0) => this.NotAtEnd(ahead) ? this.source[curr + ahead] : '\0';
    private char Next()
    {
        if (NotAtEnd())
        {
            curr++;
            column++;
        }
        return this.Peek();
    }
}
