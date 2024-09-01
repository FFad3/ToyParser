namespace ToyParser.Parser.ASTnodes
{
    public sealed class ClassDeclaration(Identifier identifier, IEnumerable<ASTNode> childNodes) : ASTNode
    {
        public Identifier Identifier { get; } = identifier;
        private readonly IEnumerable<ASTNode> _children = childNodes;

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren()
        {
            var nodes = new List<ASTNode>() { Identifier };
            nodes.AddRange(_children);
            return nodes;
        }
    }
}