﻿namespace ToyParser.Lexer
{
    public enum TokenType
    {
        EOF,
        ILLEGAL,

        WHITE_SPACE,
        COMMENT_SINGLE_LINE,
        COMMENT_MULTI_LINE,

        TRUE,
        FALSE,
        RETURN,

        IDNETIFIER,
        DIGIT,
        STRING_LITERAL,

        ASSIGN,
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,

        EQUALS,
        NOT_EQUALS,
        LESS_THAN,
        GREATER_THAN,
        LESS_THAN_OR_EQUALS,
        GREATER_THAN_OR_EQUALS,

        SEMICOLON,
        COMMA,
        DOT,
        EXCLAMATION,

        L_PAREN,
        R_PAREN,
        L_BRACE,
        R_BRACE,
    }
}