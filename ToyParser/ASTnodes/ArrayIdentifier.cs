namespace ToyParser.Parser.ASTnodes
{
    public sealed class ArrayIdentifier(Identifier identifier, ArrayIndexCall index) : ASTNode
    {
        public Identifier Identifier { get; } = identifier;
        public ArrayIndexCall Index { get; } = index;

        public override IEnumerable<ASTNode> GetChildren() => [Identifier,Index];

        public override string GetValue() => string.Empty;
    }
}