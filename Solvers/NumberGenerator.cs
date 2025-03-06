namespace Library.Solvers
{
    /// <summary>
    /// Methods for generating useful numerical data during development
    /// </summary>
    public class NumberGenerator
    {
        /// <summary>
        /// Create a random 13-digit long that emulates a ISBN code
        /// </summary>
        /// <returns></returns>
        public static long GetRandom13DigitNumber()
        {
            Random random = new Random();
            return Math.Abs((long)(random.NextDouble() * 9_000_000_000_000L) + 1_000_000_000_000L);
        }
    }
}
