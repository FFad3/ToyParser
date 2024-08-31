namespace ToyParser.Lexer
{
    public interface ILexer
    {
        Token GetNextToken();

        void SetContent(ReadOnlyMemory<char> content);
        void SetIgnoredTokens(params TokenType[] tokenTypes);
    }
}
