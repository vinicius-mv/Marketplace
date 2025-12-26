using Marketplace.Framework;
using System.Text.RegularExpressions;

namespace Marketplace.Domain;

public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
{
    public static ClassifiedAdTitle FromString(string title)
    {
        CheckValidity(title);
        return new ClassifiedAdTitle(title);
    }

    public static ClassifiedAdTitle FromHtml(string htmlTitle)
    {
        var supportedTagsReplaced = htmlTitle
            .Replace("<b>", "")
            .Replace("</b>", "")
            .Replace("<i>", "")
            .Replace("</i>", "")
            .Replace("<u>", "")
            .Replace("</u>", "");

        var value = Regex.Replace(supportedTagsReplaced, "<.*?>", string.Empty);
        CheckValidity(value);

        return new ClassifiedAdTitle(value);
    }

    public string Value { get; }

    /// <summary>
    /// Internal constructor used when rehydrating this value object from persisted events.
    /// It intentionally bypasses runtime validation so historical event data is accepted as-is.
    /// This ensures that applying past events remains deterministic and has no side effects even
    /// if current validation rules or value-object logic have changed since the event was produced.
    /// </summary>
    internal ClassifiedAdTitle(string value) => Value = value;

    public static implicit operator string(ClassifiedAdTitle title) => title.Value;

    private static void CheckValidity(string value)
    {
        if (value.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Title cannot be longer than 100 characters");
    }

    // Satisfy the serialization requirements
    protected ClassifiedAdTitle() { }
}
