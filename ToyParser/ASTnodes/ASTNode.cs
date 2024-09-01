using ToyParser.Parser.Visitor;

namespace ToyParser.Parser.ASTnodes
{
    public abstract class ASTNode
    {
        public string Name => this.GetType().Name;

        public abstract string GetValue();

        public abstract IEnumerable<ASTNode> GetChildren();

        public virtual void Accept(IASTVisitor visitor, bool IsLast)
        {
            visitor.Visit(this, IsLast);
        }

        public override string ToString()
        {
            return $"{Name} : {GetValue()}";
        }
    }
}