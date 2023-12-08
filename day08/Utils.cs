static class Utils
{
    public static long GCD(long a, long b)
    {
        if (a == 0)
        {
            return b;
        }

        return GCD(b % a, a);
    }

    public static long LCM(long a, long b)
    {
        return a * b / GCD(a, b);
    }
}
