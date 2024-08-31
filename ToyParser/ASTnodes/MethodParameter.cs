using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class MethodParameter(VariableType Type, Identifier Identifier) : ASTNode
    {
        public VariableType Type { get; } = Type;
        public Identifier Identifier { get; } = Identifier;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string GetValue() => Type.ToString();

        public override IEnumerable<ASTNode> GetChildren() => [Identifier];
    }
}