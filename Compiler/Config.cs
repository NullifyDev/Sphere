using System.Runtime.InteropServices;

namespace Sphere.Compiler;

public class Config
{
    public static PlatformID? Platform = Environment.OSVersion.Platform;
    public static Architecture Archictecture = RuntimeInformation.OSArchitecture;
    public static string ProjectDir = Directory.GetCurrentDirectory();
}