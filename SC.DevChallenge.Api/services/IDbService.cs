using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using SC.DevChallenge.Api.Models;
using SC.DevChallenge.Api.Utils;

namespace SC.DevChallenge.Api.Services
{
  public abstract class BasePriceService
  {
    public List<PriceValues> values;

    public decimal GetFilteredAvg(int ts,
              String owner = null, String portfolio = null, String instrument = null)
    {
      return this.GetFiltered(ts, owner, portfolio, instrument)
        .Select((x) => x.Price).Average();
    }

    public IEnumerable<PriceValues> GetFiltered(int ts,
              String owner = null, String portfolio = null, String instrument = null)
    {
      var dateStart = TimeslotManager.TsToDt(ts);
      var dateEnd = TimeslotManager.TsToDt(ts + 1);

      return this.GetFiltered(dateStart, dateEnd, owner, portfolio, instrument);
    }

    public IEnumerable<PriceValues> GetFiltered(DateTime dateStart, DateTime dateEnd,
              String owner = null, String portfolio = null, String instrument = null)
    {
      var res = values.Where((x) => x.Date >= dateStart && x.Date < dateEnd);
      if (owner != null)
      {
        res = res.Where((x) => x.Owner == owner);
      }
      if (portfolio != null)
      {
        res = res.Where((x) => x.Portfolio == portfolio);
      }
      if (instrument != null)
      {
        res = res.Where((x) => x.Instrument == instrument);
      }

      return res;
    }
  }
}