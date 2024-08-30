using System.Text;
using ToyParser.Lexer;
using ToyParser.Utils;

namespace ToyParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();
            var fileInfos = GetFileInfos(directory, 20);

            //Get specific file content
            var content = ReadFromFileInfo(fileInfos.ToArray()[0]);
            ILexer lexer = new Lexer.Lexer(content.AsMemory(), TokenType.WHITE_SPACE, TokenType.COMMENT_MULTI_LINE, TokenType.WHITE_SPACE);
            List<Token> tokens = new List<Token>();
            Token token;
            do
            {
                token = lexer.GetNextToken();
                tokens.Add(token);
            } while (token.Type != TokenType.EOF);
            DisplayTokenTable(tokens);
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

        private static string ReadFromFileInfo(FileInfo fileInfo)
        {
            string content;
            using (StreamReader reader = fileInfo.OpenText())
            {
                content = reader.ReadToEnd();
            }
            return content;
        }

        private static void PrintFilesSummmary(FileInfo[] files, int spacing)
        {
            var sb = new StringBuilder();
            sb.AppendLine(new string('-', 8 + (spacing * 2)));
            sb.AppendLine($"{"Number".PadRight(8)}{"FileName".PadRight(spacing)}{"FileSize".PadRight(spacing)}");
            sb.AppendLine(new string('-', 8 + (spacing * 2)));
            for (int i = 0; i < files.Length; i++)
            {
                var fileName = files[i].Name;
                var fileSize = FileSizeExtension.FormatFileSizeAsString(files[i].Length);
                sb.AppendLine($"{i.ToString().PadRight(8)}{fileName.PadRight(spacing)}{fileSize.PadRight(spacing)}");
            }
            Console.WriteLine(sb);
        }

        private static IEnumerable<FileInfo> GetFileInfos(string directory, int spacing)
        {
            string solutionDirectory = Directory.GetParent(directory)!.Parent!.Parent!.Parent!.FullName;
            var files = Directory.GetFiles(solutionDirectory, "*.txt").Select(f => new FileInfo(f)).ToArray();

            PrintFilesSummmary(files, spacing);

            return files;
        }
    }
}