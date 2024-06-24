namespace PennyPlanner.Utils
{
    public static class NamingUtils
    {
        public static string ConvertToCamelCaseWithSpaces(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString()));
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.ToLower());
        }
    }
}
