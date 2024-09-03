namespace ToyParser.Lexer
{
    public enum TokenType
    {
        EOF,
        ILLEGAL,

        WHITE_SPACE,
        COMMENT_SINGLE_LINE,
        COMMENT_MULTI_LINE,

        CLASS,
        INT,
        STRING,
        BOOLEAN,

        TRUE,
        FALSE,
        IF,
        ELSE,
        FOR,
        RETURN,

        IDENTIFIER,
        DIGIT,
        STRING_LITERAL,

        ASSIGN,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        MODULO,

        EQUALS,
        NOT_EQUALS,
        LESS_THAN,
        GREATER_THAN,
        LESS_THAN_OR_EQUALS,
        GREATER_THAN_OR_EQUALS,

        AND,
        OR,

        SEMICOLON,
        COMMA,
        DOT,
        EXCLAMATION,

        L_PAREN,
        R_PAREN,
        L_BRACE,
        R_BRACE,
        L_BRACKET,
        R_BRACKET,
    }
}