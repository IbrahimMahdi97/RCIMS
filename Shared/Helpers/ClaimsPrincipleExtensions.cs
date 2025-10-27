using System.Security.Claims;

namespace Shared.Helpers;

public static class ClaimsPrincipleExtensions
{
    public static int RetrieveUserIdFromPrincipal(this ClaimsPrincipal user)
    {
        return Convert.ToInt32(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    public static IEnumerable<string> RetrieveRolesFromPrincipal(this ClaimsPrincipal user)
    {
        return user.FindAll(ClaimTypes.Role).Select(claim => claim.Value);
    }
}