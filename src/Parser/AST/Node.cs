namespace Sphere.Parsers.AST;

public record ExprNode {
    public string File;
    public int Line;
    public int Column;
};
public record InstNode {
    public string File;
    public int Line;
    public int Column;
};