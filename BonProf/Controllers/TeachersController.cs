using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BonProf.Models;
using BonProf.Services;

namespace BonProf.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class TeachersController : ControllerBase
{
    private readonly TeacherService _teacherProfileService;

    public TeachersController(TeacherService teacherProfileService)
    {
        _teacherProfileService = teacherProfileService;
    }
    [HttpGet("all")]
    public async Task<ActionResult<Response<List<UserDetails>>>> GetAllTeacherProfiles()
    {
        var response = await _teacherProfileService.GetAllTeacherProfilesAsync();

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpGet("my-profile")]
    [Authorize]
    public async Task<ActionResult<Response<UserDetails>>> GetMyTeacherProfile()
    {
        var response = await _teacherProfileService.GetTeacherFullProfileAsync( User);
        return StatusCode(response.Status, response);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<Response<UserDetails>>> GetTeacherProfileByUserId(
        [FromRoute] Guid userId)
    {
        var response = await _teacherProfileService.GetTeacherProfileByUserIdAsync(userId);

        if (response.Status == 200)
        {
            return Ok(response);
        }

        return StatusCode(response.Status, response);
    }

    [HttpPut("update-profile")]
    [Authorize(Roles = "Teacher")]
    public async Task<ActionResult<Response<UserDetails>>> UpdateTeacherProfile(
        [FromBody] UserUpdate teacherUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new Response<object>
            {
                Status = 400,
                Message = "Donn�es de validation invalides",
                Data = ModelState
            });
        }

        var response = await _teacherProfileService.UpdateTeacherProfileAsync(teacherUpdateDto, User);

        return StatusCode(response.Status, response);
    }
}
