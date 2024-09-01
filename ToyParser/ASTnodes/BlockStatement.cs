using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class BlockStatement(IEnumerable<ASTNode> statements, ReturnStatement returnStatement) : ASTNode
    {
        private readonly IEnumerable<ASTNode> _statements = statements;
        public ReturnStatement ReturnStatement { get; } = returnStatement;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren()
        {
            var nodes = new List<ASTNode>();
            nodes.AddRange(_statements);
            nodes.Add(ReturnStatement);
            return nodes;
        }
    }
}