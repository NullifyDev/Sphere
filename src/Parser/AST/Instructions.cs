using System;

namespace Sphere.Parsers.AST
{
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
        public record If(ExprNode Cond, List<ExprNode>? Block) : InstNode {
            public override string ToString() => $"if {Cond} {{\n    {string.Join("\n    ", Block ?? new())}\n}} ";
        }
        public record Elif(ExprNode Cond, List<ExprNode>? Block) : InstNode {
            public override string ToString() => $"else if {Cond} {{\n    {string.Join("\n    ", Block ?? new())}\n}} ";
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
        public record For : InstNode {
            public ExprNode Start, End;
            public ExprNode? Id;
            public List<ExprNode>? Block;
            
            public Expressions.BinOp? In;

            public For(ExprNode Start, ExprNode End, ExprNode? Id, List<ExprNode>? Block) {
                this.Start = Start;
                this.End = End;
                this.Id = Id;
                this.Block = Block;
            }
            public For(ExprNode In, List<ExprNode>? Block) {
                if(In is not Expressions.BinOp) Utils.Error($"Failed to only get a Binary Operator! Got {In} instead");
                if((In as Expressions.BinOp)!.Op.Kind != Lexer.TokenKind.Colon) Utils.Error($"Expected ':' Got {(In as Expressions.BinOp)!.Op.Kind} instead");

                this.In = (In as Expressions.BinOp)!;

                this.Block = Block;
            }
            public override string ToString() => In == null ? 
                $"for {Start} {End} {Id} {{\n    {string.Join("\n    ", Block ?? new List<object>(0).AsEnumerable())}\n}}" :
                $"for {In.Left} in {In.Right} {{\n    {string.Join("\n    ", Block ?? new List<object>(0).AsEnumerable())}\n}}";
        }
        public record While(ExprNode Condition, List<ExprNode>? Block) : InstNode {
            public override string ToString() => $"while {Condition} {{\n    {string.Join(", ", Block ?? new List<object>(0).AsEnumerable())}\n}}";
        }
    }
}