using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace NeTelegram.Commands;

public static class CommandParser
{
    private static readonly Regex CommandRegex =
        new(@"^\s*\/([a-z0-9_]{1,32})(?:@([a-z0-9_]+))?(?:\s|$)");

    public static bool TryParse(
        string message,
        [NotNullWhen(true)] out string? command,
        out string? botUsername,
        out IEnumerable<string> args
    )
    {
        if (!TryParseCommand(ref message, out command, out botUsername))
        {
            args = [];
            return false;
        }

        args = ParseArguments(message);

        return true;
    }

    private static bool TryParseCommand(
        ref string message,
        [NotNullWhen(true)] out string? command,
        out string? botUsername
    )
    {
        var match = CommandRegex.Match(message);

        if (!match.Success)
        {
            command = botUsername = null;
            return false;
        }

        command = match.Groups[1].Value;
        botUsername = match.Groups[2].Value;

        message = message.Substring(match.Index + match.Length);

        return true;
    }

    public static IEnumerable<string> ParseArguments(string input)
    {
        var result = new List<string>();
        var span = input.AsSpan();
        var i = 0;

        while (i < span.Length)
        {
            SkipWhitespace(span, ref i);
            if (i >= span.Length) break;

            var c = span[i];
            if (c == '"' || c == '\'')
            {
                result.Add(ParseQuoted(span, ref i));
            }
            else
            {
                result.Add(ParseNonQuoted(span, ref i));
            }
        }

        return result;
    }

    private static string ParseNonQuoted(ReadOnlySpan<char> span, ref int i)
    {
        var start = i;
        while (i < span.Length && !char.IsWhiteSpace(span[i]) && span[i] != '"' && span[i] != '\'')
        {
            i++;
        }

        return span.Slice(start, i - start).ToString();
    }

    private static string ParseQuoted(ReadOnlySpan<char> span, ref int i)
    {
        var quoteChar = span[i++];
        var start = i;
        while (i < span.Length)
        {
            if (span[i] == quoteChar)
            {
                var quoted = span.Slice(start, i - start).ToString();
                i++; // Skip closing quote
                return quoted;
            }

            i++;
        }

        // Unterminated quote, return rest of string as-is
        return span.Slice(start - 1).ToString(); // Include starting quote
    }

    private static void SkipWhitespace(ReadOnlySpan<char> span, ref int i)
    {
        while (i < span.Length && char.IsWhiteSpace(span[i]))
        {
            i++;
        }
    }
}
