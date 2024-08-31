using System.Drawing;
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
        private readonly ColorPicker _colorPicker = new ColorPicker();
        private readonly Stack<Indent> _indents = new Stack<Indent>();

        private void PrintIndent()
        {
            foreach (var indent in _indents.Reverse())
            {
                Console.ForegroundColor = indent.Color;
                Console.Write(indent.Value);
            }
        }

        public void Visit(CompilationUnit compilationUnit)
        {
            Console.WriteLine(compilationUnit);
            var color = Console.ForegroundColor;
            VisitChildren(compilationUnit);
            Console.ForegroundColor = color;
        }

        public void Visit(ASTNode node, bool isLast)
        {
            PrintIndent();
            Console.WriteLine(node);
            var color = _indents.Pop().Color;

            if (isLast)
            {
                _indents.Push(Indent.Space(color));
            }
            else
            {
                _indents.Push(Indent.Vertical(color));
            }

            VisitChildren(node);
            _indents.Pop();
        }

        private void VisitChildren(ASTNode node)
        {
            var color = _colorPicker.GetNextColor();
            _indents.Push(Indent.Space(color));
            var children = node.GetChildren().ToArray();
            for (int i = 0; i < children.Length; i++)
            {
                if (i == children.Length - 1)
                {
                    _indents.Push(Indent.Corner(color));
                    Visit(children[i], true);
                }
                else
                {
                    _indents.Push(Indent.Cross(color));
                    Visit(children[i], false);
                }
            }
            _indents.Pop();
        }
    }
}