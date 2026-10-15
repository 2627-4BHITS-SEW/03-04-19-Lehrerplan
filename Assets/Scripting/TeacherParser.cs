
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

public static class TeacherParser
{
    // Alle Lehrer-Links aus der Übersichtsseite auslesen

    public static List<string> ParseTeacherLinks(string html, string roomNumber)
    {
        HashSet<string> links = new HashSet<string>();

        // Aus "C 205" wird "C205"
        string room = Regex.Replace(roomNumber.ToUpperInvariant(), @"[\s_-]", "");

        // Findet beispielsweise "-c205.html" oder "-c-205.html"
        string roomPattern = Regex.Escape(room.Substring(0, 1))
                           + @"[-_\s]?"
                           + Regex.Escape(room.Substring(1));

        string pattern =
            @"href\s*=\s*[""'](?<url>[^""']*/lehrerinnen-details/[^""']+\.html)[""']";

        foreach (Match match in Regex.Matches(html, pattern, RegexOptions.IgnoreCase))
        {
            string link = WebUtility.HtmlDecode(match.Groups["url"].Value);

            if (Regex.IsMatch(link, @"[-_]" + roomPattern + @"\.html$",
                              RegexOptions.IgnoreCase))
            {
                links.Add(link);
            }
        }

        return new List<string>(links);
    }


    // Einzelne Lehrer-Detailseite auslesen
    public static TeacherData ParseTeacherDetails(string html)
    {
        // HTML-Tags entfernen
        string text = Regex.Replace(
            html, @"<script\b[^>]*>.*?</script>", "",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        text = Regex.Replace(
            text, @"<style\b[^>]*>.*?</style>", "",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        text = Regex.Replace(text, @"<[^>]+>", "\n");
        text = WebUtility.HtmlDecode(text);

        List<string> lines = new List<string>();

        foreach (string line in text.Split('\n'))
        {
            string cleaned = line.Trim();

            if (!string.IsNullOrWhiteSpace(cleaned))
                lines.Add(cleaned);
        }

        return new TeacherData
        {
            name = GetValue(lines, "Name"),
            roomNumber = GetValue(lines, "Raum"),
            phone = GetValue(lines, "Tel-DW"),
            consultationHours = GetValue(lines, "Sprechstunde"),
            email = GetValue(lines, "E-Mail")
        };
    }

    // Wert hinter einem Feldnamen ermitteln
    private static string GetValue(List<string> lines, string label)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];

            if (line.Equals(label + ":", StringComparison.OrdinalIgnoreCase))
            {
                return i + 1 < lines.Count ? lines[i + 1] : "-";
            }

            if (line.StartsWith(label + ":", StringComparison.OrdinalIgnoreCase))
            {
                return line.Substring(label.Length + 1).Trim();
            }
        }

        return "-";
    }
}
