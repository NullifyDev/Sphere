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
                    yield return new Token(TokenKind.EOL, file, this.Peek().ToString(), this.line, this.column);
                    if (Peek(1) == '\n') Next();
                    line++;
                    this.column = 0;
                    break;
                case '.': yield return new Token(TokenKind.Dot, file, this.Peek().ToString(), this.line, this.column); break;
                case '(': yield return new Token(TokenKind.LParen, file, this.Peek().ToString(), this.line, this.column); break;
                case ')': yield return new Token(TokenKind.RParen, file, this.Peek().ToString(), this.line, this.column); break;
                case '[': yield return new Token(TokenKind.LBracket, file, this.Peek().ToString(), this.line, this.column); break;
                case ']': yield return new Token(TokenKind.RBracket, file, this.Peek().ToString(), this.line, this.column); break;
                case '}': yield return new Token(TokenKind.RBrace, file, this.Peek().ToString(), this.line, this.column); break;
                case '{': yield return new Token(TokenKind.LBrace, file, this.Peek().ToString(), this.line, this.column); break;
                case '$': yield return new Token(TokenKind.Dollar, file, this.Peek().ToString(), this.line, this.column); break;
                case '@': yield return new Token(TokenKind.AtPrefix, file, this.Peek().ToString(), this.line, this.column); break;
                case '*': yield return new Token(TokenKind.Star, file, this.Peek().ToString(), this.line, this.column); break;
                case '!': yield return new Token(TokenKind.Bang, file, this.Peek().ToString(), this.line, this.column); break;
                case '%': yield return new Token(TokenKind.Modulo, file, this.Peek().ToString(), this.line, this.column); break;
                case '=':
                    if (Next() == '=')
                        yield return new Token(TokenKind.DoubleEq, file, "==", this.line, this.column);
                    else
                        yield return new Token(TokenKind.Equal, file, "=", this.line, this.column);
                    break;

                case '<': yield return new Token(TokenKind.Less, file, this.Peek().ToString(), this.line, this.column); break;
                case '>': yield return new Token(TokenKind.Greater, file, this.Peek().ToString(), this.line, this.column); break;
                case '|': yield return new Token(TokenKind.Pipe, file, this.Peek().ToString(), this.line, this.column); break;
                case '+': yield return new Token(TokenKind.Plus, file, this.Peek().ToString(), this.line, this.column); break;
                case ':': yield return new Token(TokenKind.Colon, file, this.Peek().ToString(), this.line, this.column); break;
                case '#':
                    if (Next() == '>')
                    {
                        while (Next() != '<')
                            if (Next() != '#') continue;
                    }
                    while (Next() != '\n') { }
                    break;
                default:
                    if (Peek() == '#')
                    {
                        if (Next() == '>')
                        {
                            while (Next() != '<')
                                if (Next() != '#')
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
        yield return new Token(TokenKind.EOF, file, "EOF", line, column);
    }

    private Token ScanIdentifier()
    {
        int col = this.column;
        int start = curr;
        while (NotAtEnd() && char.IsLetterOrDigit(Peek()) || NotAtEnd() && Peek() == '_')
            Next();

        string identifier = source.Substring(start, curr - start);

        if (reserved.TryGetValue(identifier, out TokenKind TokenKind))
            return new Token(TokenKind, file, identifier, this.line, col);

        if (identifier == "true" || identifier == "false")
            return new Token(TokenKind.BoolLit, file, identifier, line, column);

        return new Token(TokenKind.Identifier, file, identifier, this.line, col);
    }

    private Token ScanNumber()
    {
        int col = this.column;
        int start = curr;
        while (NotAtEnd() && char.IsLetterOrDigit(Peek()))
            Next();

        // return new Token(TokenKind.NumLit, source.Substring(start, curr - start), this.line, start+1);
        string number = source.Substring(start, curr - start);
        if (number.StartsWith("0x"))
        {
            for (int i = 2; i < number.Length; i++)
            {
                int ascii = (int)number[i];
                if (ascii >= 97 && ascii <= 102) continue;
                else if (ascii >= 65 && ascii <= 75) continue;
                else if (ascii >= 48 && ascii <= 57) continue;
                else Utils.Error($"Invalid Hexadecimal format", this.file, this.line, col);
            }
            return new Token(TokenKind.HexLit, file, source.Substring(start, curr - start), this.line, col);
        }
        return new Token(TokenKind.IntLit, file, source.Substring(start, curr - start), this.line, col);
    }

    private Token ScanString()
    {
        int col = this.column;
        Next();
        int start = curr;
        while (NotAtEnd() && Peek() != '\"')
        {
            if (Next() == '\n') Utils.Error("Trailing String Found.");
        }
        return new Token(TokenKind.StringLit, file, source.Substring(start, curr - start), this.line, col);
    }
    private Dictionary<string, TokenKind> reserved = new() {
        { "mov",      TokenKind.Mov        },
        { "incr",     TokenKind.PtrIncr    },
        { "decr",     TokenKind.PtrDecr    },
        { "outln",    TokenKind.Outln      },
        { "out",      TokenKind.Out        },
        { "in",       TokenKind.Input      },
        { "inln",     TokenKind.Inputln    },
        { "for",      TokenKind.For        },
        { "ret",      TokenKind.Return     },
        { "if",       TokenKind.If         },
        { "elif",     TokenKind.Elif       },
        { "else",     TokenKind.Else       },
        { "while",    TokenKind.While      },
        { "<=",       TokenKind.LessEq     },
        { ">=",       TokenKind.GreaterEq  },
        { "!=",       TokenKind.BangEq     },
        { "SPHERE",   TokenKind.Sphere     },
        { "conf",     TokenKind.Config     },
        { "#>",       TokenKind.RMLComment },
        { "<#",       TokenKind.LMLComment },
        { "&&",       TokenKind.And        },
        { "||",       TokenKind.Or         },
        { "at",       TokenKind.At         },
        { "continue", TokenKind.Continue   },
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
