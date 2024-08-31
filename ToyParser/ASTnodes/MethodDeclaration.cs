using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class MethodDeclaration(VariableType Type, Identifier Identifier, IEnumerable<MethodParameter> Parameters, BlockStatement BlockStatement) : ASTNode
    {
        public VariableType ReturnType { get; } = Type;
        public Identifier Identifier { get; } = Identifier;
        private readonly List<MethodParameter> _parameters = Parameters.ToList();
        public BlockStatement BlockStatement { get; } = BlockStatement;

        public void AddParameter(MethodParameter parameter) => _parameters.Add(parameter);

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