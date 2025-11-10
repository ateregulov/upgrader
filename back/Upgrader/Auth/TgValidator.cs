using System.Security.Cryptography;
using System.Text;

namespace Upgrader.Auth;

public class TgValidator
{
    private const string DataHash = "WebAppData";

    public bool ValidateInitData(string botToken, string initData)
    {
        if (initData == "undefined") return false;

        var data = ParseInitData(initData);
        var hash = data["hash"];
        data.Remove("hash");

        var sortedData = data.OrderBy(kvp => kvp.Key)
            .Select(kvp => $"{kvp.Key}={kvp.Value}")
            .ToArray();
        var concatenatedData = string.Join("\n", sortedData);

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(DataHash));
        var secretKey = hmac.ComputeHash(Encoding.UTF8.GetBytes(botToken));

        using var finalHmac = new HMACSHA256(secretKey);
        var computedHash = finalHmac.ComputeHash(Encoding.UTF8.GetBytes(concatenatedData));
        var computedHashString = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

        return hash == computedHashString;
    }

    public Dictionary<string, string> ParseInitData(string initDataRaw)
    {
        var parameters = initDataRaw.Split('&')
            .Select(p => p.Split('='))
            .ToDictionary(parts => parts[0], parts => Uri.UnescapeDataString(parts[1]));
        return parameters;
    }
}