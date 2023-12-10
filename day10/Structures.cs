class Position(int row, int col) : IEquatable<Position>
{
    public int Row { get; } = row;
    public int Col { get; } = col;

    public bool Equals(Position? other)
    {
        if (other is null)
        {
            return false;
        }

        return Row == other.Row && Col == other.Col;
    }

    public override string ToString()
    {
        return $"{Row}:{Col}";
    }
}

enum Direction
{
    Start,
    Ground,
    NorthSouth,
    EastWest,
    NorthEast,
    NorthWest,
    SouthWest,
    SouthEast,
}

class MapEntry(Position position, Direction direction) : IEquatable<MapEntry>
{
    public Position Position { get; } = position;
    public Direction Direction { get; } = direction;

    public bool Visited { get; set; }

    public bool Equals(MapEntry? other)
    {
        if (other is null)
        {
            return false;
        }

        return Position.Equals(other.Position) && Direction == other.Direction;
    }

    public override string ToString()
    {
        return $"Direction: {Direction} Position:{Position}";
    }
}

class Path(MapEntry start)
{
    public MapEntry Start { get; } = start;
    public List<MapEntry> Entries { get; } = [];
}

class Map
{
    public Map(MapEntry[][] entries)
    {
        Directions = entries;
        var startPosition = entries.SelectMany(e => e).Single(e => e.Direction == Direction.Start);
        startPosition.Visited = true;
        Start = startPosition;
    }

    public MapEntry[][] Directions { get; }
    public MapEntry Start { get; }
    public bool CanMove(MapEntry from, MapEntry to)
    {
        switch (from.Direction)
        {
            case Direction.Ground:
                return false;
            case Direction.NorthSouth:
                {
                    return
                        (from.Position.Row + 1 == to.Position.Row && from.Position.Col == to.Position.Col) ||
                        (from.Position.Row - 1 == to.Position.Row && from.Position.Col == to.Position.Col);
                }
            case Direction.EastWest:
                {
                    return
                        (from.Position.Row == to.Position.Row && from.Position.Col + 1 == to.Position.Col) ||
                        (from.Position.Row == to.Position.Row && from.Position.Col - 1 == to.Position.Col);
                }
            case Direction.NorthEast:
                {
                    return
                        (from.Position.Row - 1 == to.Position.Row && from.Position.Col == to.Position.Col) ||
                        (from.Position.Row == to.Position.Row && from.Position.Col + 1 == to.Position.Col);
                }
            case Direction.SouthEast:
                {
                    return
                        (from.Position.Row == to.Position.Row && from.Position.Col + 1 == to.Position.Col) ||
                        (from.Position.Row + 1 == to.Position.Row && from.Position.Col == to.Position.Col);
                }
            case Direction.NorthWest:
                {
                    return
                        (from.Position.Row == to.Position.Row && from.Position.Col - 1 == to.Position.Col) ||
                        (from.Position.Row - 1 == to.Position.Row && from.Position.Col == to.Position.Col);
                }
            case Direction.SouthWest:
                {
                    return
                        (from.Position.Row + 1 == to.Position.Row && from.Position.Col == to.Position.Col) ||
                        (from.Position.Row == to.Position.Row && from.Position.Col - 1 == to.Position.Col);
                }

            default: throw new Exception($"Unsupported direction: {to.Direction} {to.Position}");
        }
    }

    public bool IsSurrounded(MapEntry target, IEnumerable<MapEntry> path)
    {
        var left = Directions[target.Position.Row].Where((d, idx) => idx < target.Position.Col).ToList();
        var right = Directions[target.Position.Row].Where((d, idx) => idx > target.Position.Col).ToList();
        var top = Directions.Where((d, idx) => idx < target.Position.Row).Select(d => d[target.Position.Col]).ToList();
        var bottom = Directions.Where((d, idx) => idx > target.Position.Row).Select(d => d[target.Position.Col]).ToList();

        return path.Any(left.Contains) && path.Any(right.Contains) && path.Any(top.Contains) && path.Any(bottom.Contains);
    }
}
