namespace ToyParser.Parser.Visitor
{
    public readonly struct Indent
    {
        public string Value { get; }
        public ConsoleColor Color { get; }

        public Indent(string value, ConsoleColor color)
        {
            Value = value;
            Color = color;
        }

        public static Indent Space(ConsoleColor color) => new Indent(TreeSymbol.Space, color);
        public static Indent Cross(ConsoleColor color) => new Indent(TreeSymbol.Cross, color);
        public static Indent Corner(ConsoleColor color) => new Indent(TreeSymbol.Corner, color);
        public static Indent Vertical(ConsoleColor color) => new Indent(TreeSymbol.Vertical, color);
    }
}