using ToyParser.Lexer;

namespace ToyParser.Parser.ASTnodes
{
    public readonly struct Operator
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Operator(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString() => Type.ToString();
    }
}