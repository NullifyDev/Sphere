using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace Qbe;

public class Lexer {
    private int start = 0, current = 0, line = 1, col = 0, currLineCol = 0;
    private readonly string source;
    private List<Token> tokens = new();
    private bool EOF => current >= source.Length;
    private Dictionary<string, TokenType> reserved = new();
    public Lexer(string source)
    {
        this.source = source;
        this.reserved = new Dictionary<string, TokenType>()
        {
            // Instructions
            ["up"] = TokenType.PtrUp,
            ["down"] = TokenType.PtrDown,
            ["incr"] = TokenType.Incr,
            ["decr"] = TokenType.Decr,
            ["ret"] = TokenType.Ret,
            ["jmp"] = TokenType.Jmp,
            ["jmpTo"] = TokenType.JmpTo,
            ["comp"] = TokenType.Comp,
            ["func"] = TokenType.Func,
            ["call"] = TokenType.Call,
            ["out"] = TokenType.PrintOut,
            ["outln"] = TokenType.PrintOutln,
            ["in"] = TokenType.Input,
            ["inln"] = TokenType.InputLine,
            [">>"] = TokenType.BindToR,
            ["end"] = TokenType.End,
            ["<<"] = TokenType.BindToL,
            ["num"] = TokenType.Num,
            ["str"] = TokenType.Str,
            ["bool"] = TokenType.Bool,
            ["char"] = TokenType.Char,
            ["void"] = TokenType.Void
        };
    }
     #region Helper functions
    private char advance() => !EOF ? source[current++] : '\0';
    private char peek(int ahead = 0) => (current + ahead >= source.Length) ? '\0' : source[current + ahead];
    public string ScanString()
    {
        string str = "";
        this.advance();
        while (peek() != '\"' && !EOF)
        {
            str += this.source[this.current];
            advance();
        }
        this.col += str.Length;
        return str;
    }
    #endregion
    public List<Token> Lex() {
        string lexeme = "";
        while(!EOF) {
            var c = this.source[this.current];
            switch (c) {
                case ']':  
                    tokens.Add(new Token(TokenType.RBracket, c.ToString(), this.line, this.currLineCol));
                    this.currLineCol = this.col; 
                    break;
                case '[':  
                    tokens.Add(new Token(TokenType.LBracket, c.ToString(), this.line, this.currLineCol));
                    this.currLineCol = this.col; 
                    break;
                case '+':
                    tokens.Add(new Token(TokenType.Plus, c.ToString(), this.line, this.currLineCol));
                    this.currLineCol = this.col; 
                    break;
                case '*':  
                    tokens.Add(new Token(TokenType.Star,     c.ToString(), this.line, this.currLineCol));
                    this.currLineCol = this.col; 
                    break;
                case '\"': 
                    tokens.Add(new Token(TokenType.Str,this.ScanString(), this.line, this.currLineCol));
                    this.currLineCol = this.col; 
                    break;
                case ':':  
                    tokens.Add(new Token(TokenType.Colon,    c.ToString(), this.line, this.currLineCol));
                    this.currLineCol = this.col; 
                    break;
				case ',':
					tokens.Add(new Token(TokenType.Comma, c.ToString(), this.line, this.currLineCol));
                    this.currLineCol = this.col; 
                    break;
				case '=':
                    tokens.Add(new Token(TokenType.Equals, c.ToString(), this.line, this.currLineCol));
                    this.currLineCol = this.col;
                    break;
                case ' ':
                    if (lexeme.Length <= 0)
                    {
                        this.currLineCol = this.col;
                        break;
                    }
                    else if (this.reserved.Keys.Contains(lexeme))
                    {
                        tokens.Add(new Token(this.reserved[lexeme], lexeme, this.line, this.currLineCol));
                        this.currLineCol = this.col;
						lexeme = "";
                    } else if (int.TryParse(lexeme, out int x))
                    {
                        tokens.Add(new Token(TokenType.Num, lexeme, this.line, this.currLineCol));
                        this.currLineCol = this.col;
                        lexeme = "";
                    }
                    checkLexeme(lexeme);
                    lexeme = "";
                    break;
                case '#':
                    if (this.source[this.current + 1] == '#') {
                        tokens.Add(new Token(TokenType.slComment, "##", this.line, this.currLineCol));
                        this.currLineCol = this.col;
                    } else if (this.source[this.current + 1] == '>') {
                        tokens.Add(new Token(TokenType.mlCommentR, "#>", this.line, this.currLineCol));
                        this.currLineCol = this.col;
                    }

                    break;
                case '<':
                    if (this.source[this.current + 1] == '#') {
                        tokens.Add(new Token(TokenType.mlCommentL, "<#", this.line, this.currLineCol));
                        this.currLineCol = this.col;
                    }
                    break;
                case '\n':
                    if (lexeme.Length > 0)
                    {
                        if (this.reserved.Keys.Contains(lexeme)) {
                            this.currLineCol = this.col - lexeme.Length;
                            tokens.Add(new Token(this.reserved[lexeme], lexeme, this.line, this.currLineCol));
                        }
                        checkLexeme(lexeme);
                        if (lexeme == "\n") {
                            lexeme = "";
                            this.line++;
                            this.col = 0;
                            this.currLineCol = this.col;
                        }
                    }

                    lexeme = "";
                    this.col = 0;
                    this.line++;
                    this.currLineCol = this.col;
                    tokens.Add(new Token(TokenType.EOL, c.ToString(), this.line, this.currLineCol));
                    break;
                default:
                    lexeme += checkLexeme(lexeme);
                    break;
            }
            this.col++;
            this.current++;
        }
        this.tokens.Add(new Token(TokenType.EOF, "", this.line+1, this.col));
        return tokens;
    }

    public char checkLexeme(string lexeme)
    {
        if(lexeme.Length <= 0) return this.source[this.current];
        if (char.IsDigit(peek()) && char.IsWhiteSpace(peek()))
        {
            while (char.IsDigit(advance())) lexeme += peek();
            tokens.Add(new Token(TokenType.Num, lexeme, this.line, this.currLineCol));
            this.col += lexeme.Length;
            this.currLineCol = this.col;
            lexeme = "";
        }
        else if (lexeme == "true" || lexeme == "false")
        {
            tokens.Add(new Token(TokenType.Bool, lexeme, this.line, this.currLineCol));
            this.currLineCol = this.col;
            lexeme = "";
        }
        else {
            if (peek() == ' ' || peek() == '\n')
            {
                tokens.Add(new Token(TokenType.Identifier, lexeme, this.line, this.currLineCol));
                lexeme = "";
            }
        }
        lexeme = "";
        return this.source[this.current];
    }
}