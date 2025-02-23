using Data_Organizer_Core;
using Data_Organizer_Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Data_Organizer_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/openai")]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public OpenAIController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("summary")]
        public async Task<IActionResult> GetSummary([FromBody] SummaryRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Порожній запит або відсутній контент!");
            }

            try
            {
                request.Result = await _openAIService.GetSummary(request.Content);
                return Ok(request);
            }
            catch (Exception ex)
            {
                request.Error = ex.Message;
                return StatusCode(500, request);
            }
        }
    }
}
