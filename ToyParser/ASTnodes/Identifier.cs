namespace ToyParser.Parser.ASTnodes
{
    public sealed class Identifier(string value) : ASTNode
    {
        public string Value { get; } = value;

        public override IEnumerable<ASTNode> GetChildren() => [];

        public override string GetValue() => Value;
    }
}