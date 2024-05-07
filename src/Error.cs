using static System.Console;

using Sphere.Lexer;

namespace Sphere
{
    class ParseException(string? message = "") : Exception;
    // class ParseException : Exception
    // {
    //     public ParseException() { }
    //     public ParseException(string message) : base(message) { }
    // }

    class ErrorReporting
    {
        /// Have we gotten an error while scanning?
        public static List<ErrorReporting> Errors = new();

        /// <summary>
        /// Display a friendly message saying where we encountered an error
        /// </summary>
        /// <param name="line"></param>
        /// <param name="message"></param>
        public static void Error(int line, string message) => report(line, "", message);

        public static void Error(Token token, string message)
            => report(token.Line, token.Kind == TokenKind.EOF ? " at end" : $"at \"{token.Value}\"", message);

        // public static void Error(Token token, string message)
        // {
        //     report(token.Line, token.Kind == TokenKind.EOF ? " at end" : $"at '{token.Value}'", message);
        //     if (token.Kind == TokenKind.EOF)
        //     {
        //         report(token.Line, " at end", message);
        //     }
        //     else
        //     {
        //         report(token.Line, $"at '{token.Value}'", message);
        //     }
        // }

        private static void report(int line, string where, string message) => WriteLine($"error at L:{line} {(where != "" ? where : "")}: {message}");
    }
}
