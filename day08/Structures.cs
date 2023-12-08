class Node
{
    public Node(string current, string left, string right)
    {
        Current = current;
        CurrentEnd = current[^1];
        Left = left;
        Right = right;
    }
    public string Current { get; }
    public string Left { get; }
    public string Right { get; }

    public char CurrentEnd { get; }

    public override int GetHashCode()
    {
        return Current.GetHashCode();
    }
}
