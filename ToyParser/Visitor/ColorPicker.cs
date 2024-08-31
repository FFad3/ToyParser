namespace ToyParser.Parser.Visitor
{
    internal class ColorPicker
    {
        private readonly ConsoleColor[] _consoleColors;
        private int _current = 0;

        public ColorPicker()
        {
            _consoleColors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        }

        public void Reset() => _current = 0;

        public ConsoleColor GetNextColor()
        {
            _current++;
            var color = _consoleColors[_current];
            if (_current > 14)
            {
                Reset();
            }
            return color;
        }
    }
}