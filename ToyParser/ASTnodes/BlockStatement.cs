using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public sealed class BlockStatement(IEnumerable<ASTNode> statements, ReturnStatement? returnStatement) : ASTNode
    {
        public string BlockName { get; set; } = string.Empty;
        private readonly IEnumerable<ASTNode> _statements = statements;
        public ReturnStatement? ReturnStatement { get; } = returnStatement;

        public override void Accept(IASTVisitor visitor, bool isLast)
        {
            visitor.Visit(this, isLast);
        }

        public override string Name => BlockName;

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren()
        {
            foreach (ASTNode node in _statements)
            {
                yield return node;
            }
            if (ReturnStatement is not null)
                yield return ReturnStatement;
        }
    }
}