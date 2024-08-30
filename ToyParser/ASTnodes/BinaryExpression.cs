using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class BinaryExpression(ASTNode Left, Operator Operator, ASTNode Right) : ASTNode
    {
        public ASTNode Left { get; } = Left;
        public Operator Operator { get; } = Operator;
        public ASTNode Right { get; } = Right;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override IEnumerable<ASTNode> GetChildren() => [Left, Right];

        public override string GetValue() => Operator.ToString();
    }

}