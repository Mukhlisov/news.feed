using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using news.feed.Auth;
using news.feed.models.Policies;
using news.feed.Repository;

namespace news.feed.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AttachmentsController : ApiControllerBase<AttachmentsController>
{
    private readonly IAttachmentsRepository _attachmentsRepository;
    public AttachmentsController(
        IAttachmentsRepository attachmentsRepository, 
        ILogger<AttachmentsController> logger) 
        : base(logger)
    {
        _attachmentsRepository = attachmentsRepository;
    }

    [Auth]
    [EnableCors(nameof(Policies.AdminPanelPolicy))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAttachmentByIdAsync(Guid id)
    {
        try
        {
            await _attachmentsRepository.DeleteAttachmentByIdAsync(id).ConfigureAwait(false);
            return NoContent();
        }
        catch (Exception ex)
        {
            return HandleHttpError(ex);
        }
    }
}