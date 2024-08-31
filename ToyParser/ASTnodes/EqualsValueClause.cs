using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class EqualsValueClause(ASTNode node) : ASTNode
    {
        public ASTNode ValueClause { get; } = node;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren() => [ValueClause];
    }
}