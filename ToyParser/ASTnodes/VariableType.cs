using ToyParser.Lexer;

namespace ToyParser.Parser.ASTnodes
{
    public readonly struct VariableType
    {
        public TokenType Type { get; }
        public bool IsArray { get; }
        public string Value { get; }

        public VariableType(TokenType type, string value,bool isArray)
        {
            Type = type;
            Value = value;
            IsArray = isArray;
        }

        public override string ToString() => $"{Type.ToString()}{(IsArray ? "[]":string.Empty)}";
    }
}