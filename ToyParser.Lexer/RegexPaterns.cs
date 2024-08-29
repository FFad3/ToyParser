using System.Text.RegularExpressions;

namespace ToyParser.Lexer
{
    public static class RegexPaterns
    {
        public static readonly Regex WhiteSpaceRegex = new(@"^\s+", RegexOptions.Compiled);
        public static readonly Regex SingleLineCommentRegex = new(@"^//.*", RegexOptions.Compiled);
        public static readonly Regex MultiLineCommentRegex = new(@"^/\*[\s\S]*?\*/", RegexOptions.Compiled);

        public static readonly Regex BooleanTrueRegex = new(@"(?i)^true", RegexOptions.Compiled);
        public static readonly Regex BooleanFalseRegex = new(@"(?i)^false", RegexOptions.Compiled);
        public static readonly Regex ReturnRegex = new(@"(?i)^return", RegexOptions.Compiled);

        public static readonly Regex IdentifierRegex = new(@"^[a-zA-Z_]+[a-zA0-9_]*", RegexOptions.Compiled);
        public static readonly Regex DigitRegex = new(@"^\d+", RegexOptions.Compiled);
        public static readonly Regex StringLiteralRegex = new(@"^""[\s\S]*?""", RegexOptions.Compiled);

        public static readonly Regex AssignRegex = new(@"^\=", RegexOptions.Compiled);
        public static readonly Regex EqualsRegex = new(@"^==", RegexOptions.Compiled);
        public static readonly Regex NotEqualsRegex = new(@"^!=", RegexOptions.Compiled);
        public static readonly Regex LessThenOrEqualsRegex = new(@"^<=", RegexOptions.Compiled);
        public static readonly Regex GreaterThenOrEqualsRegex = new(@"^>=", RegexOptions.Compiled);

        public static readonly Regex PlusRegex = new(@"^\+", RegexOptions.Compiled);
        public static readonly Regex MinusRegex = new(@"^-", RegexOptions.Compiled);
        public static readonly Regex MultiplyRegex = new(@"^\*", RegexOptions.Compiled);
        public static readonly Regex DivideRegex = new(@"^/", RegexOptions.Compiled);

        public static readonly Regex LessThenRegex = new(@"^<", RegexOptions.Compiled);
        public static readonly Regex GreaterThenRegex = new(@"^>", RegexOptions.Compiled);

        public static readonly Regex ExclamationRegex = new(@"^!", RegexOptions.Compiled);
        public static readonly Regex DotRegex = new(@"^\.", RegexOptions.Compiled);
        public static readonly Regex CommaRegex = new(@"^,", RegexOptions.Compiled);
        public static readonly Regex SemicolonRegex = new(@"^;", RegexOptions.Compiled);

        public static readonly Regex L_ParenRegex = new(@"^\(", RegexOptions.Compiled);
        public static readonly Regex R_ParenRegex = new(@"^\)", RegexOptions.Compiled);
        public static readonly Regex L_BraceRegex = new(@"^{", RegexOptions.Compiled);
        public static readonly Regex R_BraceRegex = new(@"^}", RegexOptions.Compiled);
    }
}
