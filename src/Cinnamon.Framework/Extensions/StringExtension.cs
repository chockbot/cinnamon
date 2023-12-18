namespace Cinnamon.Framework.Extensions.String;

public static class StringExtension 
{
    public static string Capitalize(this string value) =>
        value switch 
        {
            null => throw new ArgumentNullException(nameof(value)),
            "" => throw new ArgumentException($"{nameof(value)} cannot be empty", nameof(value)),
            _ => value[0].ToString().ToUpper() + value.Substring(1)
        };
}