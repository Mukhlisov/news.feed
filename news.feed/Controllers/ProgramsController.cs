using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using news.feed.models.Policies;
using news.feed.Services.Programs;

namespace news.feed.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProgramsController : ControllerBase
{
    private readonly IProgramsService _programsService;

    public ProgramsController(IProgramsService programsService)
    {
        _programsService = programsService;
    }

    [EnableCors(nameof(Policies.GetNewsPolicy))]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Program>>> GetAllPrograms()
    {
        var programs = await _programsService.GetAllProgramsAsync().ConfigureAwait(false);
        return Ok(programs);
    }
}
