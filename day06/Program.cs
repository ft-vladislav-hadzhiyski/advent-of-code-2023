var input = @"Time:        40     82     84     92
Distance:   233   1011   1110   1487";

var sampleInput = @"Time:      7  15   30
Distance:  9  40  200";

var lines = input
    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

{
    var parts = lines.Select(line =>
        line
        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Skip(1)
        .Select(int.Parse)
        .ToList())
    .ToList();

    var races = parts[0].Select((time, index) => new
    {
        Time = time,
        Distance = parts[1][index]
    }).ToList();

    var possibleWaysToWin = races.Select(race =>
    {
        var range = Enumerable.Range(1, race.Time);
        var dist = range.Select(r => (race.Time - r) * r).ToList();
        var win = dist.Where(d => d > race.Distance).Count();

        return win;
    }).ToList();

    Console.WriteLine("Task 1 = {0}", possibleWaysToWin.Aggregate((r, c) => r * c));
}

{
    var parts = lines.Select(line =>
        line
        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Skip(1)
        .Aggregate((r, s) => r + s))
        .Select(long.Parse)
    .ToList();
    var time = parts[0];
    var distance = parts[1];

    var range = new List<long>();
    for (var i = 1; i < time; i += 1)
    {
        range.Add(i);
    }
    var dist = range.Select(r => (time - r) * r).ToList();
    var win = dist.Where(d => d > distance).Count();

    Console.WriteLine("Task 2 = {0}", win);
}

Console.ReadLine();
