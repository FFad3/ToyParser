using System.Diagnostics.Metrics;
using System.Text;
using ToyParser.Lexer;
using ToyParser.Parser;
using ToyParser.Parser.Visitor;
using ToyParser.Utils;

namespace ToyParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FileInfo[] files = Directory.GetFiles(Environment.CurrentDirectory, "*.txt").Select(x => new FileInfo(x)).ToArray();
            DisplayFilesSummary(files);

            ILexer lexer = CustomLexer.Instance;
            lexer.SetIgnoredTokens(TokenType.WHITE_SPACE, TokenType.COMMENT_MULTI_LINE, TokenType.COMMENT_SINGLE_LINE);

            var parser = new CustomParser(lexer);
            IASTVisitor visitor = new ASTVisitor();
            foreach (var file in files)
            {
                var tokens = GetTokenList(file);
                DisplayTokenTable(tokens);

                var ASTroot = parser.Parse(file);
                visitor.Visit(ASTroot);
            }
        }

        private static IEnumerable<Token> GetTokenList(FileInfo file)
        {
            ReadOnlyMemory<char> content;
            using (StreamReader reader = file.OpenText())
            {
                content = reader.ReadToEnd().AsMemory();
            }

            ILexer lexer = CustomLexer.Instance;
            lexer.SetIgnoredTokens(TokenType.WHITE_SPACE, TokenType.COMMENT_MULTI_LINE, TokenType.COMMENT_SINGLE_LINE);
            lexer.SetContent(content);
            List<Token> tokens = new List<Token>();
            Token token;
            do
            {
                token = lexer.GetNextToken();
                tokens.Add(token);
            } while (token.Type != TokenType.EOF);
            return tokens;
        }

        private static void DisplayTokenTable(IEnumerable<Token> tokens)
        {
            var sb = new StringBuilder();
            string header = $"{nameof(Token.Type),-20}{nameof(Token.Position.Start),-10}{nameof(Token.Position.End),-10}{nameof(Token.Value),-10}";
            sb.AppendLine(new('-', 5 * 10));
            sb.AppendLine(header);
            sb.AppendLine(new('-', 5 * 10));
            foreach (Token token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.COMMENT_MULTI_LINE:
                        sb.AppendLine($"{token.Type,-20}{token.Position.Start,-10}{token.Position.End,-10}{"//",-10}");
                        break;

                    case TokenType.COMMENT_SINGLE_LINE:
                        sb.AppendLine($"{token.Type,-20}{token.Position.Start,-10}{token.Position.End,-10}{"*/ */",-10}");
                        break;

                    default:
                        sb.AppendLine($"{token.Type,-20}{token.Position.Start,-10}{token.Position.End,-10}{token.Value,-10}");
                        break;
                }
            }
            Console.WriteLine(sb);
        }

        private static void DisplayFilesSummary(FileInfo[] filesInfos)
        {
            const int spacing = 20;
            var sb = new StringBuilder();

            sb.AppendLine(new string('-', 8 + (spacing * 2)));
            sb.Append($"{"Number".PadRight(8)}");
            sb.Append($"{"FileName".PadRight(spacing)}");
            sb.Append($"{"FileSize".PadRight(spacing)}");
            sb.AppendLine();
            sb.AppendLine(new string('-', 8 + (spacing * 2)));

            for (int i = 0; i < filesInfos.Length; i++)
            {
                FileInfo fileInfo = filesInfos[i];
                sb.Append($"{i.ToString().PadRight(8)}");
                sb.Append($"{fileInfo.Name.PadRight(spacing)}");
                sb.Append($"{FileSizeExtension.FormatFileSizeAsString(fileInfo.Length).PadRight(spacing)}");
                sb.AppendLine();
            }
            Console.WriteLine(sb);
        }
    }
}