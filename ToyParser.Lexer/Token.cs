namespace ToyParser.Lexer
{

    public readonly struct Token(TokenType type, ReadOnlyMemory<char> value, Position position)
    {
        public TokenType Type { get; } = type;
        public ReadOnlyMemory<char> Value { get; } = value;
        public Position Position { get; } = position;
    }
}