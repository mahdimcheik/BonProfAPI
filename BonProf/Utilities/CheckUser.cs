using Microsoft.AspNetCore.Identity;
using BonProf.Contexts;
using BonProf.Models;
using System.Security.Claims;

namespace BonProf.Utilities;

public class CheckUser
{
    public static UserApp? GetUserFromClaim(
        ClaimsPrincipal userClaim,
        MainContext context
    )
    {
        string? userEmail = userClaim
            .Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)
            ?.Value;
        if (userEmail == null)
            return null;
        var user = context
            .Users
            .FirstOrDefault(x => x.Email == userEmail);

        if (user == null)
            return null;
        return user;
    }

    public static async Task<(UserApp? user, bool isNull)> CheckUserNullByEmail(
        string email,
        UserManager<UserApp> _userManager
    )
    {
        var user = await _userManager.FindByEmailAsync(email);

        return (user, user is null);
    }

    public static async Task<(UserApp? user, bool isNull)> CheckUserNullByUserId(
        string id,
        UserManager<UserApp> _userManager
    )
    {
        var user = await _userManager.FindByIdAsync(id);

        return (user, user is null);
    }

}

