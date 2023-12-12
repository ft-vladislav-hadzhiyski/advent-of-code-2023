using System.Collections.Concurrent;
using System.Diagnostics;

public static class TaskSolver
{
    public static void Run(string[] input, int expandBy)
    {
        var lines = input
        .Select((line, li) =>
            line.ToCharArray().ToList())
        .ToList();

        var spacePoints = lines
            .Select((line, row) =>
                line.Select((symbol, col) => new SpacePoint(row, col, symbol)).ToList())
            .ToList();

        var galaxies = spacePoints.SelectMany((s, row) => s.Where(ss => ss.IsGalaxy).ToList()).ToList();
        ExpandRows(expandBy, spacePoints, galaxies);
        ExpandCols(expandBy, spacePoints, galaxies);


        var galaxiesWithIndex = galaxies.Select((galaxy, index) => new { Galaxy = galaxy, Index = index }).ToList();
        var pairs = galaxiesWithIndex
            .SelectMany(g => galaxiesWithIndex, (a, b) => Tuple.Create(a, b))
            .Where(g => g.Item1.Index < g.Item2.Index).Select(tup => Tuple.Create(tup.Item1.Galaxy, tup.Item2.Galaxy)).ToList();



        var sum = pairs.Select(pair => 0L + Math.Abs(pair.Item1.Row - pair.Item2.Row) + Math.Abs(pair.Item1.Col - pair.Item2.Col)).Sum();

        Console.WriteLine("Result = {0}", sum);
    }

    private static void ExpandCols(int expandBy, List<List<SpacePoint>> spacePoints, List<SpacePoint> galaxies)
    {
        var spacePointsCols = spacePoints
                    .SelectMany((row) =>
                        row.Select((point, col) => new { Index = col, Point = point }))
                    .GroupBy(x => x.Index, r => r.Point)
                    .ToList();

        var emptyColIndices = spacePointsCols
            .Select((line, row) => new
            {
                Empty = line.All(s => s.Symbol == '.'),
                Index = row,
            })
            .Where(x => x.Empty)
            .Select(x => x.Index)
            .ToList();

        for (var i = 0; i < emptyColIndices.Count; i += 1)
        {
            var index = emptyColIndices[i];
            var pointsRight = galaxies.Where(g => g.Col > index).ToList();

            foreach (var point in pointsRight)
            {
                point.Col += expandBy;
            }

            for (var j = i + 1; j < emptyColIndices.Count; j += 1)
            {
                emptyColIndices[j] += expandBy;
            }
        }
    }

    private static void ExpandRows(int expandBy, List<List<SpacePoint>> spacePoints, List<SpacePoint> galaxies)
    {
        var emptyRowIndices = spacePoints
                    .Select((line, row) => new
                    {
                        Empty = line.All(s => s.Symbol == '.'),
                        Index = row,
                    })
                    .Where(x => x.Empty)
                    .Select(x => x.Index)
                    .ToList();

        for (var i = 0; i < emptyRowIndices.Count; i += 1)
        {
            var index = emptyRowIndices[i];
            var pointsBelow = galaxies.Where(g => g.Row > index).ToList();

            foreach (var point in pointsBelow)
            {
                point.Row += expandBy;
            }

            for (var j = i + 1; j < emptyRowIndices.Count; j += 1)
            {
                emptyRowIndices[j] += expandBy;
            }

        }
    }
}
