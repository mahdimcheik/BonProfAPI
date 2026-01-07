using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BonProf.Models;
using BonProf.Services;

namespace BonProf.Controllers;

/// <summary>
/// Contr�leur pour la gestion des cr�neaux
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[Route("[controller]")]
[ApiController]
[EnableCors]
public class SlotsController(SlotsService slotsService) : ControllerBase
{
    [Authorize(Roles = "Teacher")]
    [HttpPost("teacher/add")]
    public async Task<ActionResult<Response<SlotDetails>>> AddSlotByTeacher(
        [FromBody] SlotCreate slotDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new Response<object>
                {
                    Status = 400,
                    Message = "Donn�es de validation invalides",
                    Data = ModelState,
                }
            );
        }

        var response = await slotsService.AddSlotByTeacherAsync(slotDto, User);

        return StatusCode(response.Status, response);
    }
    [Authorize(Roles = "Teacher")]
    [HttpPut("teacher/update")]
    public async Task<ActionResult<Response<SlotDetails>>> UpdateSlotByTeacher(
        [FromBody] SlotUpdate slotDto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                new Response<object>
                {
                    Status = 400,
                    Message = "Donn�es de validation invalides",
                    Data = ModelState,
                }
            );
        }

        var response = await slotsService.UpdateSlotByTeacherAsync(slotDto, User);

        return StatusCode(response.Status, response);
    }

    [Authorize(Roles = "Teacher")]
    [HttpDelete("teacher/remove/{slotId:guid}")]
    public async Task<ActionResult<Response<bool>>> RemoveSlotByTeacher(
        [FromRoute] Guid slotId
    )
    {
        var response = await slotsService.RemoveSlotByTeacherAsync(slotId, User);

        return StatusCode(response.Status, response);
    }

    [Authorize(Roles = "Teacher")]
    [HttpPost("teacher/my-slots")]
    public async Task<ActionResult<Response<List<SlotDetails>>>> GetSlotsByTeacher(
        [FromBody] PeriodTime periodTime
    )
    {
        var response = await slotsService.GetSlotsByTeacherAndDatesAsync(User, periodTime.DateFrom, periodTime.DateTo);

        return StatusCode(response.Status, response);
    }

    [AllowAnonymous]
    [HttpGet("teacher/{teacherId:guid}/available-slots")]
    public async Task<ActionResult<Response<List<SlotDetails>>>> GetAvailableSlotsByTeacher(
        [FromRoute] Guid teacherId,
        [FromQuery] DateTimeOffset dateFrom,
        [FromQuery] DateTimeOffset dateTo
    )
    {
        var response = await slotsService.GetSlotsByTeacherIdAndDatesAsync(
            teacherId,
            dateFrom,
            dateTo
        );

        return StatusCode(response.Status, response);
    }
}
