using System.Text.RegularExpressions;
using ToyParser.Utils;

namespace ToyParser.Lexer
{
    public sealed class CustomLexer : ILexer
    {
        private static readonly ILexer _instance = new CustomLexer();

        private ReadOnlyMemory<char> _content = ReadOnlyMemory<char>.Empty;
        private TokenType[] _ignoredTokens = [];
        private int _position;

        private bool _isEOF => this._position >= this._content.Length;
        private ReadOnlyMemory<char> _currentSlice => this._content[this._position..];

        private static readonly Dictionary<Regex, TokenType> _tokenTypeToRegex;

        static CustomLexer()
        {
            _tokenTypeToRegex = new()
            {
                {RegexPaterns.WhiteSpaceRegex, TokenType.WHITE_SPACE },
                {RegexPaterns.SingleLineCommentRegex, TokenType.COMMENT_SINGLE_LINE },
                {RegexPaterns.MultiLineCommentRegex, TokenType.COMMENT_MULTI_LINE },

                {RegexPaterns.BooleanTrueRegex, TokenType.TRUE },
                {RegexPaterns.BooleanFalseRegex, TokenType.FALSE },
                {RegexPaterns.IfKeywordRegex, TokenType.IF },
                {RegexPaterns.ElseKeywordRegex, TokenType.ELSE },
                {RegexPaterns.ForKeywordRegex, TokenType.FOR },
                {RegexPaterns.ReturnRegex, TokenType.RETURN },

                {RegexPaterns.IdentifierRegex, TokenType.IDNETIFIER },
                {RegexPaterns.DigitRegex, TokenType.DIGIT },
                {RegexPaterns.StringLiteralRegex, TokenType.STRING_LITERAL },

                {RegexPaterns.EqualsRegex, TokenType.EQUALS },
                {RegexPaterns.NotEqualsRegex, TokenType.NOT_EQUALS },
                {RegexPaterns.LessThenOrEqualsRegex, TokenType.LESS_THAN_OR_EQUALS },
                {RegexPaterns.GreaterThenOrEqualsRegex, TokenType.GREATER_THAN_OR_EQUALS },
                {RegexPaterns.AssignRegex, TokenType.ASSIGN },

                {RegexPaterns.PlusRegex, TokenType.PLUS },
                {RegexPaterns.MinusRegex, TokenType.MINUS },
                {RegexPaterns.MultiplyRegex, TokenType.MULTIPLY },
                {RegexPaterns.DivideRegex, TokenType.DIVIDE },

                {RegexPaterns.LessThenRegex, TokenType.LESS_THAN },
                {RegexPaterns.GreaterThenRegex, TokenType.GREATER_THAN },

                {RegexPaterns.ExclamationRegex, TokenType.EXCLAMATION },
                {RegexPaterns.CommaRegex, TokenType.COMMA },
                {RegexPaterns.DotRegex, TokenType.DOT },
                {RegexPaterns.SemicolonRegex, TokenType.SEMICOLON },

                {RegexPaterns.L_ParenRegex, TokenType.L_PAREN },
                {RegexPaterns.R_ParenRegex, TokenType.R_PAREN },
                {RegexPaterns.L_BraceRegex, TokenType.L_BRACE },
                {RegexPaterns.R_BraceRegex, TokenType.R_BRACE },
            };
        }

        private CustomLexer()
        { }

        public Token GetToken()
        {
            Position position = new Position(this._position, this._content.Length);

            if (_isEOF)
                return new Token(TokenType.EOF, this._currentSlice, new());

            foreach ((Regex regex, TokenType tokenType) in _tokenTypeToRegex)
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
            this._position += token.Position.Length;

            if (_ignoredTokens.Contains(token.Type))
            {
                token = GetNextToken();
            }
            return token;
        }

        public void SetContent(ReadOnlyMemory<char> content)
        {
            this._position = 0;
            this._content = content;
        }

        public void SetIgnoredTokens(params TokenType[] tokenTypes)
        {
            this._ignoredTokens = tokenTypes;
        }

        public static ILexer Instance => _instance;
    }
}