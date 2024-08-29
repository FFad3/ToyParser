using System.Text.RegularExpressions;

namespace ToyParser.Utils
{
    public static class RegexExtension
    {
        public static Match Match(this Regex regex, ReadOnlyMemory<char> readOnlyMemory)
        {
            string content = new string(readOnlyMemory.Span);
            return regex.Match(readOnlyMemory.ToString());
        }
    }
}