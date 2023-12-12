public class SpacePoint(int row, int col, char symbol)
{
    public int Row { get; set; } = row;
    public int Col { get; set; } = col;
    public char Symbol { get; } = symbol;
    public bool IsGalaxy { get; } = symbol == '#';

    public override string ToString()
    {
        return $"{Row}:{Col} {Symbol}";
    }
}
