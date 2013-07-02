using System.Linq;

namespace OptParse.Extensions
{
    public static class StringExtensions
    {
        public static bool IsArgument(this string value)
        {
            return !string.IsNullOrEmpty(value) && value.Contains('-');
        }

        public static bool Exists(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
