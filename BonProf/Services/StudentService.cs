using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BonProf.Contexts;
using BonProf.Models;
using BonProf.Utilities;
using System;
using System.Security.Claims;

namespace BonProf.Services;


public class StudentService
{
    private readonly MainContext _context;
    private readonly UserManager<UserApp> _userManager;

    public StudentService(MainContext context, UserManager<UserApp> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Response<UserDetails>> GetTStudentProfileAsync(ClaimsPrincipal principal)
    {
        try
        {
            var user = CheckUser.GetUserFromClaim(principal, _context);
            if (user is null)
            {
                return new Response<UserDetails>
                {
                    Status = 404,
                    Message = "Profil enseignant non trouv�",
                };
            }
            var teacher = await _context
                .Users
                .Where(p => p.Id == user.Id)
                .Include(p => p.Gender)
                .Include(p => p.Languages)
                .Include(p => p.Student)
                // .Include(p => p.Addresses)
                .FirstOrDefaultAsync();
            
            if (teacher == null)
            {
                return new Response<UserDetails>
                {
                    Status = 404,
                    Message = "Profil enseignant non trouvé",
                };
            }
            return new Response<UserDetails>
            {
                Status = 200,
                Message = "Profil enseignant r�cup�r� avec succès",
                Data = new UserDetails(teacher, null),
            };
        }
        catch (Exception ex)
        {
            return new Response<UserDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration du profil: {ex.Message}",
            };
        }
    }

    /// <summary>
    /// R�cup�re un profil enseignant par l'identifiant de l'utilisateur
    /// </summary>
    public async Task<Response<UserDetails>> GetStudentByUserIdAsync(Guid userId)
    {
        try
        {
            var teacher = await _context
                .Users
                .Where(p => p.Id == userId)
                .Include(p => p.Student)
                .Include(p => p.Gender)
                .Include(p => p.Languages)
                .FirstOrDefaultAsync();
            if (teacher == null)
            {
                return new Response<UserDetails>
                {
                    Status = 404,
                    Message = "Profil enseignant non trouv�",
                };
            }

            return new Response<UserDetails>
            {
                Status = 200,
                Message = "Profil enseignant r�cup�r� avec succ�s",
                Data = new UserDetails(teacher, null),
            };
        }
        catch (Exception ex)
        {
            return new Response<UserDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration du profil: {ex.Message}",
            };
        }
    }

    public async Task<Response<UserDetails>> UpdateStudentProfileAsync(
        UserUpdate userUpdate,
        ClaimsPrincipal userPrincipal
    )
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = CheckUser.GetUserFromClaim(userPrincipal, _context);
            if (user == null)
            {
                return new Response<UserDetails>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifié",
                    Data = null,
                };
            }

            var student = await _context
                .Users
                .Include(t => t.Student)
                .Include(p => p.Languages)
                .FirstOrDefaultAsync(t => t.Id == user.Id);

            if (student == null)
            {
                return new Response<UserDetails>
                {
                    Status = 404,
                    Message = "Profil élève non trouvé",
                    Data = null,
                };
            }

            var languages = await _context.Languages.ToListAsync();

            userUpdate.UpdateUser(user, languages);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var updatedUser = await _context
                .Users
                .Include(p => p.Gender)
                .Include(p => p.Languages)
                .Include(p => p.Formations)
                .Include(p => p.Addresses)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return new Response<UserDetails>
            {
                Status = 200,
                Message = "Profil enseignant mis à jour avec succ�s",
                Data = new UserDetails(updatedUser!, null),
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new Response<UserDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la mise à jour du profil: {ex.Message}",
                Data = null,
            };
        }
    }
}
