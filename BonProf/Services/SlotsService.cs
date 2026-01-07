using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using BonProf.Contexts;
using BonProf.Models;
using BonProf.Utilities;

namespace BonProf.Services;

/// <summary>
/// Service pour la gestion des cr�neaux
/// </summary>
public class SlotsService(MainContext context)
{
    /// <summary>
    /// Ajoute un nouveau cr�neau pour un enseignant
    /// </summary>
    /// <param name="slotDto">Donn�es du cr�neau � cr�er</param>
    /// <param name="userPrincipal">Principal de l'utilisateur connect�</param>
    /// <returns>Cr�neau cr��</returns>
    public async Task<Response<SlotDetails>> AddSlotByTeacherAsync(
        SlotCreate slotDto,
        ClaimsPrincipal userPrincipal
    )
    {
        try
        {
            // R�cup�rer l'utilisateur connect�
            var user = CheckUser.GetUserFromClaim(userPrincipal, context);
            if (user == null)
            {
                return new Response<SlotDetails>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifi�",
                    Data = null,
                };
            }

            // V�rifier que l'utilisateur est bien un enseignant
            var teacher = await context.Teachers.FirstOrDefaultAsync(t =>
                t.UserId == user.Id
            );

            if (teacher == null)
            {
                return new Response<SlotDetails>
                {
                    Status = 403,
                    Message = "Vous devez �tre un enseignant pour cr�er des cr�neaux",
                    Data = null,
                };
            }

            // Validation des dates
            if (slotDto.DateFrom >= slotDto.DateTo)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "La date de fin doit �tre post�rieure � la date de d�but",
                    Data = null,
                };
            }

            // V�rifier que le cr�neau est dans le futur
            if (slotDto.DateFrom < DateTimeOffset.UtcNow)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Le cr�neau doit �tre dans le futur",
                    Data = null,
                };
            }

            // V�rifier que le type de cr�neau existe

            var typeExists = await context.TypeSlots.AnyAsync(t =>
                t.Id == slotDto.TypeId && t.ArchivedAt == null
            );
            if (!typeExists)
            {
                return new Response<SlotDetails>
                {
                    Status = 404,
                    Message = "Type de cr�neau non trouv�",
                    Data = null,
                };
            }

            // V�rifier qu'il n'y a pas de chevauchement avec un autre cr�neau du m�me enseignant
            var hasOverlap = await context.Slots.AnyAsync(s =>
                s.TeacherId == teacher.Id
                && s.ArchivedAt == null
                && (
                    (slotDto.DateFrom >= s.DateFrom && slotDto.DateFrom < s.DateTo)
                    || (slotDto.DateTo > s.DateFrom && slotDto.DateTo <= s.DateTo)
                    || (slotDto.DateFrom <= s.DateFrom && slotDto.DateTo >= s.DateTo)
                )
            );

            if (hasOverlap)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Ce cr�neau chevauche un cr�neau existant",
                    Data = null,
                };
            }

            var slot = new Slot
            {
                Id = Guid.NewGuid(),
                DateFrom = slotDto.DateFrom,
                DateTo = slotDto.DateTo,
                TeacherId = teacher.Id,
                TypeId = slotDto.TypeId,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            context.Slots.Add(slot);
            await context.SaveChangesAsync();

            // Recharger avec les relations pour la r�ponse
            var createdSlot = await context
                .Slots.Include(s => s.Teacher)
                .ThenInclude(t => t.User)
                .Include(s => s.Type)
                .FirstAsync(s => s.Id == slot.Id);

            return new Response<SlotDetails>
            {
                Status = 201,
                Message = "Cr�neau cr�� avec succ�s",
                Data = new SlotDetails(createdSlot),
            };
        }
        catch (Exception ex)
        {
            return new Response<SlotDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la cr�ation du cr�neau: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Ajoute un nouveau cr�neau pour un enseignant
    /// </summary>
    /// <param name="slotDto">Donn�es du cr�neau � cr�er</param>
    /// <param name="userPrincipal">Principal de l'utilisateur connect�</param>
    /// <returns>Cr�neau cr��</returns>
    public async Task<Response<SlotDetails>> UpdateSlotByTeacherAsync(
        SlotUpdate slotDto,
        ClaimsPrincipal userPrincipal
    )
    {
        try
        {
            // R�cup�rer l'utilisateur connect�
            var user = CheckUser.GetUserFromClaim(userPrincipal, context);
            if (user == null)
            {
                return new Response<SlotDetails>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifi�",
                    Data = null,
                };
            }

            // V�rifier que l'utilisateur est bien un enseignant
            var teacher = await context.Teachers            .FirstOrDefaultAsync(t =>
                t.UserId == user.Id
            );

            if (teacher == null)
            {
                return new Response<SlotDetails>
                {
                    Status = 403,
                    Message = "Vous devez �tre un enseignant pour cr�er des cr�neaux",
                    Data = null,
                };
            }

            // Validation des dates
            if (slotDto.DateFrom >= slotDto.DateTo)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "La date de fin doit �tre post�rieure � la date de d�but",
                    Data = null,
                };
            }

            // V�rifier que le cr�neau est dans le futur
            if (slotDto.DateFrom < DateTimeOffset.UtcNow)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Le cr�neau doit �tre dans le futur",
                    Data = null,
                };
            }

            // V�rifier que le type de cr�neau existe

            var typeExists = await context.TypeSlots.AnyAsync(t =>
                t.Id == slotDto.TypeId && t.ArchivedAt == null
            );
            if (!typeExists)
            {
                return new Response<SlotDetails>
                {
                    Status = 404,
                    Message = "Type de cr�neau non trouv�",
                    Data = null,
                };
            }

            // V�rifier qu'il n'y a pas de chevauchement avec un autre cr�neau du m�me enseignant
            var hasOverlap = await context.Slots.AnyAsync(s =>
                s.TeacherId == teacher.Id
                && s.Id != slotDto.Id
                && s.ArchivedAt == null
                && (
                    (slotDto.DateFrom >= s.DateFrom && slotDto.DateFrom < s.DateTo)
                    || (slotDto.DateTo > s.DateFrom && slotDto.DateTo <= s.DateTo)
                    || (slotDto.DateFrom <= s.DateFrom && slotDto.DateTo >= s.DateTo)
                )
            );

            if (hasOverlap)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Ce cr�neau chevauche un cr�neau existant",
                    Data = null,
                };
            }

            var slot = await context.Slots.FirstOrDefaultAsync(x => x.Id == slotDto.Id);

            if (slot is null)
            {
                return new Response<SlotDetails>
                {
                    Status = 400,
                    Message = "Ce cr�neau n'existe pas/plus",
                    Data = null,
                };
            }
            slotDto.UpdateSlot(slot);
            await context.SaveChangesAsync();

            return new Response<SlotDetails>
            {
                Status = 201,
                Message = "Cr�neau cr�� avec succ�s",
                Data = new SlotDetails(slot),
            };
        }
        catch (Exception ex)
        {
            return new Response<SlotDetails>
            {
                Status = 500,
                Message = $"Erreur lors de la cr�ation du cr�neau: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// Supprime un cr�neau pour un enseignant (suppression logique)
    /// </summary>
    /// <param name="slotId">Identifiant du cr�neau</param>
    /// <param name="userPrincipal">Principal de l'utilisateur connect�</param>
    /// <returns>R�sultat de l'op�ration</returns>
    public async Task<Response<bool>> RemoveSlotByTeacherAsync(
        Guid slotId,
        ClaimsPrincipal userPrincipal
    )
    {
        try
        {
            // R�cup�rer l'utilisateur connect�
            var user = CheckUser.GetUserFromClaim(userPrincipal, context);
            if (user == null)
            {
                return new Response<bool>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifi�",
                    Data = false,
                };
            }

            // V�rifier que l'utilisateur est bien un enseignant
            var teacher = await context.Teachers.FirstOrDefaultAsync(t =>
                t.UserId == user.Id
            );

            if (teacher == null)
            {
                return new Response<bool>
                {
                    Status = 403,
                    Message = "Vous devez �tre un enseignant pour supprimer des cr�neaux",
                    Data = false,
                };
            }

            var slot = await context
                .Slots.Include(s => s.Reservation)
                .FirstOrDefaultAsync(s => s.Id == slotId && s.ArchivedAt == null);

            if (slot == null)
            {
                return new Response<bool>
                {
                    Status = 404,
                    Message = "Cr�neau non trouv�",
                    Data = false,
                };
            }

            // V�rifier que le cr�neau appartient bien � l'enseignant
            if (slot.TeacherId != teacher.Id)
            {
                return new Response<bool>
                {
                    Status = 403,
                    Message = "Vous n'�tes pas autoris� � supprimer ce cr�neau",
                    Data = false,
                };
            }

            // V�rifier que le cr�neau n'est pas d�j� r�serv�
            if (slot.Reservation != null)
            {
                return new Response<bool>
                {
                    Status = 400,
                    Message = "Impossible de supprimer un cr�neau r�serv�",
                    Data = false,
                };
            }

            slot.ArchivedAt = DateTimeOffset.UtcNow;
            slot.UpdatedAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();

            return new Response<bool>
            {
                Status = 200,
                Message = "Cr�neau supprim� avec succ�s",
                Data = true,
            };
        }
        catch (Exception ex)
        {
            return new Response<bool>
            {
                Status = 500,
                Message = $"Erreur lors de la suppression du cr�neau: {ex.Message}",
                Data = false,
            };
        }
    }

    /// <summary>
    /// R�cup�re les cr�neaux d'un enseignant entre deux dates
    /// </summary>
    /// <param name="userPrincipal">Principal de l'utilisateur connect�</param>
    /// <param name="dateFrom">Date de d�but</param>
    /// <param name="dateTo">Date de fin</param>
    /// <returns>Liste des cr�neaux</returns>
    public async Task<Response<List<SlotDetails>>> GetSlotsByTeacherAndDatesAsync(
        ClaimsPrincipal userPrincipal,
        DateTimeOffset dateFrom,
        DateTimeOffset dateTo
    )
    {
        try
        {
            // R�cup�rer l'utilisateur connect�
            var user = CheckUser.GetUserFromClaim(userPrincipal, context);
            if (user == null)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 401,
                    Message = "Utilisateur non authentifi�",
                    Data = null,
                };
            }

            // V�rifier que l'utilisateur est bien un enseignant
            var teacher = await context.Teachers.FirstOrDefaultAsync(t =>
                t.UserId == user.Id
            );

            if (teacher == null)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 403,
                    Message = "Vous devez �tre un enseignant pour consulter des cr�neaux",
                    Data = null,
                };
            }

            // Validation des dates
            if (dateFrom >= dateTo)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 400,
                    Message = "La date de fin doit �tre post�rieure � la date de d�but",
                    Data = null,
                };
            }

            var slots = await context
                .Slots.AsNoTracking()
                .Include(s => s.Teacher)
                .ThenInclude(t => t.User)
                .Include(s => s.Type)
                .Where(s =>
                    s.TeacherId == teacher.Id
                    && s.ArchivedAt == null
                    && s.DateFrom >= dateFrom
                    && s.DateTo <= dateTo
                )
                .OrderBy(s => s.DateFrom)
                .Select(s => new SlotDetails(s))
                .ToListAsync();

            return new Response<List<SlotDetails>>
            {
                Status = 200,
                Message = "Cr�neaux r�cup�r�s avec succ�s",
                Data = slots,
                Count = slots.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<SlotDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration des cr�neaux: {ex.Message}",
                Data = null,
            };
        }
    }

    /// <summary>
    /// R�cup�re les cr�neaux d'un enseignant sp�cifique entre deux dates (pour consultation publique)
    /// </summary>
    /// <param name="teacherId">Identifiant de l'enseignant</param>
    /// <param name="dateFrom">Date de d�but</param>
    /// <param name="dateTo">Date de fin</param>
    /// <returns>Liste des cr�neaux disponibles</returns>
    public async Task<Response<List<SlotDetails>>> GetSlotsByTeacherIdAndDatesAsync(
        Guid teacherId,
        DateTimeOffset dateFrom,
        DateTimeOffset dateTo
    )
    {
        try
        {
            // V�rifier que l'enseignant existe
            var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);

            if (teacher == null)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 404,
                    Message = "Enseignant non trouv�",
                    Data = null,
                };
            }

            // Validation des dates
            if (dateFrom >= dateTo)
            {
                return new Response<List<SlotDetails>>
                {
                    Status = 400,
                    Message = "La date de fin doit �tre post�rieure � la date de d�but",
                    Data = null,
                };
            }

            var slots = await context
                .Slots.AsNoTracking()
                .Include(s => s.Teacher)
                .ThenInclude(t => t.User)
                .Include(s => s.Type)
                .Include(s => s.Reservation)
                .Where(s =>
                    s.TeacherId == teacherId
                    && s.ArchivedAt == null
                    && s.DateFrom >= dateFrom
                    && s.DateTo <= dateTo
                    && s.Reservation == null // Seulement les cr�neaux disponibles
                )
                .OrderBy(s => s.DateFrom)
                .Select(s => new SlotDetails(s))
                .ToListAsync();

            return new Response<List<SlotDetails>>
            {
                Status = 200,
                Message = "Cr�neaux disponibles r�cup�r�s avec succ�s",
                Data = slots,
                Count = slots.Count,
            };
        }
        catch (Exception ex)
        {
            return new Response<List<SlotDetails>>
            {
                Status = 500,
                Message = $"Erreur lors de la r�cup�ration des cr�neaux: {ex.Message}",
                Data = null,
            };
        }
    }
}
