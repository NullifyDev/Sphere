using System;

namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public class Instructions
{
    public record Mov(Expressions.Identifier Item, long Amount = 1) : InstNode {
        public override string ToString() => $"Mov: [ {Item}, {Amount} ]";
    }
    public record Incr(Expressions.Identifier Item, long Amount = 1) : InstNode {
        public override string ToString() => $"Incr: [ {Item}, {Amount} ]";
    }
    public record Decr(Expressions.Identifier Item, long Amount = 1) : InstNode {
        public override string ToString() => $"Decr: [ {Item}, {Amount} ]";
    }
    public record If(ExprNode Cond, CodeBlock Block) : InstNode {
        public override string ToString() => $"if {Cond} {{\n    {string.Join("\n    ", Block)}\n}} ";
    }
    public record Elif : InstNode {
        public ExprNode Cond;
        public CodeBlock Block;
        public Elif(ExprNode cond, CodeBlock block, string file, int line, int col) {
            this.File = file;
            this.Line = line;
            this.Column = col;

            this.Cond = cond;
            this.Block = block;
        }

        public Elif(If i)
        {
            this.File = i.File;
            this.Line = i.Line;
            this.Column = i.Column;

            this.Cond = i.Cond;
            this.Block = i.Block;
        }
        public override string ToString() => $"else if {Cond} {{\n    {string.Join("\n    ", Block)}\n}} ";
    }
    public record Else(List<ExprNode> Block) : InstNode {
        public override string ToString() => $"else {{\n    {string.Join("\n     ", Block)}\n}}";
    }
    public record Out(IEnumerable<object> Args) : InstNode
    {
        public override string ToString() => $"out({string.Join(", ", Args ?? new List<object>(0).AsEnumerable())}) ];";
    }
    public record Outln(IEnumerable<object> Args) : InstNode {
        public override string ToString() => $"outln({string.Join(", ", Args ?? new List<object>(0).AsEnumerable())});";
    }
    public record In(Expressions.Literal? Prompt) : InstNode
    {
        public override string ToString() => $"in({Prompt})";
    }
    public record Inln(Expressions.Literal? Prompt) : InstNode {
        public override string ToString() => $"inln({Prompt})";
    }
    public record Continue : InstNode
    {
        public override string ToString() => $"continue";
    }
    public record For : InstNode {
        public ExprNode Start, End;
        public ExprNode? Id;
        public CodeBlock Block;

        public For(ExprNode Start, ExprNode End, ExprNode? Id, CodeBlock Block) {
            this.Start = Start;
            this.End = End;
            this.Id = Id;
            this.Block = Block;
        }
        public override string ToString() => $"for {Start} {End} {Id} {{\n    {string.Join("\n    ", Block)}\n}}";
    }

    public record Function(Expressions.Identifier name, ExprNode ReturnType, List<ExprNode> Params, List<ExprNode> Body) : InstNode {
        public override string ToString() => $"{name}({string.Join(", ", Params)}): {ReturnType} {{\n    {string.Join("\n    ", Body ?? new List<object>(0).AsEnumerable())}\n}}";
    }
    public record Foreach : InstNode {
        
        public Expressions.BinOp? In;
        public CodeBlock Block;

        public Foreach(ExprNode In, CodeBlock Block) {
            if(In is not Expressions.BinOp) Utils.InternalError($"Failed to only get a Binary Operator! Got {In} instead", In.File, In.Line, In.Column);
            if((In as Expressions.BinOp)!.Op.Kind != Lexer.TokenKind.Colon) Utils.InternalError($"Expected ':' Got {(In as Expressions.BinOp)!.Op.Kind} instead", In.File, In.Line, In.Column);

            this.In = (In as Expressions.BinOp)!;
            this.Block = Block;
        }
        public override string ToString() => $"for {In!.Left} in {In!.Right} {{\n    {string.Join("\n    ", Block)}\n}}";
    }
    public record While(ExprNode Condition, CodeBlock Block) : InstNode {
        public override string ToString() => $"while {Condition} {{\n    {string.Join(", ", Block)}\n}}";
    }
    public record Sphere : InstNode {
        public List<ExprNode> Block = new();
        public Sphere(List<ExprNode> Block) {
            this.Block = Block;
        }
        public override string ToString() => $"SPHERE: {{ \n    {string.Join("\n    ", Block ?? new List<ExprNode>(0).AsEnumerable())} \n}}\n";
    }
    public record Sphereln : InstNode
    {
        public ExprNode Block;
        public Sphereln(ExprNode? Block = null)
        {
            this.Block = Block ?? new();
        }
        public override string ToString() => $"SPHERE: {string.Join("\n    ", Block ?? new ExprNode())}";
    }
}