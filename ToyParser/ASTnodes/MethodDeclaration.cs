using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class MethodDeclaration(VariableType type, Identifier identifier, IEnumerable<MethodParameter> parameters, BlockStatement blockStatement) : ASTNode
    {
        public VariableType ReturnType { get; } = type;
        public Identifier Identifier { get; } = identifier;
        private readonly IEnumerable<MethodParameter> _parameters = parameters;
        public BlockStatement BlockStatement { get; } = blockStatement;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string GetValue() => ReturnType.ToString();

        public override IEnumerable<ASTNode> GetChildren()
        {
            var nodes = new List<ASTNode>() { Identifier };
            nodes.AddRange(_parameters);
            nodes.Add(BlockStatement);
            return nodes;
        }
    }
}