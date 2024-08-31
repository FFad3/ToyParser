using ToyParser.Lexer;

namespace ToyParser.Tests
{
    public class LexerTests
    {
        [Theory]
        [InlineData("aaa")]
        [InlineData("_aaa")]
        [InlineData("aaa_")]
        [InlineData("a12aa_")]
        public void GetNextToken_ReturnsIdentifierToken(string value)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(TokenType.IDENTIFIER, token.Type);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("1")]
        public void GetNextToken_ReturnsNumberToken(string value)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(TokenType.DIGIT, token.Type);
        }

        [Theory]
        [InlineData("\"123\"")]
        [InlineData("\"1\"")]
        public void GetNextToken_ReturnsStringLiteralToken(string value)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(TokenType.STRING_LITERAL, token.Type);
        }

        [Theory]
        [InlineData("(", TokenType.L_PAREN)]
        [InlineData(")", TokenType.R_PAREN)]
        public void GetNextToken_ReturnsParenToken(string value, TokenType tokenType)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(tokenType, token.Type);
        }

        [Theory]
        [InlineData("{", TokenType.L_BRACE)]
        [InlineData("}", TokenType.R_BRACE)]
        public void GetNextToken_ReturnsBraceToken(string value, TokenType tokenType)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(tokenType, token.Type);
        }

        [Theory]
        [InlineData("=", TokenType.ASSIGN)]
        [InlineData("+", TokenType.PLUS)]
        [InlineData("-", TokenType.MINUS)]
        [InlineData("/", TokenType.DIVIDE)]
        [InlineData("*", TokenType.MULTIPLY)]
        [InlineData("==", TokenType.EQUALS)]
        [InlineData("!=", TokenType.NOT_EQUALS)]
        [InlineData("<", TokenType.LESS_THAN)]
        [InlineData(">", TokenType.GREATER_THAN)]
        [InlineData("<=", TokenType.LESS_THAN_OR_EQUALS)]
        [InlineData(">=", TokenType.GREATER_THAN_OR_EQUALS)]
        public void GetNextToken_ReturnsOperatorToken(string value, TokenType tokenType)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(tokenType, token.Type);
        }

        [Theory]
        [InlineData(";", TokenType.SEMICOLON)]
        [InlineData(",", TokenType.COMMA)]
        [InlineData(".", TokenType.DOT)]
        [InlineData("!", TokenType.EXCLAMATION)]
        public void GetNextToken_ReturnsPunctuationToken(string value, TokenType tokenType)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(tokenType, token.Type);
        }

        [Theory]
        [InlineData("tRue", TokenType.TRUE)]
        [InlineData("false", TokenType.FALSE)]
        [InlineData("if", TokenType.IF)]
        [InlineData("else", TokenType.ELSE)]
        [InlineData("for", TokenType.FOR)]
        [InlineData("return", TokenType.RETURN)]
        public void GetNextToken_ReturnsKeywordToken(string value, TokenType tokenType)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(tokenType, token.Type);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\t\t")]
        [InlineData("\n\n\n\n")]
        public void GetNextToken_ReturnsWhiteSpaceToken(string value)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(TokenType.WHITE_SPACE, token.Type);
        }

        [Theory]
        [InlineData("//1233")]
        [InlineData("//\"dsadsa\"")]
        [InlineData("//   ")]
        public void GetNextToken_SingleLineCommentToken(string value)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(TokenType.COMMENT_SINGLE_LINE, token.Type);
        }

        [Theory]
        [InlineData("/* \" dsada\"*/")]
        [InlineData("/* \n 123 \n \t*/")]
        [InlineData("/* \t\t\t \n\n \t*/")]
        public void GetNextToken_MultiLineCommentToken(string value)
        {
            //Arrange
            ILexer tokenizer = CustomLexer.Instance; tokenizer.SetContent(value.AsMemory());
            //Act
            var token = tokenizer.GetNextToken();
            //Assert
            Assert.Equal(value, token.Value);
            Assert.Equal(TokenType.COMMENT_MULTI_LINE, token.Type);
        }
    }
}