namespace Sphere.Parsers.AST;

public record Node {
    public string Type;
    public string File;
    public int Line, Column;
    public Node(string File, int Line, int Column) {
        this.File = File;
        this.Line = Line;
        this.Column = Column;
    }
}