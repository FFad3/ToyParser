namespace ToyParser.Parser.ASTnodes
{
    public sealed class MethodCall(Identifier identifier, IEnumerable<MethodCallParameter> parameters) : ASTNode
    {
        public Identifier Identifier { get; } = identifier;
        private readonly IEnumerable<MethodCallParameter> _parameter = parameters;

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren()
        {
            List<ASTNode> children = new List<ASTNode>();
            children.Add(Identifier);
            children.AddRange(_parameter);
            return children;
        }
    }

    public sealed class MethodCallParameter(ASTNode paremeter) : ASTNode
    {
        public ASTNode Parameter { get; } = paremeter;

        public override string GetValue() => string.Empty;

        public override IEnumerable<ASTNode> GetChildren() => [Parameter];
    }
}