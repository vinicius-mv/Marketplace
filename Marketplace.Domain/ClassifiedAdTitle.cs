using Marketplace.Framework;
using System.Text.RegularExpressions;

namespace Marketplace.Domain;

public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
{
    public static ClassifiedAdTitle FromString(string title) => new ClassifiedAdTitle(title);

    public static ClassifiedAdTitle FromHtml(string htmlTitle)
    {
        var supportedTagsReplaced = htmlTitle
            .Replace("<b>", "")
            .Replace("</b>", "")
            .Replace("<i>", "")
            .Replace("</i>", "")
            .Replace("<u>", "")
            .Replace("</u>", "");

        return new ClassifiedAdTitle(Regex.Replace(
            supportedTagsReplaced, "<.*?>", string.Empty));
    }

    private readonly string _value;

    private ClassifiedAdTitle(string value)
    {
        if (value.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Title cannot be longer than 100 characters");

        _value = value;
    }
}
