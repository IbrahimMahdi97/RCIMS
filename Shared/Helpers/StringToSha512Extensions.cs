using System.Security.Cryptography;
using System.Text;

namespace Shared.Helpers;

public static class StringToSha512Extensions
{
    public static string ToSha512(this string text)
    {
        var utf8 = new UTF8Encoding();
        var passBuff = utf8.GetBytes(text);

        var hashVal = SHA512.Create();
        var passHash = hashVal.ComputeHash(passBuff);

        return Convert.ToHexString(passHash);
    }
}