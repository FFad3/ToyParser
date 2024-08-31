using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class VariableDeclaration(VariableType Type, List<VariableDeclarator> declarators) : ASTNode
    {
        public VariableType Type { get; } = Type;
        private List<VariableDeclarator> _declarators = declarators;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string GetValue() => $"{Type}";

        public override IEnumerable<ASTNode> GetChildren() => _declarators;
    }
}