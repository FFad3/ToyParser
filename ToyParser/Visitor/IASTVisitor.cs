using ToyParser.Parser.ASTnodes;

namespace ToyParser.Parser.Visitor
{
    public interface IASTVisitor
    {
        void Visit(CompilationUnit compilationUnit);

        void Visit(ASTNode node, bool isLast);
    }

    public class ASTVisitor : IASTVisitor
    {
        private static class Symbol
        {
            public const string Cross = "├─";
            public const string Corner = "└─";
            public const string Vertical = "│ ";
            public const string Space = "  ";
        }

        private readonly Stack<string> _indents = new Stack<string>();

        private void PrintIndent()
        {
            foreach (var indent in _indents.Reverse())
            {
                Console.Write(indent);
            }
        }

        public void Visit(CompilationUnit compilationUnit)
        {
            Console.WriteLine(compilationUnit);
            VisitChildren(compilationUnit);
        }

        public void Visit(ASTNode node, bool isLast)
        {
            PrintIndent();
            Console.WriteLine(node);
            _indents.Pop();

            if (isLast)
            {
                _indents.Push(Symbol.Space);
            }
            else
            {
                _indents.Push(Symbol.Vertical);
            }

            VisitChildren(node);
            _indents.Pop();
        }

        private void VisitChildren(ASTNode node)
        {
            _indents.Push(Symbol.Space);
            var children = node.GetChildren().ToArray();
            for (int i = 0; i < children.Length; i++)
            {
                if (i == children.Length - 1)
                {
                    _indents.Push(Symbol.Corner);
                    Visit(children[i], true);
                }
                else
                {
                    _indents.Push(Symbol.Cross);
                    Visit(children[i], false);
                }
            }
            _indents.Pop();
        }
    }
}