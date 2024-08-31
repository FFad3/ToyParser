using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class ReturnStatement(ASTNode expression) : ASTNode
    {
        public ASTNode Expression { get; } = expression;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren() => [Expression];
    }
}