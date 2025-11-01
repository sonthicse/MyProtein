using System.Net;
using System.Text.RegularExpressions;

namespace MyProtein.Helpers
{
    public static class HtmlSanitizer
    {
        public static string ToPlainText(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var decoded = WebUtility.HtmlDecode(input);

            decoded = Regex.Replace(decoded, @"<\s*br\s*/?>", "\n", RegexOptions.IgnoreCase);
            decoded = Regex.Replace(decoded, @"</\s*p\s*>", "\n", RegexOptions.IgnoreCase);
            decoded = Regex.Replace(decoded, @"<\s*p[^>]*>", string.Empty, RegexOptions.IgnoreCase);

            decoded = Regex.Replace(decoded, @"<[^>]+>", string.Empty);

            decoded = Regex.Replace(decoded, @"\r?\n", " ");
            decoded = Regex.Replace(decoded, @"\s{2,}", " ");

            return decoded.Trim();
        }
    }
}