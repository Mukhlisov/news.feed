using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using news.feed.Auth;
using news.feed.models.Policies;
using news.feed.Repository;

namespace news.feed.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AttachmentsController : ControllerBase
{
    private readonly IAttachmentsRepository _attachmentsRepository;
    public AttachmentsController(IAttachmentsRepository attachmentsRepository)
    {
        _attachmentsRepository = attachmentsRepository;
    }

    [Auth]
    [EnableCors(nameof(Policies.AdminPanelPolicy))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAttachmentByIdAsync(Guid id)
    {
        await _attachmentsRepository.DeleteAttachmentByIdAsync(id).ConfigureAwait(false);
        return NoContent();
    }
}