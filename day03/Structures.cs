using System.Text.RegularExpressions;

class StringValue(string value, int index)
{
    public string Value { get; } = value;
    public int Index { get; } = index;

    public override string ToString()
    {
        return Value;
    }
}
class NumberValue(int value, int index, int length)
{
    public int Value { get; } = value;
    public int Index { get; } = index;

    public int Length { get; } = length;

    public override string ToString()
    {
        return Value.ToString();
    }
}

class NumbersAndSymbols(IEnumerable<NumberValue> numbers, IEnumerable<StringValue> symbols)
{
    public IEnumerable<NumberValue> Numbers { get; } = numbers;
    public IEnumerable<StringValue> Symbols { get; } = symbols;
}

static class NumbersAndSymbolsFinder
{
    public static NumbersAndSymbols Find(string line, IEnumerable<char> allSymbols)
    {
        var numbers = Regex.Matches(line, @"\d+").Select(match => new NumberValue(int.Parse(match.Value), match.Index, match.Length)).ToList();

        var escapedSymbols = string.Join('\\', allSymbols);
        var symbols = Regex.Matches(line, $"[{escapedSymbols}]").Select(match => new StringValue(match.Value, match.Index)).ToList();

        return new NumbersAndSymbols(numbers, symbols);
    }
}
