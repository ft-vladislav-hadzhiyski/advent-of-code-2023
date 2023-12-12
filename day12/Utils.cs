public static class CombinationGenerator
{
    public static IEnumerable<string> Generate(string input, int[] counts)
    {
        var combinations = new List<string>();
        char[] chars = input.ToCharArray();

        GenerateRecursive(chars, 0, combinations, counts);

        return combinations;
    }

    static void GenerateRecursive(char[] chars, int index, List<string> combinations, int[] counts)
    {
        if (index == chars.Length)
        {
            var chb = chars.Count(ch => ch == '#');
            if (chb == counts.Sum())
            {
                combinations.Add(new string(chars));
            }

            return;
        }

        if (chars[index] == '?')
        {
            chars[index] = '.';
            GenerateRecursive(chars, index + 1, combinations, counts);

            if (chars.Count(ch => ch == '#') < counts.Sum())
            {
                chars[index] = '#';

                GenerateRecursive(chars, index + 1, combinations, counts);
            }

            chars[index] = '?'; // reset for backtracking
        }
        else
        {
            GenerateRecursive(chars, index + 1, combinations, counts);
        }
    }
}
