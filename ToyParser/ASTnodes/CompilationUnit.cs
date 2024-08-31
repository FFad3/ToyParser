using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class CompilationUnit(List<ASTNode> nodes) : ASTNode
    {
        private readonly List<ASTNode> _nodes = nodes;

        public override IEnumerable<ASTNode> GetChildren() => _nodes;

        public override string GetValue() => string.Empty;

        public override void Accept(IASTVisitor visitor, bool IsLast)
        {
            visitor.Visit(this);
        }
    }
}