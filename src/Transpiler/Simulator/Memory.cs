using System.IO.Compression;
using Sphere.Core;
using Sphere.Parsers.AST;

namespace Sphere.Compilation;

public partial record Transpiler
{
    public static Dictionary<string, object>  Variables = new();
    public static List<Instructions.Function> Functions = new();
    public static Dictionary<string, List<Node>> Objects = new();

    private class Memory(int Size)
    {
        private int[] Mem = new int[Size]; 
        private List<Label> Labels = new();

        public void AddLabel(string name, LabelType type, int address, int size = 1) {

            // Checks current item for cross-label overlapping.
            if (Labels.Any(x => address >= x.Address || address + size < x.Address + x.Size)) {
                // Errors.Add()
            }

                // x.Address >= address + size && x.Address + x.Size < address ))
            Labels.Add(new(name, type, address, size));
        }
    }

    private record Label
    {
        public string Name;
        public LabelType Type;
        public int Address, Size;

        public Label(string Name, LabelType Type, int Address, int Size) {
            this.Name = Name;
            this.Type = Type;
            this.Address = Address;
            this.Size = Size;
        }

        public string ToString(int[] data) => $"{this.Name}: {this.Type} @ {this.Address} - {this.Size} | Content : {string.Join(", ", data[this.Address..(this.Address + this.Size)])}";
        public override string ToString()  => $"{this.Name}: {this.Type} @ {this.Address} - {this.Size}";

        public int GetAddress() => this.Address;
        public int GetSize() => this.Size;
        public string GetName() => this.Name;

        public int SetAddress(int address) => this.Address = address;
        public int SetSize(int size) => this.Size = size;
    }
}

public enum LabelType {
    String,
    Int, 
    Bool,

    /* Prevents Cross-Partition-Labels . 
    Disallows other Partition Labels from overlapping other Parition Labels. */ 
    Parititon
}