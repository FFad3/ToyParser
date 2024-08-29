using System.Text.RegularExpressions;

namespace ToyParser.Lexer
{
    public static class RegexPaterns
    {

        public static readonly Regex ReturnRegex = new(@"(?i)^return", RegexOptions.Compiled);
    }
}
