using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sphere
{
    public static class Utils
    {
        public static void Out(string msg = "") => Console.Write(msg);
        public static void Outln(string msg = "") => Console.WriteLine(msg);
        // public static void Error(string msg = "") => Outln($"[INTERNAL ERROR]: {msg}");
        public static object         Error(string file, string? msg, int line, int column) => throw new Exception($"{file}({line}:{column}): {msg} ");
        public static void           Error(string? msg) => throw new Exception($"{msg}");
        public static object InternalError(string file, string? msg, int line, int column) => throw new Exception($"[INTERNAL ERROR]: {msg} | Errored At: {file}: {line}:{column}");
    }
}