namespace ToyParser.Lexer
{
    public readonly struct Position(int start, int end)
    {
        public int Start { get; } = start;
        public int End { get; } = end;
        public int Length => End - Start;

        public override string ToString()
        {
            return $"POSITION: [{Start},{End}] LENGHT: {Length}";
        }
    }
}