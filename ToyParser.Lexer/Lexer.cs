using System.Text.RegularExpressions;
using ToyParser.Utils;

namespace ToyParser.Lexer
{
    public class Lexer : ILexer
    {
        private readonly ReadOnlyMemory<char> _content;
        private readonly TokenType[] _ignoredTokens;
        private static readonly Dictionary<Regex, TokenType> _regexToTokenType;
        private int _position;
        private bool _isEOF => this._position >= this._content.Length;
        private ReadOnlyMemory<char> _currentSlice => this._content[this._position..];

        static Lexer()
        {
            _regexToTokenType = new()
            {
                {RegexPaterns.ReturnRegex, TokenType.RETURN },
            };
        }

        public Lexer(ReadOnlyMemory<char> content, TokenType[] ignoredTokens)
        {
            this._position = 0;
            this._content = content;
            this._ignoredTokens = ignoredTokens;
        }

        public Token GetToken()
        {
            Position position = new Position(this._position, this._content.Length);

            if (_isEOF)
                return new Token(TokenType.EOF, this._currentSlice, new());

            foreach ((Regex regex, TokenType tokenType) in _regexToTokenType)
            {
                Match match = regex.Match(this._currentSlice);
                if (match.Success)
                {
                    position = new Position(this._position, this._position + match.Length);

                    return tokenType switch
                    {
                        _ => new Token(tokenType, this._content.Slice(this._position, match.Length), position)
                    };
                }
            }
            return new Token(TokenType.ILLEGAL, this._currentSlice, position);
        }

        public Token GetNextToken()
        {
            Token token = GetToken();
            if (_ignoredTokens.Contains(token.Type))
            {
                token = GetToken();
            }
            this._position += token.Position.Length;
            return token;
        }
    }
}