namespace NamedRangeTestApp.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveFirstCharacter(this string str)
        {
            var result = str.Remove(0, 1);

            return result;
        }

        public static string GetFirstCharacter(this string str)
        {
            var result = str.Substring(0, 1);

            return result;
        }
    }
}
