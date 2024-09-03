using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class ConditionalExpression(ASTNode condition, BlockStatement trueBlock, BlockStatement? falseBlock) : ASTNode
    {
        public ASTNode Condition { get; } = condition;
        public BlockStatement TrueBlock { get; } = trueBlock;
        public BlockStatement? FalseBlock { get; } = falseBlock;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren()
        {
            yield return Condition;
            yield return TrueBlock;
            if (FalseBlock is not null)
                yield return FalseBlock;
        }
    }
}