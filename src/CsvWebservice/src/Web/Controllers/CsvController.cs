using Bison.CsvWebservice.Application.Handlers.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bison.CsvWebservice.Web.Controllers;

[ApiController]
[Route("/api/csv")]
public class CsvController : ControllerBase
{
    private readonly ILogger<CsvController> _logger;
    private ISender _sender;

    public CsvController(ILogger<CsvController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet]
    public async IAsyncEnumerable<int> Index()
    {
		for (int i = 0; i < 10; i++)
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
			yield return i;
		}
	}

    [HttpPost]
    [Consumes("text/csv")]
    public async Task<IActionResult> ParseCsv()
    {
        _logger.LogDebug("Let's start process a new csv !");
        try
        {
            CsvParsedReport report = await _sender.Send(new CsvParseCommand()
            {
                Content = Request.Body,
                ContentLength = Request.ContentLength ?? 0
            });
            return Ok(report);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (Exception e)
        {
            _logger.LogError(e, e.Message);
			return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the CSV file. Please try again later.");
		}
        

	}
}
