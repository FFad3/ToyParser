namespace ToyParser.Parser.ASTnodes
{
    public sealed class StringLiteral(string value) : ASTNode
    {
        public string Value { get; } = value;

        public override IEnumerable<ASTNode> GetChildren() => [];

        public override string GetValue() => Value;
    }

}