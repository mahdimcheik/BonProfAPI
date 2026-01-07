using Microsoft.EntityFrameworkCore;
using BonProf.Contexts;
using BonProf.Models;

namespace BonProf.Services;

/// <summary>
/// Service pour la gestion des r�les
/// </summary>
public class RolesService(MainContext context)
{
    /// <summary>
    /// R�cup�re tous les r�les
    /// </summary>
    /// <returns>Liste des r�les</returns>
    public async Task<Response<List<RoleDetails>>> GetAllRolesAsync()
    {
        try
        {
            var roles = await context
                .Roles
                .AsNoTracking()
                .Where(r => r.ArchivedAt == null)
                .OrderBy(r => r.Name)
                .Select(r => new RoleDetails(r))
                .ToListAsync();

            return new Response<List<RoleDetails>>
            {
                Status = 200,
                Message = "R�les r�cup�r�s avec succ�s",
                Data = roles,
                Count = roles.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<RoleDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration des r�les: {ex.Message}",
                Data = null,
            };
        }
    }
}
