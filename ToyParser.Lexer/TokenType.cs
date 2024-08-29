namespace ToyParser.Lexer
{
    public enum TokenType
    {
        EOF,
        ILLEGAL,
        IDNETIFIER,
        DIGIT,

        ASSIGN,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,

        EQUALS,
        NOT_EQUALS,
        LESS_THAN,
        GREATER_THAN,

        SEMICOLON,
        COMMA,
        DOT,
        EXCLAMATION,

        L_PAREN,
        R_PAREN,

        L_BRACE,
        R_BRACE,

        TRUE,
        FALSE,

        RETURN,
    }
}