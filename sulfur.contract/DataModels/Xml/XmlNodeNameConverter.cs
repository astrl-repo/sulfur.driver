using System.Text.RegularExpressions;

namespace Sulfur.Contract.DataModels.Xml
{
    public static class XmlNodeNameConverter
    {
        public static string ProcessXPath(string xpath)
        {
            return Regex.Replace(xpath, @"(?<=//|/)([^\[\]/@]+)", match => EscapeGameObjectName(match.Value));
        }

        public static string EscapeGameObjectName(string name)
        {
            var escapedName = name
                .Replace(" ", "_") // Replace spaces with underscores
                .Replace("(", "_lp_") // Left parenthesis
                .Replace(")", "_rp_") // Right parenthesis
                .Replace(".", "_dot_") // Dots
                .Replace(":", "_colon_"); // Colons

            // Ensure valid XML element name
            if (char.IsDigit(escapedName[0]))
            {
                escapedName = "_" + escapedName; // Prefix with underscore if name starts with a digit
            }

            return escapedName;
        }

        public static string UnescapeGameObjectName(string name)
        {
            var unescapedName = name
                .Replace("_lp_", "(")
                .Replace("_rp_", ")")
                .Replace("_dot_", ".")
                .Replace("_colon_", ":")
                .Replace("_", " ");

            // Remove underscore prefix if it was added to handle leading digits
            if (unescapedName.StartsWith(" "))
            {
                unescapedName = unescapedName.Substring(1);
            }

            return unescapedName;
        }
    }
}