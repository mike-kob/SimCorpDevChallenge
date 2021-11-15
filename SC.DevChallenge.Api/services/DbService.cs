using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using SC.DevChallenge.Api.Models;
using SC.DevChallenge.Api.Utils;

namespace SC.DevChallenge.Api.Services
{
  public class DbService
  {
    public List<PriceValues> values;

    public DbService()
    {
      values = File.ReadAllLines("./Input/data.csv")
                                      .Skip(1)
                                      .Select(v => PriceValues.FromCsv(v))
                                      .ToList();
    }

    public decimal GetFilteredAvg(String owner,  String portfolio, String instrument, int ts) {
      var dateStart = TimeslotManager.TsToDt(ts);
      var dateEnd = TimeslotManager.TsToDt(ts + 1);                               

      var res = values.Where((x) => x.Date >= dateStart && x.Date < dateEnd);
      if (owner != null) {
        res = res.Where((x) => x.Owner == owner);
      }
      if (portfolio != null) {
        res = res.Where((x) => x.Portfolio == portfolio);
      }
      if (instrument != null) {
        res = res.Where((x) => x.Instrument == instrument);
      }
    
      return res.Select((x) => x.Price).Average();
    }
  }
}