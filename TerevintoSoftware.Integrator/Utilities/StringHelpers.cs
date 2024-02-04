using System.Text.RegularExpressions;

namespace TerevintoSoftware.Integrator.Utilities;

internal static partial class StringHelpers
{
    internal static string ToSentenceCase(this string value)
    {
        return value[0].ToString().ToUpperInvariant() + value[1..].ToLowerInvariant();
    }

    internal static string ToKebabCase(this string value)
    {
        // 1. Separate words that have numbers (i.e., B2c => -b2c-)
        var replacedValue = FindPascalCasedWordGroupsWithNumbers().Replace(value, "-$1-");

        if (replacedValue == value)
        {
            // 2. If the previous Regex didn't match, just split words by numbers (i.e., b2c => b-2-c).
            replacedValue = FindAllWordGroupsWithNumbers().Replace(value, "-$1-");
        }

        // 3. Separate SentenceCased words (i.e, HelloWorld => Hello-World)
        replacedValue = FindPascalCasedWordGroups().Replace(replacedValue, "$1-$2");

        // 4. Remove a potential suffix/prefix '-'  from step 1 or 2, and convert the result to lowercase.
        return replacedValue.Trim('-').ToLower();
    }

    [GeneratedRegex("([A-Z]?\\d+[a-z]+)")]
    private static partial Regex FindPascalCasedWordGroupsWithNumbers();

    [GeneratedRegex("(\\d+)")]
    private static partial Regex FindAllWordGroupsWithNumbers();

    [GeneratedRegex("([a-zA-Z])([A-Z])")]
    private static partial Regex FindPascalCasedWordGroups();
}
