﻿using System.Data;
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

        private ClassDeclaration ParseClassDeclaration()
        {
            Eat(TokenType.CLASS);
            var identifier = ParseIdentifier();
            Eat(TokenType.L_BRACE);
            List<ASTNode> nodes = new List<ASTNode>();
            while (_currentToken.Type != TokenType.R_BRACE)
            {
                nodes.Add(ParseStatement());
            }
            Eat(TokenType.R_BRACE);

            return new ClassDeclaration(identifier, nodes);
        }

        private ASTNode ParseStatement()
        {
            return _currentToken.Type switch
            {
                TokenType.INT => ParseVariableOrMethodDeclaration(ParseIntType()),
                TokenType.STRING => ParseVariableOrMethodDeclaration(ParseStringType()),
                TokenType.BOOLEAN => ParseVariableOrMethodDeclaration(ParseBooleanType()),
                TokenType.IDENTIFIER => ParseAssigmentOrMethodCall(),
                TokenType.CLASS => ParseClassDeclaration(),
                TokenType.IF => ParseConditionalExpression(),
                _ => throw new SyntaxErrorException($"Unexpected token {_currentToken}")
            };
        }

        private ConditionalExpression ParseConditionalExpression()
        {
            Eat(TokenType.IF);
            var condition = ParseExpression();
            var trueBlock = ParseBlockStatement();
            trueBlock.BlockName = "TrueBlock";
            BlockStatement? falseBlock = null;
            if (_currentToken.Type == TokenType.ELSE)
            {
                Eat(TokenType.ELSE);
                falseBlock = ParseBlockStatement();
                falseBlock.BlockName = "FalseBlock";
            }
            return new ConditionalExpression(condition, trueBlock, falseBlock);
        }

        private ASTNode ParseAssigmentOrMethodCall()
        {
            Identifier identifier = ParseIdentifier();
            ASTNode result;
            switch (_currentToken.Type)
            {
                case TokenType.ASSIGN:
                    result = ParseVariableDeclarator(identifier);
                    break;

                case TokenType.L_PAREN:
                    result = ParseMethodCall(identifier);
                    break;

                default:
                    throw new SyntaxErrorException($"Unexpected token {_currentToken}");
            }
            Eat(TokenType.SEMICOLON);
            return result;
        }

        private VariableDeclarator ParseVariableDeclarator()
        {
            var identifier = ParseIdentifier();
            return ParseVariableDeclarator(identifier);
        }

        private VariableDeclarator ParseVariableDeclarator(Identifier identifier)
        {
            var equalsValueClause = ParseEqualsValueClause();
            return new VariableDeclarator(identifier, equalsValueClause);
        }

        private EqualsValueClause ParseEqualsValueClause()
        {
            Eat(TokenType.ASSIGN);
            return _currentToken.Type switch
            {
                TokenType.L_BRACKET => new EqualsValueClause(ParseArrayIndexCall()),
                _ => new EqualsValueClause(ParseExpression())
            };
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
            var declarator = new VariableDeclarator(identifier, ParseEqualsValueClause());
            variableDeclarators.Add(declarator);

            while (_currentToken.Type != TokenType.SEMICOLON)
            {
                Eat(TokenType.COMMA);
                var variableDeclarator = ParseVariableDeclarator();
                variableDeclarators.Add(variableDeclarator);
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
            blockStatement.BlockName = "MethodBlock";

            if (blockStatement.ReturnStatement is null)
                throw new SyntaxErrorException("Method declaration require return statement");

            return new MethodDeclaration(variableType, identifier, methodParameters, blockStatement);
        }

        private BlockStatement ParseBlockStatement()
        {
            Eat(TokenType.L_BRACE);
            ReturnStatement returnStatement = null!;
            List<ASTNode> statements = new List<ASTNode>();
            while (_currentToken.Type != TokenType.R_BRACE)
            {
                switch (_currentToken.Type)
                {
                    case TokenType.RETURN:
                        returnStatement = ParseReturnStatement();
                        break;

                    default:
                        statements.Add(ParseStatement());
                        break;
                }
            }
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
                TokenType.IDENTIFIER => ParseIdentifierOrMethodCall(),
                TokenType.TRUE => ParseTrueBolleanConstant(),
                TokenType.FALSE => ParseFalseBolleanConstant(),
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

        private ASTNode ParseIdentifierOrMethodCall()
        {
            Identifier identifier = ParseIdentifier();
            return _currentToken.Type switch
            {
                TokenType.L_PAREN => ParseMethodCall(identifier),
                TokenType.L_BRACKET => ParseArrayIndexIdentifier(identifier),
                _ => identifier
            };
        }

        private MethodCall ParseMethodCall(Identifier identifier)
        {
            Eat(TokenType.L_PAREN);
            IEnumerable<MethodCallParameter> _methodCallParameters = ParseMethodParameters();
            Eat(TokenType.R_PAREN);
            return new MethodCall(identifier, _methodCallParameters);
        }

        private IEnumerable<MethodCallParameter> ParseMethodParameters()
        {
            List<MethodCallParameter> methodCallParameters = new List<MethodCallParameter>();
            while (_currentToken.Type != TokenType.R_PAREN)
            {
                var parameter = ParseMethodCallParameter();
                methodCallParameters.Add(parameter);

                if (_currentToken.Type == TokenType.COMMA)
                    Eat(TokenType.COMMA);
            }
            return methodCallParameters;
        }

        private MethodCallParameter ParseMethodCallParameter()
        {
            ASTNode node = ParsePrimary();
            return new MethodCallParameter(node);
        }

        private ArrayIdentifier ParseArrayIndexIdentifier(Identifier identifier)
        {
            var index = ParseArrayIndexCall();
            return new ArrayIdentifier(identifier, index);
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

        private BooleanConstant ParseTrueBolleanConstant() => ParseBooleanConstant(TokenType.TRUE);

        private BooleanConstant ParseFalseBolleanConstant() => ParseBooleanConstant(TokenType.FALSE);

        private BooleanConstant ParseBooleanConstant(TokenType tokenType)
        {
            var booleanConstant = new BooleanConstant(_currentToken.Value);
            Eat(tokenType);
            return booleanConstant;
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
                TokenType.GREATER_THAN => 30,
                TokenType.LESS_THAN => 30,
                TokenType.GREATER_THAN_OR_EQUALS => 30,
                TokenType.LESS_THAN_OR_EQUALS => 30,
                TokenType.EQUALS => 30,
                TokenType.NOT_EQUALS => 30,
                TokenType.MULTIPLY => 4, // * and / have higher precedence
                TokenType.DIVIDE => 4,
                TokenType.MODULO => 4,
                TokenType.PLUS => 3,     // + and - have lower precedence
                TokenType.MINUS => 3,
                TokenType.AND => 2,
                TokenType.OR => 1,
                _ => 0                   // Other tokens have the lowest precedence
            };
        }
    }
}