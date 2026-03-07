using System.Collections.Generic;

public static class AdBlockService
{
    private static HashSet<string> blockedDomains = new HashSet<string>();

    public static void LoadFilters(IEnumerable<string> domains)
    {
        blockedDomains = new HashSet<string>(domains);
    }

    public static bool IsAd(string url)
    {
        foreach (var domain in blockedDomains)
        {
            if (url.Contains(domain))
                return true;
        }

        return false;
    }
}