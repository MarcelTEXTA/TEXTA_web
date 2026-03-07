using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public static class FilterListLoader
{
    public static async Task<List<string>> LoadEasyList()
    {
        var client = new HttpClient();

        var text = await client.GetStringAsync(
            "https://easylist.to/easylist/easylist.txt");

        return ParseList(text);
    }

    private static List<string> ParseList(string text)
    {
        var domains = new List<string>();

        foreach (var line in text.Split('\n'))
        {
            if (line.StartsWith("||"))
            {
                var domain = line.Replace("||", "")
                                 .Split('^')[0];

                domains.Add(domain);
            }
        }

        return domains;
    }
}