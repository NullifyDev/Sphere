using System;

namespace Sphere.Parsers.AST;

using static Sphere.Parsers.AST.Expressions;

public class Instructions
{
    public record Mov : Node
    {
        public Expressions.Identifier Item;
        public long Amount;
        
        public Mov(Expressions.Identifier item, long amount, string file, int line, int col) : base(file, line, col) {
            this.Item = item;
            this.Amount = amount;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"Mov: [ {Item}, {Amount} ]";
    }
    public record Incr : Node
    {
        public Expressions.Identifier Item;
        public long Amount;

        public Incr(Expressions.Identifier item, long amount, string file, int line, int col) : base(file, line, col)
        {
            this.Item = item;
            this.Amount = amount;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"Incr: [ {Item}, {Amount} ]";
    }
    public record Decr : Node
    {
        public Expressions.Identifier Item;
        public long Amount;

        public Decr(Expressions.Identifier item, long amount, string file, int line, int col) : base(file, line, col)
        {
            this.Item = item;
            this.Amount = amount;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"Decr: [ {Item}, {Amount} ]";
    }
    public record If : Node
    {
        public Node Cond; 
        public List<Node> Body;
        public If(Node cond, List<Node> body, string file, int line, int col) : base(file, line, col)
        {
            base.Type = this.GetType().Name;

            this.Cond = cond;
            this.Body = body;
        }
        public override string ToString() => $"if {Cond} {{\n    {string.Join("\n    ", Body)}\n}} ";
    }
    public record Elif : Node
    {
        public Node Cond;
        public List<Node> Body;
        public Elif(Node cond, List<Node> body, string file, int line, int col) : base(file, line, col)
        {
            base.Type = this.GetType().Name;

            this.Cond = cond;
            this.Body = body;
        }

        public Elif(If i, string file, int line, int col) : base(file, line, col)
        {
            base.Type = this.GetType().Name;

            this.Cond = i.Cond;
            this.Body = i.Body;
        }
        public override string ToString() => $"else if {Cond} {{\n    {string.Join("\n    ", Body)}\n}} ";
    }
    public record Else : Node
    {
        public List<Node> Body;
        public Else(List<Node> body, string file, int line, int col) : base(file, line, col)
        {
            this.Body = body ?? new();

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"else {{\n    {string.Join("\n     ", Body)}\n}}";
    }
    public record Out : Node
    {
        public IEnumerable<Node> Args;
        public Out(IEnumerable<Node> args, string file, int line, int col) : base(file, line, col)
        {
            this.Args = args;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"out({string.Join(", ", Args ?? new List<object>(0).AsEnumerable())}) ];";
    }
    public record Outln : Node
    {
        public IEnumerable<Node> Args;
        public Outln(IEnumerable<Node> args, string file, int line, int col) : base(file, line, col)
        {
            this.Args = args;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"outln({string.Join(", ", Args ?? new List<object>(0).AsEnumerable())});";
    }
    public record In : Node
    {
        public Expressions.Literal Prompt;
        public In(Expressions.Literal prompt, string file, int line, int col) : base(file, line, col)
        {
            this.Prompt = prompt;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"in({Prompt})";
    }
    public record Inln : Node
    {
        public Expressions.Literal Prompt;
        public Inln(Expressions.Literal prompt, string file, int line, int col) : base(file, line, col)
        {
            this.Prompt = prompt;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"inln({Prompt})";
    }
    public record Continue : Node
    {
        public Continue(string file, int line, int col) : base(file, line, col) {
            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"continue";
    }
    public record For : Node
    {
        public Node Start, End;
        public Node? Id;
        public List<Node> Body;

        public For(Node Start, Node End, Node? Id, List<Node> Body, string file, int line, int col) : base(file, line, col)
        {
            this.Start = Start;
            this.End = End;
            this.Id = Id;
            this.Body = Body;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"for {Start} {End} {Id} {{\n    {string.Join("\n    ", Body)}\n}}";
    }
    public record Function : Node
    {
        public Expressions.Identifier Name;
        public Node ReturnType;
        public List<Node> Params, Body;

        public Function(Expressions.Identifier name, Node ReturnType, List<Node> Params, List<Node> Body, string file, int line, int col)
            : base(file, line, col)
        {
            this.Name = name;
            this.ReturnType = ReturnType;
            this.Params = Params;
            this.Body = Body;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"{this.Name}({string.Join(", ", this.Params)}): {this.ReturnType} {{\n    {string.Join("\n    ", this.Body ?? new List<object>(0).AsEnumerable())}\n}}";
    }
    public record Foreach : Node
    {

        public Expressions.Operator? In;
        public List<Node> Body;

        public Foreach(Node In, List<Node> Body, string file, int line, int col) : base(file, line, col)
        {
            if (In is not Expressions.Operator) Utils.InternalError($"Failed to only get a Binary Operator! Got {In} instead", In.File, In.Line, In.Column);
            if ((In as Expressions.Operator)!.OpType != Lexer.TokenKind.Colon) Utils.InternalError($"Expected ':' Got {(In as Expressions.Operator)!.Type} instead", In.File, In.Line, In.Column);

            this.In = (In as Expressions.Operator)!;
            this.Body = Body;
            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"for {In!.Left} in {In!.Right} {{\n    {string.Join("\n    ", Body)}\n}}";
    }
    public record While : Node
    {
        public Node Condition;
        public List<Node> Body;

        public While(Node condition, List<Node> body, string file, int line, int col) : base(file, line, col)
        {
            this.Condition = condition;
            this.Body = body;

            base.Type = this.GetType().Name;
        }

        public override string ToString() => $"while {Condition} {{\n    {string.Join(", ", Body)}\n}}";
    }
    public record Sphere : Node
    {
        public List<Node> Body = new();
        public Sphere(List<Node> Body, string file, int line, int col) : base(file, line, col)
        {
            this.Body = Body;

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"SPHERE: {{ \n    {string.Join("\n    ", Body ?? new List<Node>(0).AsEnumerable())} \n}}\n";
    }
    public record Sphereln : Node
    {
        public Node Body;
        public Sphereln(Node Body, string file, int line, int col) : base(file, line, col)
        {
            this.Body = Body;
        }
        public override string ToString() => $"SPHERE: {string.Join("\n    ", Body)}";
    }
    public record Return : Node
    {
        public List<Node> Items;
        public Return(string file, int line, int col, List<Node> items) : base(file, line, col)
        {
            base.Type = this.GetType().Name;
            this.Items = items ?? new();

            base.Type = this.GetType().Name;
        }
        public override string ToString() => $"return {string.Join("\n    ", Items)}";
    }
}