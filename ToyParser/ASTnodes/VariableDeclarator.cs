namespace ToyParser.Parser.ASTnodes
{
    public sealed class VariableDeclarator(Identifier identifier, EqualsValueClause initializer) : ASTNode
    {
        public Identifier Identifier { get; } = identifier;
        public EqualsValueClause Initializer { get; } = initializer;

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren() => [Identifier, Initializer];
    }
}