class Mapping(long source, long destination, long length)
{
    public long Source { get; } = source;
    public long Destination { get; } = destination;
    public long Length { get; } = length;

    public long Map(long seed)
    {
        if (seed < Source || seed > Source + Length)
        {
            return seed;
        }

        var diff = seed - Source;

        return Destination + diff;
    }

    public bool InRange(long seed)
    {
        return seed >= Source && seed <= Source + Length;
    }

    public override string ToString()
    {
        return $"{Source}-{Destination}:{Length}";
    }
}
