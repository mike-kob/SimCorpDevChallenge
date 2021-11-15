using Microsoft.AspNetCore.Mvc;

using System;

using SC.DevChallenge.Api.Services;
using SC.DevChallenge.Api.Utils;

namespace SC.DevChallenge.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PricesController : ControllerBase
  {
    private DbService _dbService;

    public PricesController(DbService dbService)
    {
      _dbService = dbService;
    }

    [HttpGet("average")]
    public IActionResult Average([FromQuery] string owner, [FromQuery] string portfolio, [FromQuery] string instrument, [FromQuery] string date)
    {
      if (date == null)
      {
        return BadRequest();
      }
      try
      {
        var datePared = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", null);
        var ts = TimeslotManager.DtToTs(datePared);
        Console.WriteLine(owner + " " + portfolio + " " + instrument + " " + ts);
        return Ok(new {
          Date = TimeslotManager.TsToDt(ts).ToString("dd/MM/yyyy HH:mm:ss"),
          Price =  _dbService.GetFilteredAvg(owner?.ToLower(), portfolio?.ToLower(), instrument?.ToLower(), ts)
        });
      }
      catch (FormatException)
      {
        return BadRequest();
      }
      catch (InvalidOperationException)
      {
        return NotFound();
      }
    }
  }
}
