enum SpringStatusState
{
    Operational,
    Broken,
    Unknown
}

class Spring
{
    public Spring(string value, int[] counts)
    {
        Value = value;
        Counts = counts;

        var statuses = GetStatuses(value);
        Statuses = statuses;
        Groups = GetGroups(statuses);
    }

    public bool HasMatch()
    {
        var brokenGroups = Groups.Where(gr => gr[0] == SpringStatusState.Broken).Select(gr => gr.Count).ToArray();
        return Enumerable.SequenceEqual(brokenGroups, Counts);
    }

    public string Value { get; }
    public List<List<SpringStatusState>> Groups { get; }
    public SpringStatusState[] Statuses { get; }
    public int[] Counts { get; }

    private static SpringStatusState[] GetStatuses(string value)
    {
        return value.ToCharArray().Select((ch, i) =>
        {
            var state = ch switch
            {
                '.' => SpringStatusState.Operational,
                '#' => SpringStatusState.Broken,
                '?' => SpringStatusState.Unknown,
                _ => throw new Exception("Unknown spring status"),
            };

            return state;
        })
            .ToArray();
    }

    private static List<List<SpringStatusState>> GetGroups(SpringStatusState[] statuses)
    {
        var statusGroups = new List<List<SpringStatusState>>();

        for (var i = 1; i < statuses.Length; i += 1)
        {
            if (i == 1)
            {
                statusGroups.Add([statuses[i - 1]]);
            }
            var prev = statuses[i - 1];
            var next = statuses[i];
            if (prev != next)
            {
                statusGroups.Add([next]);
            }
            else
            {
                statusGroups.Last().Add(next);
            }
        }

        return statusGroups;
    }
}

class ComboCounts(IEnumerable<string> combos, int[] counts)
{
    public IEnumerable<string> Combos { get; } = combos;
    public int[] Counts { get; } = counts;
}
