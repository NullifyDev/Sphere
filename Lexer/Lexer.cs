using System.Collections.Generic;

namespace Qbe
{
    public class Lexer
    {
        public string source;
        private int curr = 0;
        private int line = 0;
        private int col = 0;

        public Lexer(string source)
        {
            this.source = source;
        }

        public IEnumerable<Token> Lex()
        {
            while (curr < source.Length)
            {
                this.col++;
                char currentChar = source[curr];

                if (char.IsWhiteSpace(currentChar))
                {
                    curr++;
                    continue;
                }
                else if (currentChar.Equals('\n'))
                {
                    this.line++;
                    this.col = 0;
                    curr++;
                    // yield return new Token(TokenType.EOL, "\n", this.line, this.col);
                }
                else if (char.IsLetter(currentChar)) yield return ScanIdentifier();
                else if (char.IsDigit(currentChar)) yield return ScanNumber();
                else if (currentChar == '"') yield return ScanString();
                else
                {
                    // Handle other special characters or tokens
                    curr++;
                }
            }
        }

        private Token ScanIdentifier()
        {
            int start = curr;
            while (curr < source.Length && char.IsLetterOrDigit(source[curr]))
            {
                curr++;
            }
            string identifier = source.Substring(start, curr - start);
            // Check for reserved words and return the appropriate token type
            if (reserved.TryGetValue(identifier, out TokenType tokenType))
            {
                return new Token(tokenType, identifier, this.line, this.col);
            }

            return new Token(TokenType.Identifier, identifier, this.line, this.col);
        }

        private Token ScanNumber()
        {
            int start = curr;
            while (curr < source.Length && char.IsDigit(source[curr]))
            {
                curr++;
            }
            string number = source.Substring(start, curr - start);
            return new Token(TokenType.NumLit, number, this.line, start);
        }

        private Token ScanString()
        {
            int start = curr + 1;
            while (curr < source.Length && source[curr] != '"')
            {
                curr++;
            }
            if (curr >= source.Length || source[curr] != '"')
            {
                throw new Exception("Unterminated string");
            }
            string str = source.Substring(start, curr - start);
            curr++; // Consume the closing double quote
            return new Token(TokenType.StringLit, str, this.line, this.col);
        }

        public bool AtEnd(int curr) => curr >= this.source.Length;
        
        private Dictionary<string, TokenType> reserved = new Dictionary<string, TokenType>
        {
            { "up", TokenType.PtrUp },
            { "down", TokenType.PtrDown },
            { "incr", TokenType.IncrAddr },
            { "decr", TokenType.DecrAddr }
        };

        public bool NotAtEnd() => curr < this.source.Length;
    }
}