using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BonProf.Models;
using BonProf.Services;

namespace BonProf.Controllers;

/// <summary>
/// Contr�leur pour la gestion des r�les
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class RolesController(RolesService rolesService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<Response<List<RoleDetails>>>> GetAllRoles()
    {
        var response = await rolesService.GetAllRolesAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }
}
