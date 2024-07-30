using System.Diagnostics.Tracing;
using System.Security;

namespace Sphere.Compilation;

// Compiler variables
public static class Variables
{

    public static void SetValue(string variable, object value) => Props[variable] = value;
    public static object GetValue(string variable) => Props[variable];

    public static Dictionary<string, object> Props = new() {
        { "OperatingSystem", OSType.None },
        { "Architecture", Architecture.x86 },
        { "Bit", Bit._64 }
    };

    public enum OSType {
        None,
        Linux,
        MacOS,
        Windows
    }

    public enum Architecture {
        x86,
        Arm,
        RISCV
    }

    public enum Bit {
        _4 = 4,
        _8 = 8,
        _16 = 16,
        _32 = 32,
        _64 = 64
    }

}