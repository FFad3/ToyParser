using System.Data;
using ToyParser.Lexer;
using ToyParser.Parser.ASTnodes;

namespace ToyParser.Parser
{
    internal class CustomParser
    {
        private ReadOnlyMemory<char> _content;
        private readonly ILexer _lexer;
        private Token _currentToken;

        public CustomParser(ILexer lexer)
        {
            this._lexer = lexer;
        }

        private void Eat(TokenType type)
        {
            if (this._currentToken.Type != type)
                throw new SyntaxErrorException($"Unexpected token: {this._currentToken.Type}, expected: {type}");

            this._currentToken = this._lexer.GetNextToken();
        }

        public CompilationUnit Parse(FileInfo file)
        {
            using (StreamReader reader = file.OpenText())
            {
                this._content = reader.ReadToEnd().AsMemory();
            }

            _lexer.SetContent(this._content);
            this._currentToken = this._lexer.GetNextToken();

            return Parse();
        }

        private CompilationUnit Parse()
        {
            var nodes = new List<ASTNode>();
            while (this._currentToken.Type != TokenType.EOF)
            {
                nodes.Add(ParseStatement());
            }
            var node = new CompilationUnit(nodes);
            return node;
        }

        private ASTNode ParseStatement()
        {
            return ParseExpression();
        }

        private ASTNode ParseExpression()
        {
            return ParseBinaryExpression();
        }

        private ASTNode ParseBinaryExpression(int precedence = 0)
        {
            var left = ParsePrimary();
            while (true)
            {
                var operatorPrecedence = GetPrecedence(this._currentToken);
                if (operatorPrecedence <= precedence)
                {
                    break;
                }

                var @operator = ParseOperator();
                var right = ParseBinaryExpression(operatorPrecedence);
                left = new BinaryExpression(left, @operator, right);
            }

            return left;
        }
        private ASTNode ParsePrimary()
        {
            return _currentToken.Type switch
            {
                TokenType.DIGIT => ParseNumericLiteral(),
                TokenType.STRING_LITERAL => ParseStringLiteral(),
                TokenType.L_PAREN => ParseInneBinaryExpression(),
                _ => throw new SyntaxErrorException($"Unexpected token {_currentToken}")
            };
        }

        private ASTNode ParseInneBinaryExpression()
        {
            Eat(TokenType.L_PAREN);
            var exp = ParseBinaryExpression();
            Eat(TokenType.R_PAREN);
            return exp;
        }

        private StringLiteral ParseStringLiteral()
        {
            var literal = new StringLiteral(_currentToken.Value);
            Eat(TokenType.STRING_LITERAL);
            return literal;
        }

        private NumericLiteral ParseNumericLiteral()
        {
            var literal = new NumericLiteral(_currentToken.Value);
            Eat(TokenType.DIGIT);
            return literal;
        }

        private Operator ParseOperator()
        {
            var @operator = new Operator(this._currentToken.Type, this._currentToken.Value);
            Eat(@operator.Type);
            return @operator;
        }

        private static int GetPrecedence(Token token)
        {
            return token.Type switch
            {
                TokenType.MULTIPLY => 2, // * and / have higher precedence
                TokenType.DIVIDE => 2,
                TokenType.PLUS => 1,     // + and - have lower precedence
                TokenType.MINUS => 1,
                _ => 0                   // Other tokens have the lowest precedence
            };
        }
    }
}