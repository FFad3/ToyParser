namespace ToyParser.Parser.ASTnodes
{
    public sealed class CompilationUnit(List<ASTNode> nodes) : ASTNode
    {
        private readonly List<ASTNode> _nodes = nodes;

        public override IEnumerable<ASTNode> GetChildren() => _nodes;

        public override string GetValue() => string.Empty;
    }
}