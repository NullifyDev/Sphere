using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Sphere;
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
                    yield return new Token(TokenType.EOL, this.Peek(), this.line, this.column);
                    if (Peek(1) == '\n') Next();
                    line++;
                    this.column = 0;
                    break;
                case '(': yield return new Token(TokenType.LParen, this.Peek(), this.line, this.column); break;
                case ')': yield return new Token(TokenType.RParen, this.Peek(), this.line, this.column); break;
                case '[': yield return new Token(TokenType.LBracket, this.Peek(), this.line, this.column); break;
                case ']': yield return new Token(TokenType.RBracket, this.Peek(), this.line, this.column); break;
                case '}': yield return new Token(TokenType.RBrace, this.Peek(), this.line, this.column); break;
                case '{': yield return new Token(TokenType.LBrace, this.Peek(), this.line, this.column); break;
                case '$': yield return ScanObject(); continue;
                case '@': yield return ScanObject(); continue;
                case '*': yield return new Token(TokenType.Star, this.Peek(), this.line, this.column); break;
                case '=': yield return new Token(TokenType.Equals, this.Peek(), this.line, this.column); break;
                case '<': yield return new Token(TokenType.LThan, this.Peek(), this.line, this.column); break;
                case '>': yield return new Token(TokenType.MThan, this.Peek(), this.line, this.column); break;
                case '|': yield return new Token(TokenType.Pipe, this.Peek(), this.line, this.column); break;
                case '+': yield return new Token(TokenType.Plus, this.Peek(), this.line, this.column); break;
                case ':': yield return new Token(TokenType.Colon, this.Peek(), this.line, this.column); break;
                default:
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
    }

    private Token ScanIdentifier()
    {
        int col = this.column;
        int start = curr;
        while (NotAtEnd() && char.IsLetterOrDigit(Peek()))
            Next();
        
        string identifier = source.Substring(start, curr - start);
        
        if (reserved.TryGetValue(identifier, out TokenType tokenType))
            return new Token(tokenType, identifier, this.line, col);

        return new Token(TokenType.Identifier, identifier, this.line, col);
    }
    
    private Token ScanObject()
    {
        int col = this.column;
        int start = curr;
        while (NotAtEnd() && (char.IsLetterOrDigit(Peek()) || Peek() == '$' || Peek() == '@'))
            Next();
        
        return new Token(TokenType.Object, source.Substring(start, curr - start), this.line, col);
    }
    
    private Token ScanNumber()
    {
        int col = this.column;
        int start = curr;
        while (NotAtEnd() && char.IsDigit(Peek()))
            Next();
        
        // return new Token(TokenType.NumLit, source.Substring(start, curr - start), this.line, start+1);
        return new Token(TokenType.NumLit, source.Substring(start, curr - start), this.line, col);
    }

    private Token ScanBool()
    {
        int col = this.column;
        Next();
        int start = curr;
        while (!char.IsWhiteSpace(this.Peek(1)))
            Next();
        
        string lit = source.Substring(start, curr - start);
        if (lit == "true") new Token(TokenType.BoolLit, lit, this.line, col);
        else if (lit == "false") new Token(TokenType.BoolLit, lit, this.line, col);
        else Error.Add(new(ErrorType.Syntax, this.file, $"Unknown literal '{lit}' ", this.line, col));
        return null;
    }

    private Token ScanString()
    {
        int col = this.column;
        Next();
        int start = curr;
        while (NotAtEnd() && Peek() != '\"')
            Next();

        return new Token(TokenType.StringLit, source.Substring(start, curr - start), this.line, col);
    }
    private Dictionary<string, TokenType> reserved = new() {
        { "up",     TokenType.PtrUp      },
        { "down",   TokenType.PtrDown    },
        { "incr",   TokenType.IncrAddr   },
        { "decr",   TokenType.DecrAddr   },
        { "outln",  TokenType.PrintOutln },
        { "out",    TokenType.PrintOut   },
        { "fn",     TokenType.Func       },
        { "in",     TokenType.Input      },
        { "inln",   TokenType.InputLine  },
        { "ret",    TokenType.Ret        },
        { "if",     TokenType.If         },
        { "jmp",    TokenType.Jmp        },
        { "exit",   TokenType.Exit       },
        { "<=",     TokenType.LTEqu      },
        { ">=",     TokenType.MTEqu      },
        { "!=",     TokenType.NEqu       },
        { "sphere", TokenType.Pragma     },
        { "conf",   TokenType.Config     }
    };
    
    private bool NotAtEnd(int ahead = 0) => curr + ahead < this.source.Length;
    private char Peek    (int ahead = 0) => this.NotAtEnd(ahead) ? this.source[curr + ahead] : '\0';
    private char Next    () {
        if (NotAtEnd())
        {
            curr++;
            column++;
        }
        return this.Peek();
    }
}   
