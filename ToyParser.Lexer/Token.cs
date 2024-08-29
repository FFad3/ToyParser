namespace ToyParser.Lexer
{

    public readonly struct Token(TokenType type, ReadOnlyMemory<char> readOnlyMemory, Position position)
    {
        private readonly ReadOnlyMemory<char> _readOnlyMemory = readOnlyMemory;
        public TokenType Type { get; } = type;
        public Position Position { get; } = position;
        public string Value => _readOnlyMemory.ToString();
    }
}