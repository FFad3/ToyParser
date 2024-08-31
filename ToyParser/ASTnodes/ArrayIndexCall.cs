using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class ArrayIndexCall(NumericLiteral numericLiteral) : ASTNode
    {
        public NumericLiteral Index { get; } = numericLiteral;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren() => [Index];
    }
}