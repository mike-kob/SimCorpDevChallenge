using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;

using SC.DevChallenge.Api.Services;
using SC.DevChallenge.Api.Utils;
using SC.DevChallenge.Api.Models;

namespace SC.DevChallenge.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PricesController : ControllerBase
  {
    private static string DATE_FMT = "dd/MM/yyyy HH:mm:ss";
    private PriceService _dbService;

    public PricesController(PriceService dbService)
    {
      _dbService = dbService;
    }

    /// <summary>
    /// Returns simple average of prices with specified conditions
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="portfolio"></param>
    /// <param name="instrument"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    [HttpGet("average")]
    public IActionResult Average([FromQuery] string owner, [FromQuery] string portfolio, [FromQuery] string instrument, [FromQuery] string date)
    {
      if (date == null)
      {
        return BadRequest();
      }
      try
      {
        var datePared = DateTime.ParseExact(date, DATE_FMT, null);
        var ts = TimeslotManager.DtToTs(datePared);

        return Ok(new
        {
          Date = TimeslotManager.TsToDt(ts).ToString(DATE_FMT),
          Price = _dbService.GetFilteredAvg(ts, owner?.ToLower(), portfolio?.ToLower(), instrument?.ToLower())
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

    /// <summary>
    /// Returns benchmark (price average without outliers) with the specified
    /// conditions.
    /// </summary>
    /// <param name="portfolio">The portfolio for which benchmark is calculated</param>
    /// <param name="date">The date that falls into the time interval for which benchmark is calculated</param>
    /// <returns></returns>
    [HttpGet("benchmark")]
    public IActionResult Benchmark([FromQuery] string portfolio, [FromQuery] string date)
    {
      var dateParsed = DateTime.ParseExact(date, DATE_FMT, null);
      var ts = TimeslotManager.DtToTs(dateParsed);

      var vals = _dbService.GetFiltered(ts, portfolio: portfolio?.ToLower())
        .OrderBy(x => x.Price).Select(x => x.Price).ToArray();

      if (vals.Count() == 0) {
        return NotFound();
      }

      var avgPrice = StatisticsManager.GetBenchmark(vals);

      return Ok(new
      {
        Date = TimeslotManager.TsToDt(ts).ToString(DATE_FMT),
        Price = avgPrice
      });
    }

    /// <summary>
    /// Aggregates prices into specified number of intervals according
    /// to provided conditinons.
    /// </summary>
    /// <param name="portfolio">Filter by portfolio</param>
    /// <param name="startdate">The date that falls into the first time interval</param>
    /// <param name="enddate">The date that falls into the last time interval</param>
    /// <param name="intervals">Number of groups to divide all intervals in</param>
    /// <returns></returns>
    [HttpGet("aggregate")]
    public IActionResult Aggregate([FromQuery] string portfolio, [FromQuery] string startdate,
              [FromQuery] string enddate, [FromQuery] int intervals)
    {
      var startdateParsed = DateTime.ParseExact(startdate, DATE_FMT, null);
      var enddateParsed = DateTime.ParseExact(enddate, DATE_FMT, null);

      var tsStart = TimeslotManager.DtToTs(startdateParsed);
      var tsEndNext = TimeslotManager.DtToTs(enddateParsed) + 1;

      var allVals = _dbService.GetFiltered(
        dateStart: TimeslotManager.TsToDt(tsStart),
        dateEnd: TimeslotManager.TsToDt(tsEndNext),
        portfolio: portfolio.ToLower()
      );
      if (allVals.Count() == 0)
      {
        return NotFound();
      }

      var totalTs = tsEndNext - tsStart;
      int groupSize = totalTs / intervals;
      int numberOfGroupExtra = totalTs % intervals;

      var li = new List<BenchmarkResult>();
      while (tsStart < tsEndNext)
      {
        var tsNext = tsStart + groupSize;
        if (numberOfGroupExtra > 0)
        {
          tsNext++;
          numberOfGroupExtra--;
        }
        var dtStart = TimeslotManager.TsToDt(tsStart);
        var dtNext = TimeslotManager.TsToDt(tsNext);
        var groupData = _dbService.GetFiltered(dtStart, dtNext, portfolio: portfolio.ToLower())
          .OrderBy(x => x.Price)
          .Select(x => x.Price)
          .ToArray();

        if (groupData.Count() == 0)
        {
          li.Add(new BenchmarkResult
          {
            Error = "Not found"
          });
        }
        else
        {
          var benchmark = StatisticsManager.GetBenchmark(groupData);
          li.Add(new BenchmarkResult
          {
            Date = TimeslotManager.TsToDt(tsNext - 1).ToString(DATE_FMT),
            Price = benchmark,
          });
        }

        tsStart = tsNext;
      }

      return Ok(li);
    }
  }
}
