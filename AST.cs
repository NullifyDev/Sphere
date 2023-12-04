using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Serialization;
using OneOf;

namespace Sphere;

public class AST
{

    public static Dictionary<string, Function> Functions = new();
    public static Dictionary<string, Variable> Variables = new();
    public static Dictionary<string, Vector>   Vectors = new();
    public static List<Instruction> Instructions = new();
    
    public class Node {
        public object Value;
        public int? Line;
        public int? Column;
        public Node(object node, int? line, int? column) {
            this.Value = node;
            this.Line = line;
            this.Column = column;
        }
    }


    public enum Type 
    {
        Int,
        String,
        Float,
        Bit,
        Any,
        Void,

        Up,
        Down,
        Incr,
        Decr,
        Out,
        Outln,

        For,
        Class,
        Struct,
        Call,
        
        Func,
            
        Use,
        SphereConf,
        
        EOF
    }

    public class Pragma {
        TokenType Type;
        string Property;
        object Value;

        public Pragma(TokenType type, string property, object? value) {
            this.Type = type;
            this.Property = property;
            this.Value = value == null ? new object() : value;
        }
    }

    public class TypeCastException : Exception
    {
        public TypeCastException() {}
        public TypeCastException(string msg) : base(msg) {}
    }

    public class Function { 
        public string Name;
        public Type ReturnType;
        public Dictionary<string, Variable> Parameters;
        public IEnumerable<AST.Node> Body;

        public Function()
        {
        }

        public Function(string name, AST.Type returnType, Dictionary<string, AST.Variable>? parameters, IEnumerable<Node> body = null) {
            this.Name = name;
            this.ReturnType = returnType;
            this.Parameters = parameters == null ? new() : parameters;
            this.Body = body;
        }

        public void SetReturnType(Type type) => this.ReturnType = type;
        public Type GetReturnType() => this.ReturnType;
    }

    public class Instruction {
        public Type Type;
        public object Args;
        public Instruction(Type type, object args) {
            this.Type = type;
            this.Args = args;
        }
        
        public Instruction(Type type, Token args) {
            this.Type = type;
            this.Args = args;
        }
    }

    public class Vector
    {
        Type Type; 
        List<OneOf<Variable, int, Vector>> Value = new();
        public Vector(Type type, List<OneOf<Variable, int, Vector>> value) {
            this.Type = type;
            this.Value = value;
        }
    }

    public class Variable
    {
        public Type Type;
        public object? Value;

        public Variable()
        {
            this.Type = Type.Any;
            this.Value = null;
        }

        public Variable(Type type, object value)
        {
            this.Type = type;
            this.Value = value;
        }
        
        public Variable(Type type)
        {
            this.Type = type;
            this.Value = null;
        }

        public Variable(Token token, object? value = null)
        {
            this.Type = token.Type switch
            {
                TokenType.StringLit  => Type.String,
                TokenType.NumLit     => Type.Int,
                TokenType.BoolLit    => Type.Bit,
                TokenType.Star       => Type.Int,
                _                    => throw new Sphere.Exceptions.UnknownDataType(token.Type) // Type.Any will be implemented later
            };
            this.Value = value;
        }
        
        public Variable(Token token, bool isParam)
        {
            this.Type = token.Type switch
            {
                TokenType.StringLit  => Type.String,
                TokenType.NumLit     => Type.Int,
                TokenType.BoolLit    => Type.Bit,
                TokenType.Star       => Type.Any,
                _                    => Type.Any
            };
            this.Value = isParam ? null : token.value;
        }
        
        public Variable(string type)
        {
            this.Type = type switch
            {
                "str" => Type.String,
                "num" => Type.Int,
                "bit" => Type.Bit,
                "*" => Type.Any,
                _ => throw new Exception("[] Unknown type")
            };
            this.Value = null;
        }

        public Variable(TokenType type)
        {
            this.Type = type switch
            {
                TokenType.StringLit  => Type.String,
                TokenType.NumLit     => Type.Int,
                TokenType.BoolLit    => Type.Bit,
                TokenType.Star       => Type.Any
            };
            this.Value = null;
        }

        bool canConvert(Type t1, Type t2) => (t1, t2) switch
        {
            (Type.Int,   Type.String) => true,
            (Type.Float, Type.String) => true,
            _                         => t1 == t2
        };

        T tryCastTo<T>(Type t1, Type tOther)
        {
            if (!canConvert(t1, tOther))
                throw new TypeCastException("Cannot interpret t1 as t2");

            return (T)Value;
        }
        
        int asInt() => tryCastTo<int>(Type, Type.Int);
        string asString() => tryCastTo<string>(Type, Type.String);
    }

    public class Identifier
    {
        public Type Type ;
        public Identifier(Type type) {
            this.Type = type;
        }
    }
}