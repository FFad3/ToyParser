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
            return _currentToken.Type switch
            {
                TokenType.INT => ParseVariableOrMethodDeclaration(ParseIntType()),
                TokenType.STRING => ParseVariableOrMethodDeclaration(ParseStringType()),
                TokenType.BOOLEAN => ParseVariableOrMethodDeclaration(ParseBooleanType()),
                TokenType.RETURN => ParseReturnStatement(),
                _ => throw new SyntaxErrorException($"Unexpected token {_currentToken}")
            };
        }

        private VariableDeclarator ParseVariableDeclarator(bool isArray)
        {
            var identifier = ParseIdentifier();
            var equalsValueClause = ParseEqualsValueClause(isArray);
            return new VariableDeclarator(identifier, equalsValueClause);
        }

        private EqualsValueClause ParseEqualsValueClause(bool isArray)
        {
            Eat(TokenType.ASSIGN);
            ASTNode node;
            if (isArray)
            {
                node = ParseArrayIndexCall();
            }
            else
            {
                node = ParseExpression();
            }
            return new EqualsValueClause(node);
        }

        private ASTNode ParseVariableOrMethodDeclaration(VariableType variableType)
        {
            Identifier identifier = ParseIdentifier();
            return _currentToken.Type switch
            {
                TokenType.ASSIGN => ParseVariableDeclaracion(variableType, identifier),
                TokenType.L_PAREN => ParseMethodDeclaracion(variableType, identifier),
                _ => throw new SyntaxErrorException($"Unexpected token {_currentToken}")
            };
        }

        private ASTNode ParseVariableDeclaracion(VariableType variableType, Identifier identifier)
        {
            var variableDeclarators = new List<VariableDeclarator>();
            //Process first declarator because we need picked first identifier to recognize if its method or variable declaration
            var declarator = new VariableDeclarator(identifier, ParseEqualsValueClause(variableType.IsArray));
            variableDeclarators.Add(declarator);

            while (_currentToken.Type != TokenType.SEMICOLON)
            {
                var variableDeclarator = ParseVariableDeclarator(variableType.IsArray);
                variableDeclarators.Add(variableDeclarator);

                if (_currentToken.Type == TokenType.COMMA)
                    Eat(TokenType.COMMA);
            }
            Eat(TokenType.SEMICOLON);
            return new VariableDeclaration(variableType, variableDeclarators);
        }

        private ASTNode ParseMethodDeclaracion(VariableType variableType, Identifier identifier)
        {
            Eat(TokenType.L_PAREN);
            List<MethodParameter> methodParameters = new List<MethodParameter>();
            while (_currentToken.Type != TokenType.R_PAREN)
            {
                var methodParameter = ParseMethodParameter();
                methodParameters.Add(methodParameter);

                if (_currentToken.Type == TokenType.COMMA)
                    Eat(TokenType.COMMA);
            }
            Eat(TokenType.R_PAREN);

            BlockStatement blockStatement = ParseBlockStatement();
            return new MethodDeclaration(variableType, identifier, methodParameters, blockStatement);
        }

        private BlockStatement ParseBlockStatement()
        {
            Eat(TokenType.L_BRACE);
            ReturnStatement returnStatement = null!;
            List<ASTNode> statements = new List<ASTNode>();

            do
            {
                var statement = ParseStatement();
                if (statement is ReturnStatement rStatement)
                {
                    returnStatement = rStatement;
                }
                else
                {
                    statements.Add(statement);
                }
            } while (returnStatement is null);
            Eat(TokenType.R_BRACE);
            return new BlockStatement(statements, returnStatement);
        }

        private ReturnStatement ParseReturnStatement()
        {
            Eat(TokenType.RETURN);
            var expression = ParseExpression();
            Eat(TokenType.SEMICOLON);
            return new ReturnStatement(expression);
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
                TokenType.IDENTIFIER => ParseIdentifier(),
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

        private MethodParameter ParseMethodParameter()
        {
            VariableType variableType;
            switch (_currentToken.Type)
            {
                case TokenType.INT:
                    variableType = ParseVariableType(TokenType.INT);
                    break;

                case TokenType.STRING:
                    variableType = ParseVariableType(TokenType.STRING);
                    break;

                case TokenType.BOOLEAN:
                    variableType = ParseVariableType(TokenType.BOOLEAN);
                    break;

                default:
                    throw new SyntaxErrorException($"Unexpected token {_currentToken}");
            }
            Identifier identifier = ParseIdentifier();
            return new MethodParameter(variableType, identifier);
        }

        private Identifier ParseIdentifier()
        {
            var identifier = new Identifier(_currentToken.Value);
            Eat(TokenType.IDENTIFIER);
            return identifier;
        }

        private ArrayIndexCall ParseArrayIndexCall()
        {
            Eat(TokenType.L_BRACKET);
            var numericliteral = ParseNumericLiteral();
            Eat(TokenType.R_BRACKET);
            return new ArrayIndexCall(numericliteral);
        }

        private VariableType ParseIntType() => ParseVariableType(TokenType.INT);

        private VariableType ParseStringType() => ParseVariableType(TokenType.STRING);

        private VariableType ParseBooleanType() => ParseVariableType(TokenType.BOOLEAN);

        private VariableType ParseVariableType(TokenType tokenType)
        {
            Eat(tokenType);
            if (_currentToken.Type == TokenType.L_BRACKET)
            {
                Eat(TokenType.L_BRACKET);
                Eat(TokenType.R_BRACKET);
                return new VariableType(tokenType, _currentToken.Value, true);
            }
            return new VariableType(tokenType, _currentToken.Value, false);
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